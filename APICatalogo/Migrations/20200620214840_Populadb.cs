using Microsoft.EntityFrameworkCore.Migrations;

namespace APICatalogo.Migrations
{
    public partial class Populadb : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("insert into categorias(Nome, ImagemUrl) values ('Bebidas','http//www.marcoratti.net/imagens/1.jpg')");
            mb.Sql("insert into categorias(Nome, ImagemUrl) values ('Lanches','http//www.marcoratti.net/imagens/2.jpg')");
            mb.Sql("insert into categorias(Nome, ImagemUrl) values ('Sobremesas','http//www.marcoratti.net/imagens/3.jpg')");

            mb.Sql("insert into Produtos(nome,descricao,preco,imagemurl,estoque,datacadastro,categoriaid)" +
                " values('Coca-cola','Refrigerante de cola 350 ml', 5.45, 'http//www.marcoratti.net/imagens/coca.jpg', " +
                " 50, now()," + "(select categoriaid from categorias where nome='Bebidas'))");
            mb.Sql("insert into Produtos(nome,descricao,preco,imagemurl,estoque,datacadastro,categoriaid)" +
                " values('Lanche atum','Lanche de atum com maionese', 8.5, 'http//www.marcoratti.net/imagens/atum.jpg', " +
                " 10, now()," + "(select categoriaid from categorias where nome='Lanches'))");
            mb.Sql("insert into Produtos(nome,descricao,preco,imagemurl,estoque,datacadastro,categoriaid)" +
                " values('Pudim','Pudim de leite condensado 100g', 6.75, 'http//www.marcoratti.net/imagens/pudim.jpg', " +
                " 20, now()," + "(select categoriaid from categorias where nome='Sobremesas'))");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("delete from categorias");
            mb.Sql("delete from produtos");
        }
    }
}
