using API_Painel_Investimentos.Dto.Telemetria;
using API_Painel_Investimentos.Enums;
using API_Painel_Investimentos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace API_Painel_Investimentos.Filters
{
    public class TelemetriaActionFilter(ITelemetriaRepository telemetriaRepository) : IAsyncActionFilter
    {
        private readonly ITelemetriaRepository _telemetriaRepository = telemetriaRepository;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();

            await next();

            stopwatch.Stop();

            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var endpointName = descriptor?.ActionName ?? "Unknown";
            var codEndpoint = GetEndpointCode(endpointName);

            var telemetriaDTO = new TelemetriaDTO
            {
                DataRequisicao = DateOnly.FromDateTime(DateTime.Now),
                CodEndpoint = codEndpoint,
                TempoResposta = (ushort)stopwatch.ElapsedMilliseconds
            };

            await _telemetriaRepository.SalvarDadosTelemetria(telemetriaDTO);
        }

        private static ushort GetEndpointCode(string endpointName) => endpointName switch
        {
            nameof(EndpointTelemetriaEnum.PostSimulacaoInvestimento) => (ushort)EndpointTelemetriaEnum.PostSimulacaoInvestimento,
            nameof(EndpointTelemetriaEnum.GetSimulacoes) => (ushort)EndpointTelemetriaEnum.GetSimulacoes,
            nameof(EndpointTelemetriaEnum.GetSimulacoesProdutoDia) => (ushort)EndpointTelemetriaEnum.GetSimulacoesProdutoDia,
            nameof(EndpointTelemetriaEnum.GetPerfilRiscoCliente) => (ushort)EndpointTelemetriaEnum.GetPerfilRiscoCliente,
            nameof(EndpointTelemetriaEnum.GetProdutosRecomendadosPerfil) => (ushort)EndpointTelemetriaEnum.GetProdutosRecomendadosPerfil,
            nameof(EndpointTelemetriaEnum.GetHistoricoInvestimentosCliente) => (ushort)EndpointTelemetriaEnum.GetHistoricoInvestimentosCliente,
            nameof(EndpointTelemetriaEnum.PostTokenUsuario) => (ushort)EndpointTelemetriaEnum.PostTokenUsuario,
            nameof(EndpointTelemetriaEnum.PostUsuario) => (ushort)EndpointTelemetriaEnum.PostUsuario,
            _ => 0
        };
    }
}
