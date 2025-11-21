using API_Painel_Investimentos.Dto.Telemetria;

namespace API_Painel_Investimentos.Interfaces
{
    public interface ITelemetriaRepository
    {
        Task<(List<TelemetriaServicoDto>, PeriodoDto)> ObterDadosTelemetria();
        Task SalvarDadosTelemetria(TelemetriaDTO telemetriaDTO);
    }
}
