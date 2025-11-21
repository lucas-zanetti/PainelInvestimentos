using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Painel_Investimentos.Data.Entities
{
    public class InvestimentoEntity
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
        public required string Tipo { get; set; }

        [Required]
        public double Valor { get; set; }

        [Required]
        public double Rentabilidade { get; set; }

        [Required]
        public DateOnly Data { get; set; }
    }
}
