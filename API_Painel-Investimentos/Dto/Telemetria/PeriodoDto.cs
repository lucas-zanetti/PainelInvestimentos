using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.Telemetria
{
    [SwaggerSchema(Description = "DTO que representa o período contido nos dados de telemetria.")]
    public record PeriodoDto
    {
        [SwaggerParameter(Description = "Data início dos dados de telemetria.")]
        public DateOnly Inicio { get; set; }

        [SwaggerParameter(Description = "Data fim dos dados de telemetria.")]
        public DateOnly Fim { get; set; }
    }
}
