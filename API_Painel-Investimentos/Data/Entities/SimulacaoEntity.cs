using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Painel_Investimentos.Data.Entities
{
    public class SimulacaoEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        [Required]
        public uint ClienteId { get; set; }

        [Required]
        [ForeignKey(nameof(ClienteId))]
        public required ClienteEntity Cliente { get; set; }

        [Required]
        public uint ProdutoId { get; set; }

        [Required]
        [ForeignKey(nameof(ProdutoId))]
        public required ProdutoEntity Produto { get; set; }

        [Required]
        public double ValorInvestido { get; set; }

        [Required]
        public double ValorFinal { get; set; }

        [Required]
        public ushort PrazoMeses { get; set; }

        [Required]
        public DateTime DataSimulacao { get; set; }
    }
}
