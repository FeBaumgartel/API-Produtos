using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ListaCompras.Models;
using Dapper;

namespace ListaCompras.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[Controller]")]
    public class ProdutosController : Controller
    {
        public IConfiguration Configuration { get; }

        public ProdutosController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<Produtos> GetAll()
        {
            List<Produtos> listaProdutos = new List<Produtos>();
            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    listaProdutos = conn.Query<Produtos>("SELECT * FROM produtos ORDER BY id ASC").ToList();
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e);
                    Console.ForegroundColor = ConsoleColor.White;
                    conn.Close();
                }
            }

            return listaProdutos;
        }

        [HttpGet("{id}")]
        public Produtos GetById(int id)
        {
            Produtos produto = new Produtos();
            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    produto = conn.Query<Produtos>("SELECT * FROM produtos WHERE id = " + id + " ORDER BY id ASC").FirstOrDefault();
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e);
                    Console.ForegroundColor = ConsoleColor.White;
                    conn.Close();
                }
            }

            return produto;
        }

               
        [HttpPost]
        public IActionResult Create([FromBody] Produtos produto)
        {
            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();

                try
                {
                    TiposProdutos tipo = new TiposProdutos();
                    tipo = conn.Query<TiposProdutos>("SELECT TOP 1 * FROM tipos_produtos WHERE descricao_produto = " + produto.descricao_produto + " ORDER BY id DESC").FirstOrDefault();
                    if (tipo.descricao_produto == null)
                    {
                        string insertTipo = "INSERT INTO tipos_produtos (descricao_produto) VALUES (" + produto.descricao_produto + ")";

                        using (SqlCommand query = conn.CreateCommand())
                        {
                            query.CommandText = insertTipo;
                            query.ExecuteNonQuery();
                        }
                    }

                    tipo = conn.Query<TiposProdutos>("SELECT TOP 1 * FROM tipos_produtos WHERE descricao_produto = "+ produto.descricao_produto + " ORDER BY id DESC").FirstOrDefault();

                    string insertProduto = "INSERT INTO produtos (nome_produto, preco, quantidade, id_tipos_produtos) VALUES (" + produto.nome_produto + "," + produto.preco + "," + produto.quantidade + "," + tipo.id + ")";


                    using (SqlCommand query = conn.CreateCommand())
                    {
                        query.CommandText = insertProduto;
                        query.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e);
                    Console.ForegroundColor = ConsoleColor.White;

                    return BadRequest(new { message = "Ocorreu algum erro durante a criação de um produto." });
                }
            }

            return Ok(new { message = "Produto cadastrado com sucesso" });
        }

        [HttpPut]
        public IActionResult Update([FromBody] Produtos produto)
        {
            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();

                try
                {
                    TiposProdutos tipo = new TiposProdutos();
                    tipo = conn.Query<TiposProdutos>("SELECT * FROM tipos_produtos WHERE descricao_produto = " + produto.descricao_produto).FirstOrDefault();
                    if (tipo.descricao_produto == null)
                    {
                        string insertTipo = "INSERT INTO tipos_produtos (descricao_produto) VALUES (" + produto.descricao_produto + ")";

                        using (SqlCommand query = conn.CreateCommand())
                        {
                            query.CommandText = insertTipo;
                            query.ExecuteNonQuery();
                        }
                    }

                    tipo = conn.Query<TiposProdutos>("SELECT TOP 1 * FROM tipos_produtos WHERE descricao_produto = " + produto.descricao_produto + " ORDER BY id DESC").FirstOrDefault();

                    string insertProduto = "UPDATE produtos SET nome_produto = " + produto.nome_produto + ", preco = " + produto.preco + ", quantidade = " + produto.quantidade + ", id_tipos_produtos = " + tipo.id + " WHERE id = " + produto.id;

                    using (SqlCommand query = conn.CreateCommand())
                    {
                        query.CommandText = insertProduto;
                        query.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e);
                    Console.ForegroundColor = ConsoleColor.White;

                    return BadRequest(new { message = "Ocorreu algum erro durante a atualização de um produto." });
                }
            }

            return Ok(new { message = "Produto atualiza com sucesso" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();

                try
                {
                    string insertProduto = "DELETE produtos WHERE id = " + id;


                    using (SqlCommand query = conn.CreateCommand())
                    {
                        query.CommandText = insertProduto;
                        query.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e);
                    Console.ForegroundColor = ConsoleColor.White;

                    return BadRequest(new { message = "Ocorreu algum erro durante a exclusão de um produto." });
                }
            }

            return Ok(new { message = "Produto deletado com sucesso" });
        }
    }
}
