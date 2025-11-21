using API_Painel_Investimentos.Data.Contexts;
using API_Painel_Investimentos.Data.Entities;
using API_Painel_Investimentos.Dto.Telemetria;
using API_Painel_Investimentos.Enums;
using API_Painel_Investimentos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Painel_Investimentos.Data.Repositories
{
    public class TelemetriaRepository(DbTelemetriaContext context) : ITelemetriaRepository
    {
        private readonly DbTelemetriaContext _context = context;

        public async Task<(List<TelemetriaServicoDto>, PeriodoDto)> ObterDadosTelemetria()
        {
            var dadosAgregados = await _context.DadosTelemetria
                .GroupBy(t => new { t.DataRequisicao, t.CodEndpoint })
                .Select(g => new
                {
                    CodigoEndpoint = g.Key.CodEndpoint,
                    TotalChamadas = g.Count(),
                    MediaResposta = g.Average(t => t.TempoResposta),
                    Data = g.Key.DataRequisicao
                })
                .ToListAsync();

            var dadosEndpointAgregados =  dadosAgregados.Select(dado => new TelemetriaServicoDto
                {
                    Nome = ((EndpointTelemetriaEnum)dado.CodigoEndpoint).ToString(),

                    QuantidadeChamadas = dado.TotalChamadas,

                    MediaTempoRespostaMs = (int)Math.Round(dado.MediaResposta)
                });

            var dataInicio = await _context.DadosTelemetria.MinAsync(t => t.DataRequisicao);
            var dataFim = await _context.DadosTelemetria.MaxAsync(t => t.DataRequisicao);

            var periodo = new PeriodoDto { Inicio = dataInicio, Fim = dataFim };

            return (dadosEndpointAgregados.ToList(), periodo);
        }

        public async Task SalvarDadosTelemetria(TelemetriaDTO telemetriaDTO)
        {
            await _context.DadosTelemetria.AddAsync(new TelemetriaEntity
            {
                DataRequisicao = telemetriaDTO.DataRequisicao,
                CodEndpoint = telemetriaDTO.CodEndpoint,
                TempoResposta = telemetriaDTO.TempoResposta
            });

            await _context.SaveChangesAsync();
        }
    }
}
