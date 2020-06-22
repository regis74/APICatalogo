using APICatalogo.Models;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace APICatalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        //sem implementar nada, poderia deixar sem nada essa Interface pq utilizaria o <T> de Produto em Repository que ja contem a implementacao
        //portanto os metodos abaixo sao extras (nao precisaria implementar)

        //como esse metodo é especifico para produto, fica na interface de produto
        IEnumerable<Produto> GetProdutosPorPreco();
    }
}
