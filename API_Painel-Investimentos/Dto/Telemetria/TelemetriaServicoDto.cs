using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.Telemetria
{
    [SwaggerSchema(Description = "DTO que representa métricas de telemetria de um serviço, incluindo nome, quantidade de chamadas e tempo médio de resposta.")]
    public record TelemetriaServicoDto
    {
        [SwaggerParameter(Description = "Nome do serviço monitorado.")]
        public required string Nome { get; set; }

        [SwaggerParameter(Description = "Número total de chamadas recebidas pelo serviço no período.")]
        public int QuantidadeChamadas { get; set; }

        [SwaggerParameter(Description = "Tempo médio de resposta em milissegundos.")]
        public int MediaTempoRespostaMs { get; set; }
    }
}
