using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        //construtor
        //sempre que tiver uma colecao

        public Categoria()
        {
            Produtos = new Collection<Produto>(); 
        }

        [Key]
        public int CategoriaID { get; set; }
        
        [Required]
        [MaxLength(80)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(500)]
        public string ImagemUrl { get; set; }

        //relacionamento entre as tabelas 1 categoria muitos produtos
        public ICollection<Produto> Produtos { get; set; }

    }
}
