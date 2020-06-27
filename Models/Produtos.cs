using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ListaCompras.Models
{
    public class Produtos
    {
        [Key]
        public int id { get; set; }
        public string nome_produto { get; set; }
        public float preco { get; set; }
        public int quantidade { get; set; }
        public int id_tipos_produtos { get; set; }
        [NotMapped]
        public string descricao_produto { get; set; }


        public Produtos()
        {
        }

        public Produtos(string nome_produto, float preco, int quantidade, int id_tipos_produtos)
        {
            this.nome_produto = nome_produto;
            this.preco = preco;
            this.quantidade = quantidade;
            this.id_tipos_produtos = id_tipos_produtos;
        }
    }
}
