using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Validation
{
    public class PrimeiraLetraMaiusculaAttribute : ValidationAttribute
    {
        //Exemplo de como fazer validação manual (substituicao dos [Required], etc) em Produto.cs
        //sobrescrever o metodo IsValid
        protected override ValidationResult IsValid(object value, //value será o valor da propriedade
            ValidationContext validationContext) //informacao do contexto onde estamos fazendo a validacao (no caso classe Produto)
        {
            //colocar esse tratamento para dar um bypass no Required
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return ValidationResult.Success;

            var primeiraLetra = value.ToString()[0].ToString(); //pega a primeira posição da string e guarda
            if (primeiraLetra != primeiraLetra.ToUpper())
                return new ValidationResult("A primeira letra do nome do produto deve ser maiúscula");

            return ValidationResult.Success;

        }

    }
}
