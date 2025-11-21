using Moq;
using API_Painel_Investimentos.Interfaces;
using API_Painel_Investimentos.Services;
using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Dto.SimulacaoInvestimento;
using API_Painel_Investimentos.Enums;

namespace API_Painel_Investimentos.Test.Services
{
    public class PainelInvestimentoServiceTests
    {
        private readonly Mock<IPainelInvestimentoRepository> _repositoryMock;
        private readonly PainelInvestimentoService _service;
        private readonly ErroDto _erroGenerico = new() { Codigo = "E001", Mensagem = "Erro de Repositório" };

        public PainelInvestimentoServiceTests()
        {
            _repositoryMock = new Mock<IPainelInvestimentoRepository>();
            _service = new PainelInvestimentoService(_repositoryMock.Object);
        }

        [Fact]
        public async Task ObterSimulacoesInvestimentoAsync_DeveRetornarSucesso_QuandoRepositorioRetornaDados()
        {
            var dados = new List<SimulacaoInvestimentoDto> { 
                new() { 
                    Id = 1, 
                    ClienteId = 1, 
                    Produto = "teste", 
                    DataSimulacao = DateTime.UtcNow,
                    ValorFinal = 1100.0,
                    ValorInvestido = 1000.0,
                    PrazoMeses = 12
                } };
            var resultadoEsperado = ResultadoDto<List<SimulacaoInvestimentoDto>>.Ok(dados);
            _repositoryMock.Setup(r => r.ObterTodasSimulacoesInvestimentoAsync())
                           .ReturnsAsync(resultadoEsperado);

            var resultado = await _service.ObterSimulacoesInvestimentoAsync();

            Assert.True(resultado.Sucesso);
            Assert.Equal(dados.Count, resultado.Dado!.Count);
        }

        [Fact]
        public async Task ObterSimulacoesInvestimentoAsync_DeveRetornarFalha_QuandoRepositorioFalha()
        {
            var resultadoEsperado = ResultadoDto<List<SimulacaoInvestimentoDto>>.Falha(_erroGenerico);
            _repositoryMock.Setup(r => r.ObterTodasSimulacoesInvestimentoAsync())
                           .ReturnsAsync(resultadoEsperado);

            var resultado = await _service.ObterSimulacoesInvestimentoAsync();

            Assert.False(resultado.Sucesso);
            Assert.Equal(_erroGenerico.Codigo, resultado.Erro!.Codigo);
        }

        [Theory]
        [InlineData(10, "Conservador")]
        [InlineData(40, "Moderado")]
        [InlineData(70, "Agressivo")]
        public async Task ObterPerfilRiscoAsync_DeveRetornarPerfilCorreto_ParaPontuacaoDoCliente(ushort pontuacao, string perfilEsperado)
        {
            uint clienteId = 123;
            var cliente = new ClienteDto { Id = clienteId, Pontuacao = pontuacao };
            var detalhesPerfil = new PerfilRiscoDto { NomePerfil = "Teste", DescricaoPerfil = $"Descrição de perfil {perfilEsperado}" };

            _repositoryMock.Setup(r => r.ObterClienteAsync(clienteId))
                           .ReturnsAsync(ResultadoDto<ClienteDto>.Ok(cliente));
            _repositoryMock.Setup(r => r.ObterDetalhesPerfilRiscoAsync(perfilEsperado))
                           .ReturnsAsync(ResultadoDto<PerfilRiscoDto>.Ok(detalhesPerfil));

            var resultado = await _service.ObterPerfilRiscoAsync(clienteId);

            Assert.True(resultado.Sucesso);
            Assert.Equal(perfilEsperado, resultado.Dado!.Perfil);
            Assert.Equal(pontuacao, resultado.Dado.Pontuacao);
            Assert.Equal(detalhesPerfil.DescricaoPerfil, resultado.Dado.Descricao);
        }

        [Fact]
        public async Task ObterPerfilRiscoAsync_DeveRetornarFalha_QuandoClienteNaoEncontrado()
        {
            uint clienteId = 999;
            _repositoryMock.Setup(r => r.ObterClienteAsync(clienteId))
                           .ReturnsAsync(ResultadoDto<ClienteDto>.Falha(_erroGenerico));

            var resultado = await _service.ObterPerfilRiscoAsync(clienteId);

            Assert.False(resultado.Sucesso);
            Assert.Equal(_erroGenerico.Codigo, resultado.Erro!.Codigo);
            _repositoryMock.Verify(r => r.ObterDetalhesPerfilRiscoAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ObterProdutosPerfilAsync_DeveRetornarProdutos_QuandoPerfilEValido()
        {
            string perfil = EnumPerfilRisco.Moderado.ToString();
            string riscoEsperado = EnumRiscoInvestimento.Medio.ToString();
            var detalhesPerfil = new PerfilRiscoDto { NomePerfil = "Moderado", DescricaoPerfil = "Descrição Moderado" };
            var produtos = new List<ProdutoDto> { 
                new() { 
                    Id = 1, 
                    Risco = riscoEsperado,
                    Nome = "Produto Moderado",
                    Tipo = "Fundo",
                    Rentabilidade = 0.12,
                    PrazoMinimoResgateMeses = 12
                }};

            _repositoryMock.Setup(r => r.ObterDetalhesPerfilRiscoAsync(perfil))
                           .ReturnsAsync(ResultadoDto<PerfilRiscoDto>.Ok(detalhesPerfil));
            _repositoryMock.Setup(r => r.ObterProdutosPorRiscoAsync(riscoEsperado))
                           .ReturnsAsync(ResultadoDto<List<ProdutoDto>>.Ok(produtos));

            var resultado = await _service.ObterProdutosPerfilAsync(perfil);

            Assert.True(resultado.Sucesso);
            Assert.Single(resultado.Dado!);
            _repositoryMock.Verify(r => r.ObterProdutosPorRiscoAsync(riscoEsperado), Times.Once);
        }

        [Fact]
        public async Task ObterProdutosPerfilAsync_DeveRetornarFalha_QuandoPerfilNaoExiste()
        {
            string perfilInvalido = "Invalido";
            var erroInvalido = new ErroDto { Codigo = ErrorCodes.PerfilRiscoInexistente, Mensagem = "Perfil de risco inválido." };

            _repositoryMock.Setup(r => r.ObterDetalhesPerfilRiscoAsync(perfilInvalido))
                           .ReturnsAsync(ResultadoDto<PerfilRiscoDto>.Falha(_erroGenerico));

            var resultado = await _service.ObterProdutosPerfilAsync(perfilInvalido);

            _repositoryMock.Setup(r => r.ObterDetalhesPerfilRiscoAsync(perfilInvalido))
                           .ReturnsAsync(ResultadoDto<PerfilRiscoDto>.Ok(new PerfilRiscoDto { NomePerfil = "Moderado", DescricaoPerfil = "Descrição Moderado" }));

            var resultadoInvalido = await _service.ObterProdutosPerfilAsync(perfilInvalido);

            Assert.False(resultadoInvalido.Sucesso);
            Assert.Equal(ErrorCodes.PerfilRiscoInexistente, resultadoInvalido.Erro!.Codigo);
            _repositoryMock.Verify(r => r.ObterProdutosPorRiscoAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task SimularInvestimentoAsync_DeveCriarNovoClienteESimular_QuandoClienteNaoExiste()
        {
            uint novoClienteId = 500;
            var entrada = new RequestSimulacaoInvestimentoDto
            {
                ClienteId = novoClienteId,
                Valor = 1000.0,
                PrazoMeses = 12,
                TipoProduto = "CDB"
            };

            var produtoIdeal = new ProdutoDto
            {
                Id = 1,
                Rentabilidade = 0.10,
                Risco = EnumRiscoInvestimento.Medio.ToString(),
                PrazoMinimoResgateMeses = 6,
                Nome = "CDB Médio",
                Tipo = "CDB"
            };
            var produtosDisponiveis = new List<ProdutoDto> { produtoIdeal };

            _repositoryMock.Setup(r => r.ObterClienteAsync(novoClienteId))
                           .ReturnsAsync(ResultadoDto<ClienteDto>.Falha(new ErroDto()));

            _repositoryMock.Setup(r => r.ObterProdutosPorTipoAsync(entrada.TipoProduto))
                           .ReturnsAsync(ResultadoDto<List<ProdutoDto>>.Ok(produtosDisponiveis));

            var resultado = await _service.SimularInvestimentoAsync(entrada);

            Assert.True(resultado.Sucesso);

            _repositoryMock.Verify(r => r.CriarCliente(It.Is<ClienteDto>(c => c.Id == novoClienteId && c.Pontuacao == 25)), Times.Once);

            _repositoryMock.Verify(r => r.GravarSimulacaoInvestimento(
                novoClienteId,
                produtoIdeal.Id,
                entrada.Valor,
                It.IsAny<ResponseSimulacaoInvestimentoDto>()), Times.Once);

            Assert.InRange(resultado.Dado!.ResultadoSimulacao.ValorFinal, 1099.0, 1101.0);
        }

        [Fact]
        public async Task SimularInvestimentoAsync_DeveUsarClienteExistenteESimular_QuandoClienteExiste()
        {
            uint clienteExistenteId = 100;
            ushort pontuacaoCliente = 70;
            var entrada = new RequestSimulacaoInvestimentoDto
            {
                ClienteId = clienteExistenteId,
                Valor = 5000.0,
                PrazoMeses = 24,
                TipoProduto = "Ações"
            };

            var cliente = new ClienteDto { Id = clienteExistenteId, Pontuacao = pontuacaoCliente };
            var produtoIdeal = new ProdutoDto
            {
                Id = 2,
                Rentabilidade = 0.15,
                Risco = EnumRiscoInvestimento.Alto.ToString(),
                PrazoMinimoResgateMeses = 18,
                Nome = "Ações Agressivas",
                Tipo = "Ações"
            };
            var produtosDisponiveis = new List<ProdutoDto>
            {
                produtoIdeal,
                new() { 
                    Id = 3, 
                    Rentabilidade = 0.20, 
                    Risco = EnumRiscoInvestimento.Alto.ToString(), 
                    PrazoMinimoResgateMeses = 30,
                    Nome = "Ações Muito Agressivas",
                    Tipo = "Ações"
                }
            };

            _repositoryMock.Setup(r => r.ObterClienteAsync(clienteExistenteId))
                           .ReturnsAsync(ResultadoDto<ClienteDto>.Ok(cliente));

            _repositoryMock.Setup(r => r.ObterProdutosPorTipoAsync(entrada.TipoProduto))
                           .ReturnsAsync(ResultadoDto<List<ProdutoDto>>.Ok(produtosDisponiveis));

            var resultado = await _service.SimularInvestimentoAsync(entrada);

            Assert.True(resultado.Sucesso);
            Assert.Equal(produtoIdeal.Id, resultado.Dado!.ProdutoValidado.Id);

            _repositoryMock.Verify(r => r.CriarCliente(It.IsAny<ClienteDto>()), Times.Never);

            _repositoryMock.Verify(r => r.GravarSimulacaoInvestimento(
                clienteExistenteId,
                produtoIdeal.Id,
                entrada.Valor,
                It.IsAny<ResponseSimulacaoInvestimentoDto>()), Times.Once);
        }

        [Fact]
        public async Task SimularInvestimentoAsync_DeveFalhar_QuandoNaoEncontraProdutoCompativeil()
        {
            uint clienteId = 100;
            ushort pontuacaoCliente = 10;
            var entrada = new RequestSimulacaoInvestimentoDto
            {
                ClienteId = clienteId,
                Valor = 100.0,
                PrazoMeses = 6,
                TipoProduto = "Renda Fixa"
            };

            var cliente = new ClienteDto { Id = clienteId, Pontuacao = pontuacaoCliente };
            var produtosDisponiveis = new List<ProdutoDto>
            {
                new() { 
                    Id = 1, 
                    Rentabilidade = 0.05, 
                    Risco = EnumRiscoInvestimento.Baixo.ToString(), 
                    PrazoMinimoResgateMeses = 12,
                    Nome = "Produto Renda Fixa 1",
                    Tipo = "Renda Fixa"
                }
            };

            _repositoryMock.Setup(r => r.ObterClienteAsync(clienteId))
                           .ReturnsAsync(ResultadoDto<ClienteDto>.Ok(cliente));
            _repositoryMock.Setup(r => r.ObterProdutosPorTipoAsync(entrada.TipoProduto))
                           .ReturnsAsync(ResultadoDto<List<ProdutoDto>>.Ok(produtosDisponiveis));

            var resultado = await _service.SimularInvestimentoAsync(entrada);

            Assert.False(resultado.Sucesso);
            Assert.Equal(ErrorCodes.ProdutoCompativelInexistente, resultado.Erro!.Codigo);

            _repositoryMock.Verify(r => r.GravarSimulacaoInvestimento(
                It.IsAny<uint>(),
                It.IsAny<uint>(),
                It.IsAny<double>(),
                It.IsAny<ResponseSimulacaoInvestimentoDto>()), Times.Never);
        }
    }
}