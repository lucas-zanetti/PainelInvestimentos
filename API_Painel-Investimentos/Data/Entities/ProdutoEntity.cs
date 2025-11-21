using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Painel_Investimentos.Data.Entities
{
    public class ProdutoEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }

        [Required]
        public ushort PerfilRiscoId { get; set; }

        [Required]
        [ForeignKey(nameof(PerfilRiscoId))]
        public required PerfilRiscoEntity PerfilRisco { get; set; }

        [Required]
        public required string Nome { get; set; }

        [Required]
        public required string Tipo { get; set; }

        [Required]
        public double Rentabilidade { get; set; }

        [Required]
        public required string Risco { get; set; }

        [Required]
        public ushort PrazoMinimoResgateMeses { get; set; }
    }
}
