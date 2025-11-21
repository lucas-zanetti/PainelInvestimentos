using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Painel_Investimentos.Data.Entities
{
    public class ClienteEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public uint Id { get; set; }

        [Required]
        public ushort Pontuacao { get; set; }

        public required List<InvestimentoEntity> Investimentos { get; set; }
    }
}
