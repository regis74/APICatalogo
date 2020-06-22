using APICatalogo.Context;
using APICatalogo.Filter;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [Route("api/[Controller]")]
   [ApiController] //esse atributo é novo na versao do .netcore - nao exite que faça o modelState
    public class ProdutosController : ControllerBase
    {

        //injecao de dependencia da classe DbContext
        //isso é possivel pq na classe Startup.cs foi injetada a 'services.AddDbContext'
        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext contexto)
        {
            _context = contexto;
        }

        /// <summary>
        /// SINCRONO
        /// </summary>
        //IEnumerable de Produto pq retornar uma Lista de Produtos
        //ActionResult retornar os codigos http 200, 400, etc
        //[HttpGet]
        //public ActionResult<IEnumerable<Produto>> Get()
        //{
        //    //return _context.Produtos.ToList();

        //    //para melhorar o desempenho e nao verificar o estado, utiliza-se o AsNoTracking
        //    //Só funciona para metodos Get
        //    return _context.Produtos.AsNoTracking().ToList();
        //}

        /// <summary>
        /// ASINCRONO - a justificativa é acesso a banco de dados, ou uso de I/O
        /// </summary>
        //IEnumerable de Produto pq retornar uma Lista de Produtos
        //ActionResult retornar os codigos http 200, 400, etc
        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }

        //utilizando a interface IActionResult
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    var produto = _context.Produtos.FirstOrDefault();
        //    if (produto == null)
        //        return NotFound();

        //    return Ok(produto);
        //}


        //utilizando a ActionResult<T> 
        //só foi disponibilizada a partir da versao 2.1 do netcore
        //[HttpGet]
        //public ActionResult<Produto> Get()
        //{
        //    var produto = _context.Produtos.FirstOrDefault();
        //    if (produto == null)
        //        return NotFound();

        //    return produto;
        //}

        //colocando ":int:min(1)}" define uma restricao de valores, nao vai mais aceitar 0 por exemplo
        //com isso ele já retona 404 sem entrar no método, evita processamento desnecessario como uma ida ao BD realizar consulta
        //o proprio HTTP já bloqueia
        //[HttpGet("{id:int:min(1)}", Name = "ObterProduto")] //vincula uma rota nomeada (Name) ao Http retorno do post
        //public ActionResult<Produto> Get(int id)
        //{
        //    //var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

        //    //para melhorar o desempenho e nao verificar o estado, utiliza-se o AsNoTracking
        //    //Só funciona para metodos Get
        //    var produto = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);

        //    if (produto == null)
        //        return NotFound(); //404

        //    return produto;
        //}

        //colocando ":int:min(1)}" define uma restricao de valores, nao vai mais aceitar 0 por exemplo
        //com isso ele já retona 404 sem entrar no método, evita processamento desnecessario como uma ida ao BD realizar consulta
        //o proprio HTTP já bloqueia
        //[HttpGet("{id:int:min(1)}", Name = "ObterProduto")] //vincula uma rota nomeada (Name) ao Http retorno do post
        //public async Task<ActionResult<Produto>> GetAsync(int id, [BindRequired] string nome) //bindRequired obriga informar via QueryString
        //{
        //    var nomeProduto = nome;
            
        //    //var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

        //    //para melhorar o desempenho e nao verificar o estado, utiliza-se o AsNoTracking
        //    //Só funciona para metodos Get
        //    var produto = _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);

        //    if (produto == null)
        //        return NotFound(); //404

        //    //foreach (var item in produto.Result)
        //    //{
        //    //    item.Nome = item.Nome + nomeProduto;
        //    //}

        //    produto.Result.Nome = produto.Result.Nome + " " + nomeProduto;

        //    return await produto;
        //}

        //colocando ":int:min(1)}" define uma restricao de valores, nao vai mais aceitar 0 por exemplo
        //com isso ele já retona 404 sem entrar no método, evita processamento desnecessario como uma ida ao BD realizar consulta
        //o proprio HTTP já bloqueia
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")] //vincula uma rota nomeada (Name) ao Http retorno do post
        public async Task<ActionResult<Produto>> GetAsync([FromQuery]int id) //FromQuery altera o comportamento padrao... obtem da QueryString e nao mais da rota
        {
            //throw new Exception("Exception ao retornar produto pelo id");
            //string[] teste = null;
            //if (teste.Length > 0)
            //{

            //}


            var produto = _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);

            if (produto == null)
                return NotFound(); //404

            return await produto;
        }

        //alpha é a restricao para receber somente valores alfanumericos
        //sempre que entrar com letras ira cair nesse endpoint
        //[HttpGet("{valor:alpha:length(5)}")]
        //public ActionResult<IEnumerable<Produto>> GetAlpha5(string valor)
        //{
        //    var ret = valor;
        //    return _context.Produtos.AsNoTracking().ToList();
        //}

        //alpha com tamanho 5 é a restricao para receber somente valores alfanumericos
        //sempre que entrar com letras ira cair nesse endpoint
        //[HttpGet("{valor:alpha(6)}")]
        //public ActionResult<IEnumerable<Produto>> GetAlpha()
        //{
        //    return _context.Produtos.AsNoTracking().ToList();
        //}

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
