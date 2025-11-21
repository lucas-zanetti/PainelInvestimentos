using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Dto.Telemetria;

namespace API_Painel_Investimentos.Interfaces
{
    public interface ITelemetriaService
    {
        Task<ResultadoDto<ResponseTelemetriaServicoDto>> ObterDadosTelemetria();
    }
}
