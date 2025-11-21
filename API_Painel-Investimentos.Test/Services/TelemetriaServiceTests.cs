using Moq;
using API_Painel_Investimentos.Interfaces;
using API_Painel_Investimentos.Services;
using API_Painel_Investimentos.Dto.Telemetria;
using API_Painel_Investimentos.Enums;

namespace API_Painel_Investimentos.Test.Services
{
    public class TelemetriaServiceTests
    {
        private readonly Mock<ITelemetriaRepository> _telemetriaRepositoryMock;
        private readonly TelemetriaService _service;

        public TelemetriaServiceTests()
        {
            _telemetriaRepositoryMock = new Mock<ITelemetriaRepository>();
            _service = new TelemetriaService(_telemetriaRepositoryMock.Object);
        }

        [Fact]
        public async Task ObterDadosTelemetria_DeveRetornarSucesso_QuandoDadosExistem()
        {
            var inicio = new DateOnly(2024, 10, 1);
            var fim = new DateOnly(2024, 10, 31);
            var periodoMock = new PeriodoDto { Inicio = inicio, Fim = fim };
            var dadosTelemetriaMock = new List<TelemetriaServicoDto>
            {
                new() { Nome = "ServicoFinanceiro", QuantidadeChamadas = 500, MediaTempoRespostaMs = 80 },
                new() { Nome = "ServicoAutenticacao", QuantidadeChamadas = 2500, MediaTempoRespostaMs = 15 }
            };

            _telemetriaRepositoryMock.Setup(r => r.ObterDadosTelemetria())
                                     .ReturnsAsync((dadosTelemetriaMock, periodoMock));

            var resultado = await _service.ObterDadosTelemetria();

            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Dado);
            Assert.Equal(2, resultado.Dado!.Servicos.Count);
            Assert.Equal(inicio, resultado.Dado.Periodo.Inicio);
            Assert.Equal(fim, resultado.Dado.Periodo.Fim);
            _telemetriaRepositoryMock.Verify(r => r.ObterDadosTelemetria(), Times.Once);
        }

        [Fact]
        public async Task ObterDadosTelemetria_DeveRetornarFalha_QuandoNenhumDadoEncontrado()
        {
            var periodoMock = new PeriodoDto { Inicio = DateOnly.MinValue, Fim = DateOnly.MinValue };
            var dadosTelemetriaVazio = new List<TelemetriaServicoDto>();

            _telemetriaRepositoryMock.Setup(r => r.ObterDadosTelemetria())
                                     .ReturnsAsync((dadosTelemetriaVazio, periodoMock));

            var resultado = await _service.ObterDadosTelemetria();

            Assert.False(resultado.Sucesso);
            Assert.Null(resultado.Dado);
            Assert.NotNull(resultado.Erro);
            Assert.Equal(ErrorCodes.TelemetriaSemDados, resultado.Erro!.Codigo);
            Assert.Equal("Nenhum dado de telemetria foi encontrado.", resultado.Erro.Mensagem);
            _telemetriaRepositoryMock.Verify(r => r.ObterDadosTelemetria(), Times.Once);
        }

        [Fact]
        public async Task ObterDadosTelemetria_DeveRetornarSucesso_QuandoListaNaoVaziaEPeriodoVazio()
        {
            var periodoVazio = new PeriodoDto { Inicio = DateOnly.MinValue, Fim = DateOnly.MinValue };
            var dadosTelemetriaMock = new List<TelemetriaServicoDto>
            {
                new() { Nome = "ServicoTeste", QuantidadeChamadas = 5, MediaTempoRespostaMs = 50 }
            };

            _telemetriaRepositoryMock.Setup(r => r.ObterDadosTelemetria())
                                     .ReturnsAsync((dadosTelemetriaMock, periodoVazio));

            var resultado = await _service.ObterDadosTelemetria();

            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Dado);
            Assert.Single(resultado.Dado!.Servicos);
            Assert.Equal(DateOnly.MinValue, resultado.Dado.Periodo.Inicio);
            Assert.Equal(DateOnly.MinValue, resultado.Dado.Periodo.Fim);
            _telemetriaRepositoryMock.Verify(r => r.ObterDadosTelemetria(), Times.Once);
        }
    }
}