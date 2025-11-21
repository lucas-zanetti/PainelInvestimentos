using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.Telemetria
{
    [SwaggerSchema(Description = "DTO que representa métricas de telemetria de uma requisição, incluindo data da requisição, código do endpoint e tempo de resposta da requisição.")]

    public class TelemetriaDTO
    {
        [SwaggerParameter(Description = "Data da requisição.")]
        public DateOnly DataRequisicao { get; set; }

        [SwaggerParameter(Description = "Código do endpoint.")]
        public ushort CodEndpoint { get; set; }

        [SwaggerParameter(Description = "Tempo de resposta em milisegundos.")]
        public ushort TempoResposta { get; set; }
    }
}
