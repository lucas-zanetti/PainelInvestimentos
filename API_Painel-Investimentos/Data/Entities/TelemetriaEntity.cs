using System.ComponentModel.DataAnnotations;

namespace API_Painel_Investimentos.Data.Entities
{
    public class TelemetriaEntity
    {
        [Key]
        public uint Id { get; set; }
        public DateOnly DataRequisicao { get; set; }
        public ushort CodEndpoint { get; set; }
        public ushort TempoResposta { get; set; }
    }
}
