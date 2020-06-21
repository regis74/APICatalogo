using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Services
{

    //depois de implementar o servico, tem que registrar o servico na classe Startup.cs nome metodo ConfigureServices
    public class MeuServico : IMeuServico
    {
        public string Saudacao(string nome)
        {
            return $"Bem-vindo, {nome} \n\n{DateTime.Now}";
        }
    }
}
