using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Context
{
    //classe de contexto que se comunica com o BD
    public class AppDbContext : DbContext
    {
        //define contexto para o BD
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        //mapeamento das entidades        
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
    }
}
