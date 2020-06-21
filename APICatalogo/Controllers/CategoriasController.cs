using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {

        //injecao de dependencia da classe DbContext
        //isso é possivel pq na classe Startup.cs foi injetada a 'services.AddDbContext'
        private readonly AppDbContext _context;
        public CategoriasController(AppDbContext contexto)
        {
            _context = contexto;
        }

        //servico MeuServico [FromServices]
        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuservico, string nome)
        {
            return meuservico.Saudacao(nome);
        }

        //IEnumerable de Produto pq retornar uma Lista de Produtos
        //ActionResult retornar os codigos http 200, 400, etc
        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            //return _context.Produtos.ToList();

            //para melhorar o desempenho e nao verificar o estado, utiliza-se o AsNoTracking
            //Só funciona para metodos Get
            return _context.Categorias.AsNoTracking().ToList();
        }

        //retorna os produtos dentro da categoria (se chamar categoria, produto vem vazio "Produto[]")
        //utilizar o Include
        //Tem que criar nova rota (endpoint)
        //se der erro de referencia ciclica ( cycle was detected) é porque as classes
        // categoria esta referenciando produto e produto esta referenciando categoria
        // ver essa linha de codigo no Startup.cs :
        //          .AddNewtonsoftJson(options => //se der erro nessa linha, tem que referenciar o Microsoft.AspNetCore.Mvc.NewtonsoftJson
        //                              options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        [HttpGet("produtos")] //ficara api/categorias/produtos
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _context.Categorias.Include(p => p.Produtos).ToList();
        }

        [HttpGet("{id}", Name = "ObterCategoria")] //vincula uma rota nomeada (Name) ao Http retorno do post
        public ActionResult<Categoria> Get(int id)
        {
            //var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            //para melhorar o desempenho e nao verificar o estado, utiliza-se o AsNoTracking
            //Só funciona para metodos Get
            var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(p => p.CategoriaID == id);

            if (categoria == null)
                return NotFound(); //404

            return categoria;
        }



        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria) //sempre FromBody para recuperar os dados do Body (quem faz isso é o Model Binding)
        {
            //a partir da versao 2.1 nao precisa mais fazer!!!
            //pq o atributo [ApiController] já faz isso automaticamente
            //dados da requisicao validos?
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            _context.Categorias.Add(categoria); //esta em memoria o Add
            _context.SaveChanges(); //como se fosse um commit

            //http response, após inclusao do produto, retorna o ID
            return new CreatedAtRouteResult("ObterCategoria", //retorna uma rota 
                                            new { id = categoria.CategoriaID }, //parametros da Action 
                                            categoria );  //corpo da requisicao (Body)
        }


        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            //a partir da versao 2.1 nao precisa mais fazer!!!
            //pq o atributo [ApiController] já faz isso automaticamente
            //dados da requisicao validos?
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if(id != categoria.CategoriaID)
                return BadRequest();

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();

        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaID == id);

            //pode usar o Find tbm (só pode usar se a busca for pela PK)
            //a vantagem do Find é que ele primeiro busca na memória e depois no banco de dados
            //diferente do FirstOrDefault que ja vai direto no BD
            //var produto = _context.Find(id);


            if (categoria == null)
                return NotFound(); //404

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();
            return categoria;
        }

    }
}
