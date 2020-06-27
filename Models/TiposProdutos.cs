using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ListaCompras.Models
{
    public class TiposProdutos
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string descricao_produto { get; set; }

        public TiposProdutos()
        {
        }

        public TiposProdutos(string descricao_produto)
        {
            this.descricao_produto = descricao_produto;
        }
    }
}
