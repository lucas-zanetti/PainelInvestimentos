using Moq;
using Microsoft.AspNetCore.Mvc;
using API_Painel_Investimentos.Controllers;
using API_Painel_Investimentos.Interfaces;
using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Dto.SimulacaoInvestimento;
using API_Painel_Investimentos.Enums;

namespace API_Painel_Investimentos.Test.Controllers
{
    public class PainelInvestimentoControllerTests
    {
        private readonly Mock<IPainelInvestimentoService> _mockSimulacaoService;
        private readonly PainelInvestimentoController _controller;

        public PainelInvestimentoControllerTests()
        {
            _mockSimulacaoService = new Mock<IPainelInvestimentoService>();
            _controller = new PainelInvestimentoController(_mockSimulacaoService.Object);
        }

        [Fact]
        public async Task PostSimulacaoInvestimentoAsync_Sucesso_RetornaOkComDados()
        {
            var request = new RequestSimulacaoInvestimentoDto { ClienteId = 1, Valor = 1000, PrazoMeses = 12, TipoProduto = "CDB" };

            var produtoDto = new ProdutoDto { Id = 1, Nome = "CDB Max", Tipo = "CDB", Rentabilidade = 0.1, Risco = "Baixo", PrazoMinimoResgateMeses = 6 };
            var resultadoSimulacaoDto = new ResultadoSimulacaoDto { ValorFinal = 1200.50, RentabilidadeEfetiva = 0.205, PrazoMeses = 12 };
            var response = new ResponseSimulacaoInvestimentoDto { ProdutoValidado = produtoDto, ResultadoSimulacao = resultadoSimulacaoDto, DataSimulacao = DateTime.Now };

            var resultadoSucesso = ResultadoDto<ResponseSimulacaoInvestimentoDto>.Ok(response);

            _mockSimulacaoService
                .Setup(s => s.SimularInvestimentoAsync(request))
                .ReturnsAsync(resultadoSucesso);

            var resultado = await _controller.PostSimulacaoInvestimentoAsync(request);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var dadoRetornado = Assert.IsType<ResponseSimulacaoInvestimentoDto>(okResult.Value);
            Assert.Equal(1200.50, dadoRetornado.ResultadoSimulacao.ValorFinal);
            Assert.Equal("CDB Max", dadoRetornado.ProdutoValidado.Nome);
        }

        [Fact]
        public async Task PostSimulacaoInvestimentoAsync_ClienteInexistente_RetornaBadRequest()
        {
            var request = new RequestSimulacaoInvestimentoDto { ClienteId = 999, Valor = 1000, PrazoMeses = 12, TipoProduto = "LCI" };
            var erro = new ErroDto { Codigo = ErrorCodes.ClienteInexistente, Mensagem = "Cliente não encontrado." };

            var resultadoFalha = ResultadoDto<ResponseSimulacaoInvestimentoDto>.Falha(erro);

            _mockSimulacaoService
                .Setup(s => s.SimularInvestimentoAsync(request))
                .ReturnsAsync(resultadoFalha);

            var resultado = await _controller.PostSimulacaoInvestimentoAsync(request);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado);
            var erroRetornado = Assert.IsType<ErroDto>(badRequestResult.Value);
            Assert.Equal(ErrorCodes.ClienteInexistente, erroRetornado.Codigo);
        }

        [Fact]
        public async Task GetSimulacoesInvestimentoAsync_Sucesso_RetornaOkComLista()
        {
            var simulacoes = new List<SimulacaoInvestimentoDto>
            {
                new SimulacaoInvestimentoDto { Id = 1, ClienteId = 1, Produto = "CDB", ValorInvestido = 1000, ValorFinal = 1200, PrazoMeses = 12, DataSimulacao = DateTime.Now }
            };

            var resultadoSucesso = ResultadoDto<List<SimulacaoInvestimentoDto>>.Ok(simulacoes);

            _mockSimulacaoService
                .Setup(s => s.ObterSimulacoesInvestimentoAsync())
                .ReturnsAsync(resultadoSucesso);

            var resultado = await _controller.GetSimulacoesInvestimentoAsync();

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var listaRetornada = Assert.IsType<List<SimulacaoInvestimentoDto>>(okResult.Value);
            Assert.NotEmpty(listaRetornada);
            Assert.Equal("CDB", listaRetornada[0].Produto);
        }

        [Fact]
        public async Task GetSimulacoesInvestimentoAsync_SimulacaoInexistente_RetornaNotFound()
        {
            var erro = new ErroDto { Codigo = ErrorCodes.SimulacaoInexistente, Mensagem = "Nenhuma simulação encontrada." };

            var resultadoFalha = ResultadoDto<List<SimulacaoInvestimentoDto>>.Falha(erro);

            _mockSimulacaoService
                .Setup(s => s.ObterSimulacoesInvestimentoAsync())
                .ReturnsAsync(resultadoFalha);

            var resultado = await _controller.GetSimulacoesInvestimentoAsync();

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(resultado);
            var erroRetornado = Assert.IsType<ErroDto>(notFoundResult.Value);
            Assert.Equal(ErrorCodes.SimulacaoInexistente, erroRetornado.Codigo);
        }

        [Fact]
        public async Task GetSimulacoesInvestimentoProdutoDiaAsync_Sucesso_RetornaOkComLista()
        {
            var simulacoes = new List<SimulacaoInvestimentoProdutoDiaDto>
            {
                new SimulacaoInvestimentoProdutoDiaDto { Produto = "Tesouro Direto", Data = DateOnly.FromDateTime(DateTime.Today), QuantidadeSimulacoes = 5, MediaValorFinal = 1500 }
            };

            var resultadoSucesso = ResultadoDto<List<SimulacaoInvestimentoProdutoDiaDto>>.Ok(simulacoes);

            _mockSimulacaoService
                .Setup(s => s.ObterSimulacoesInvestimentoProdutoDiaAsync())
                .ReturnsAsync(resultadoSucesso);

            var resultado = await _controller.GetSimulacoesInvestimentoProdutoDiaAsync();

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var listaRetornada = Assert.IsType<List<SimulacaoInvestimentoProdutoDiaDto>>(okResult.Value);
            Assert.NotEmpty(listaRetornada);
            Assert.Equal("Tesouro Direto", listaRetornada[0].Produto);
        }

        [Fact]
        public async Task GetSimulacoesInvestimentoProdutoDiaAsync_SimulacaoInexistente_RetornaNotFound()
        {
            var erro = new ErroDto { Codigo = ErrorCodes.SimulacaoInexistente, Mensagem = "Nenhum resumo de simulação encontrado." };

            var resultadoFalha = ResultadoDto<List<SimulacaoInvestimentoProdutoDiaDto>>.Falha(erro);

            _mockSimulacaoService
                .Setup(s => s.ObterSimulacoesInvestimentoProdutoDiaAsync())
                .ReturnsAsync(resultadoFalha);

            var resultado = await _controller.GetSimulacoesInvestimentoProdutoDiaAsync();

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(resultado);
            var erroRetornado = Assert.IsType<ErroDto>(notFoundResult.Value);
            Assert.Equal(ErrorCodes.SimulacaoInexistente, erroRetornado.Codigo);
        }

        [Fact]
        public async Task GetPerfilRiscoClienteAsync_Sucesso_RetornaOkComPerfil()
        {
            const uint clienteId = 123;
            var perfil = new ResponsePerfilRiscoDto { ClienteId = clienteId, Perfil = "Moderado", Pontuacao = 75, Descricao = "Descricao de perfil moderado." };

            var resultadoSucesso = ResultadoDto<ResponsePerfilRiscoDto>.Ok(perfil);

            _mockSimulacaoService
                .Setup(s => s.ObterPerfilRiscoAsync(clienteId))
                .ReturnsAsync(resultadoSucesso);

            var resultado = await _controller.GetPerfilRiscoClienteAsync(clienteId);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var dadoRetornado = Assert.IsType<ResponsePerfilRiscoDto>(okResult.Value);
            Assert.Equal("Moderado", dadoRetornado.Perfil);
            Assert.Equal(75, dadoRetornado.Pontuacao);
        }

        [Fact]
        public async Task GetPerfilRiscoClienteAsync_PerfilRiscoInexistente_RetornaNotFound()
        {
            const uint clienteId = 999;
            var erro = new ErroDto { Codigo = ErrorCodes.PerfilRiscoInexistente, Mensagem = "Perfil de risco não encontrado para o cliente." };

            var resultadoFalha = ResultadoDto<ResponsePerfilRiscoDto>.Falha(erro);

            _mockSimulacaoService
                .Setup(s => s.ObterPerfilRiscoAsync(clienteId))
                .ReturnsAsync(resultadoFalha);

            var resultado = await _controller.GetPerfilRiscoClienteAsync(clienteId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(resultado);
            var erroRetornado = Assert.IsType<ErroDto>(notFoundResult.Value);
            Assert.Equal(ErrorCodes.PerfilRiscoInexistente, erroRetornado.Codigo);
        }

        [Fact]
        public async Task GetProdutosRecomendadosPerfilAsync_Sucesso_RetornaOkComLista()
        {
            const string perfil = "Agressivo";
            var produtos = new List<ProdutoDto>
            {
                new ProdutoDto { Id = 10, Nome = "Ação de Alto Risco", Tipo = "Ação", Rentabilidade = 0.5, Risco = "Alto", PrazoMinimoResgateMeses = 36 }
            };

            var resultadoSucesso = ResultadoDto<List<ProdutoDto>>.Ok(produtos);

            _mockSimulacaoService
                .Setup(s => s.ObterProdutosPerfilAsync(perfil))
                .ReturnsAsync(resultadoSucesso);

            var resultado = await _controller.GetProdutosRecomendadosPerfilAsync(perfil);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var listaRetornada = Assert.IsType<List<ProdutoDto>>(okResult.Value);
            Assert.NotEmpty(listaRetornada);
            Assert.Equal("Ação de Alto Risco", listaRetornada[0].Nome);
        }

        [Fact]
        public async Task GetProdutosRecomendadosPerfilAsync_ProdutoCompativelInexistente_RetornaNotFound()
        {
            const string perfil = "Inexistente";
            var erro = new ErroDto { Codigo = ErrorCodes.ProdutoCompativelInexistente, Mensagem = "Nenhum produto compatível encontrado para o perfil." };

            var resultadoFalha = ResultadoDto<List<ProdutoDto>>.Falha(erro);

            _mockSimulacaoService
                .Setup(s => s.ObterProdutosPerfilAsync(perfil))
                .ReturnsAsync(resultadoFalha);

            var resultado = await _controller.GetProdutosRecomendadosPerfilAsync(perfil);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(resultado);
            var erroRetornado = Assert.IsType<ErroDto>(notFoundResult.Value);
            Assert.Equal(ErrorCodes.ProdutoCompativelInexistente, erroRetornado.Codigo);
        }

        [Fact]
        public async Task GetHistoricoInvestimentosClienteAsync_Sucesso_RetornaOkComHistorico()
        {
            const uint clienteId = 123;
            var historico = new List<InvestimentoClienteDto>
            {
                new InvestimentoClienteDto { Id = 1, Tipo = "LCI", Valor = 5000, Rentabilidade = 0.1, Data = DateOnly.FromDateTime(DateTime.Today.AddYears(-1)) }
            };

            var resultadoSucesso = ResultadoDto<List<InvestimentoClienteDto>>.Ok(historico);

            _mockSimulacaoService
                .Setup(s => s.ObterHistoricoInvestimentosClienteAsync(clienteId))
                .ReturnsAsync(resultadoSucesso);

            var resultado = await _controller.GetHistoricoInvestimentosClienteAsync(clienteId);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var listaRetornada = Assert.IsType<List<InvestimentoClienteDto>>(okResult.Value);
            Assert.NotEmpty(listaRetornada);
            Assert.Equal("LCI", listaRetornada[0].Tipo);
        }

        [Fact]
        public async Task GetHistoricoInvestimentosClienteAsync_InvestimentoInexistente_RetornaNotFound()
        {
            const uint clienteId = 456;
            var erro = new ErroDto { Codigo = ErrorCodes.InvestimentoInexistente, Mensagem = "Nenhum investimento encontrado para o cliente." };

            var resultadoFalha = ResultadoDto<List<InvestimentoClienteDto>>.Falha(erro);

            _mockSimulacaoService
                .Setup(s => s.ObterHistoricoInvestimentosClienteAsync(clienteId))
                .ReturnsAsync(resultadoFalha);

            var resultado = await _controller.GetHistoricoInvestimentosClienteAsync(clienteId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(resultado);
            var erroRetornado = Assert.IsType<ErroDto>(notFoundResult.Value);
            Assert.Equal(ErrorCodes.InvestimentoInexistente, erroRetornado.Codigo);
        }
    }
}