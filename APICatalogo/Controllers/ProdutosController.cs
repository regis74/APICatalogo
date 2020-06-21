using APICatalogo.Context;
using APICatalogo.Models;
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
    public class ProdutosController : ControllerBase
    {

        //injecao de dependencia da classe DbContext
        //isso é possivel pq na classe Startup.cs foi injetada a 'services.AddDbContext'
        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext contexto)
        {
            _context = contexto;
        }

        //IEnumerable de Produto pq retornar uma Lista de Produtos
        //ActionResult retornar os codigos http 200, 400, etc
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            //return _context.Produtos.ToList();

            //para melhorar o desempenho e nao verificar o estado, utiliza-se o AsNoTracking
            //Só funciona para metodos Get
            return _context.Produtos.AsNoTracking().ToList();
        }

        [HttpGet("{id}", Name = "ObterProduto")] //vincula uma rota nomeada (Name) ao Http retorno do post
        public ActionResult<Produto> Get(int id)
        {
            //var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            //para melhorar o desempenho e nao verificar o estado, utiliza-se o AsNoTracking
            //Só funciona para metodos Get
            var produto = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);

            if (produto == null)
                return NotFound(); //404

            return produto;
        }

        [HttpPost]
        public ActionResult Post([FromBody]Produto produto) //sempre FromBody para recuperar os dados do Body (quem faz isso é o Model Binding)
        {
            //a partir da versao 2.1 nao precisa mais fazer!!!
            //pq o atributo [ApiController] já faz isso automaticamente
            //dados da requisicao validos?
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            _context.Produtos.Add(produto); //esta em memoria o Add
            _context.SaveChanges(); //como se fosse um commit

            //http response, após inclusao do produto, retorna o ID
            return new CreatedAtRouteResult("ObterProduto", //retorna uma rota 
                                            new { id = produto.ProdutoId }, //parametros da Action 
                                            produto );  //corpo da requisicao (Body)



        }


        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Produto produto)
        {
            //a partir da versao 2.1 nao precisa mais fazer!!!
            //pq o atributo [ApiController] já faz isso automaticamente
            //dados da requisicao validos?
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if(id != produto.ProdutoId)
                return BadRequest();

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();

        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            //pode usar o Find tbm (só pode usar se a busca for pela PK)
            //a vantagem do Find é que ele primeiro busca na memória e depois no banco de dados
            //diferente do FirstOrDefault que ja vai direto no BD
            //var produto = _context.Find(id);


            if (produto == null)
                return NotFound(); //404

            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return produto;
        }

    }
}
