using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Painel_Investimentos.Data.Entities
{
    public class PerfilRiscoEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ushort Id { get; set; }

        [Required]
        public required string NomePerfil { get; set; }

        [Required]
        public required string DescricaoPerfil { get; set; }
    }
}
