namespace API_Painel_Investimentos.Dto.Telemetria
{
    public class TelemetriaDTO
    {
        public DateOnly DataRequisicao { get; set; }
        public ushort CodEndpoint { get; set; }
        public ushort TempoResposta { get; set; }
    }
}
