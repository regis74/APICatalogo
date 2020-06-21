﻿using APICatalogo.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Models
{
    [Table("Produtos")]
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(20,ErrorMessage ="O nome deve ter entre 5 e 20 caracteres",MinimumLength=5)]
        [PrimeiraLetraMaiuscula]
        public string Nome { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        public string Descricao { get; set; }
        
        [Required]
        [Range(1, 10000, ErrorMessage ="O preço deve estar entre {1} e {2}")]
        public decimal Preco { get; set; }

        [Required]
        [StringLength(300,MinimumLength = 10)]
        public string ImagemUrl { get; set; }
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }

        //1 produto esta relacionado com categoria
        public Categoria Categoria { get; set; }
        public int CategoriaId { get; set; }

    }
}
