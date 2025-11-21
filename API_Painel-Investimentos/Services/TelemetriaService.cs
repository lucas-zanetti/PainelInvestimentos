using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Dto.Telemetria;
using API_Painel_Investimentos.Enums;
using API_Painel_Investimentos.Interfaces;

namespace API_Painel_Investimentos.Services
{
    public class TelemetriaService(ITelemetriaRepository telemetriaRepository) : ITelemetriaService
    {
        private readonly ITelemetriaRepository _telemetriaRepository = telemetriaRepository;
        public async Task<ResultadoDto<ResponseTelemetriaServicoDto>> ObterDadosTelemetria()
        {
            var (listaDadosTelemetriaEndpointPorDia, Periodo) = await _telemetriaRepository.ObterDadosTelemetria();

            if(listaDadosTelemetriaEndpointPorDia.Count == 0)
                return ResultadoDto<ResponseTelemetriaServicoDto>.Falha(new ErroDto
                {
                    Codigo = ErrorCodes.TelemetriaSemDados,
                    Mensagem = "Nenhum dado de telemetria foi encontrado."
                });

            return ResultadoDto<ResponseTelemetriaServicoDto>.Ok(new ResponseTelemetriaServicoDto
            {
                Servicos = listaDadosTelemetriaEndpointPorDia,
                Periodo = Periodo
            });
        }
    }
}
