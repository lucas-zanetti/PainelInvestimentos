using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.Telemetria
{
    [SwaggerSchema(Description = "DTO que representa a resposta contendo a lista de métricas de telemetria dos serviços.")]
    public record ResponseTelemetriaServicoDto
    {
        [SwaggerParameter(Description = "Lista de métricas por serviço (nome, quantidade de chamadas e tempo médio de resposta).")]
        public required List<TelemetriaServicoDto> Servicos { get; set; }

        [SwaggerParameter(Description = "Período dos dados de telemetria.")]
        public required PeriodoDto Periodo { get; set; }
    }
}
