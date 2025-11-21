using API_Painel_Investimentos.Controllers;
using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Dto.Telemetria;
using API_Painel_Investimentos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API_Painel_Investimentos.Test.Controllers
{
    public class TelemetriaControllerTests
    {
        private readonly Mock<ITelemetriaService> _mockTelemetriaService = new Mock<ITelemetriaService>();
        private readonly TelemetriaController _controller;

        public TelemetriaControllerTests()
        {
            _controller = new TelemetriaController(_mockTelemetriaService.Object);
        }

        [Fact]
        public async Task GetTelemetria_QuandoSucesso_RetornaOkComDadosTelemetria()
        {
            var dadosTelemetria = new ResponseTelemetriaServicoDto
            {
                Servicos = new List<TelemetriaServicoDto>
            {
                new TelemetriaServicoDto { Nome = "Autenticacao", QuantidadeChamadas = 500, MediaTempoRespostaMs = 120 },
            },
                Periodo = new PeriodoDto
                {
                    Inicio = new DateOnly(2023, 11, 01),
                    Fim = new DateOnly(2023, 11, 20)
                }
            };

            var resultadoSucesso = ResultadoDto<ResponseTelemetriaServicoDto>.Ok(dadosTelemetria);

            _mockTelemetriaService
                .Setup(s => s.ObterDadosTelemetria())
                .ReturnsAsync(resultadoSucesso);

            var resultado = await _controller.GetTelemetria();

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);

            var dadoRetornado = Assert.IsType<ResponseTelemetriaServicoDto>(okResult.Value);

            Assert.Single(dadoRetornado.Servicos);
            Assert.Equal("Autenticacao", dadoRetornado.Servicos.First().Nome);
        }

        [Fact]
        public async Task GetTelemetria_QuandoFalha_RetornaNotFoundComErroDto()
        {
            var erro = new ErroDto
            {
                Codigo = "TelemetriaNaoDisponivel",
                Mensagem = "Não foi possível obter os dados de telemetria no momento."
            };

            var resultadoFalha = ResultadoDto<ResponseTelemetriaServicoDto>.Falha(erro);

            _mockTelemetriaService
                .Setup(s => s.ObterDadosTelemetria())
                .ReturnsAsync(resultadoFalha);

            var resultado = await _controller.GetTelemetria();

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(resultado);
            Assert.Equal(404, notFoundResult.StatusCode);

            var erroRetornado = Assert.IsType<ErroDto>(notFoundResult.Value);
            Assert.Equal("TelemetriaNaoDisponivel", erroRetornado.Codigo);
        }
    }


}