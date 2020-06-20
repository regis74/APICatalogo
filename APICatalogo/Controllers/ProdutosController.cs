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

        [HttpGet("{id}")]
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
    }
}
