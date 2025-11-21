using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Dto.SimulacaoInvestimento;
using API_Painel_Investimentos.Enums;
using API_Painel_Investimentos.Interfaces;

namespace API_Painel_Investimentos.Services
{
    public class PainelInvestimentoService(IPainelInvestimentoRepository painelInvestimentoRepository) : IPainelInvestimentoService
    {
        private readonly IPainelInvestimentoRepository _painelInvestimentoRepository = painelInvestimentoRepository;

        public async Task<ResultadoDto<List<SimulacaoInvestimentoDto>>> ObterSimulacoesInvestimentoAsync()
        {
            var resultadoSimulacoes = await _painelInvestimentoRepository.ObterTodasSimulacoesInvestimentoAsync();

            if (!resultadoSimulacoes.Sucesso)
                return ResultadoDto<List<SimulacaoInvestimentoDto>>.Falha(resultadoSimulacoes.Erro!);

            return ResultadoDto<List<SimulacaoInvestimentoDto>>.Ok(resultadoSimulacoes.Dado!);
        }

        public async Task<ResultadoDto<List<SimulacaoInvestimentoProdutoDiaDto>>> ObterSimulacoesInvestimentoProdutoDiaAsync()
        {
            var resultadoSimulacoes = await _painelInvestimentoRepository.ObterSimulacoesPorProdutoPorDiaAsync();

            if (!resultadoSimulacoes.Sucesso)
                return ResultadoDto<List<SimulacaoInvestimentoProdutoDiaDto>>.Falha(resultadoSimulacoes.Erro!);

            return ResultadoDto<List<SimulacaoInvestimentoProdutoDiaDto>>.Ok(resultadoSimulacoes.Dado!);
        }

        public async Task<ResultadoDto<List<InvestimentoClienteDto>>> ObterHistoricoInvestimentosClienteAsync(uint clienteId)
        {
            var resultadoInvestimentos = await _painelInvestimentoRepository.ObterHistoricoInvestimentosClienteAsync(clienteId);

            if (!resultadoInvestimentos.Sucesso)
                return ResultadoDto<List<InvestimentoClienteDto>>.Falha(resultadoInvestimentos.Erro!);

            return ResultadoDto<List<InvestimentoClienteDto>>.Ok(resultadoInvestimentos.Dado!);
        }

        public async Task<ResultadoDto<ResponseSimulacaoInvestimentoDto>> SimularInvestimentoAsync(RequestSimulacaoInvestimentoDto entrada)
        {
            var resultadoCliente = await _painelInvestimentoRepository.ObterClienteAsync(entrada.ClienteId);

            var cliente = resultadoCliente.Dado;

            if (!resultadoCliente.Sucesso)
            {
                cliente = new ClienteDto { Id = entrada.ClienteId, Pontuacao = 25 };// Pontuação padrão para novos clientes, que não existem no sistema
                await _painelInvestimentoRepository.CriarCliente(cliente);
            }

            var produtoResultado = await ObterMelhorProdutoSimulacaoAsync(entrada, cliente!.Pontuacao);

            if (!produtoResultado.Sucesso)
                return ResultadoDto<ResponseSimulacaoInvestimentoDto>.Falha(produtoResultado.Erro!);

            var produto = produtoResultado.Dado!;
            var calculoSimulacao = CalcularSimulacao(entrada, produto.Rentabilidade);

            var simulacao = new ResponseSimulacaoInvestimentoDto
            {
                ProdutoValidado = produto,
                ResultadoSimulacao = calculoSimulacao,
                DataSimulacao = DateTime.UtcNow
            };

            await _painelInvestimentoRepository.GravarSimulacaoInvestimento(cliente, simulacao);

            return ResultadoDto<ResponseSimulacaoInvestimentoDto>.Ok(simulacao);
        }

        public async Task<ResultadoDto<ResponsePerfilRiscoDto>> ObterPerfilRiscoAsync(uint clienteId)
        {
            var resultadoCliente = await _painelInvestimentoRepository.ObterClienteAsync(clienteId);

            if (!resultadoCliente.Sucesso)
                return ResultadoDto<ResponsePerfilRiscoDto>.Falha(resultadoCliente.Erro!);

            var cliente = resultadoCliente.Dado!;

            var perfil = DefinirPerfilPorPontuacao(cliente.Pontuacao);

            var resultadoPerfil = await _painelInvestimentoRepository.ObterDetalhesPerfilRiscoAsync(perfil!);

            if (!resultadoPerfil.Sucesso)
                return ResultadoDto<ResponsePerfilRiscoDto>.Falha(resultadoPerfil.Erro!);

            return ResultadoDto<ResponsePerfilRiscoDto>.Ok(new ResponsePerfilRiscoDto
            {
                ClienteId = cliente.Id,
                Perfil = perfil,
                Pontuacao = cliente.Pontuacao,
                Descricao = resultadoPerfil.Dado!.DescricaoPerfil,
            });
        }

        public async Task<ResultadoDto<List<ProdutoDto>>> ObterProdutosPerfilAsync(string perfil)
        {
            var resultadoPerfil = await _painelInvestimentoRepository.ObterDetalhesPerfilRiscoAsync(perfil!);

            if (!resultadoPerfil.Sucesso)
                return ResultadoDto<List<ProdutoDto>>.Falha(resultadoPerfil.Erro!);

            var riscoResultado = DefinirRiscoPorPerfil(perfil);
            if (!riscoResultado.Sucesso)
                return ResultadoDto<List<ProdutoDto>>.Falha(riscoResultado.Erro!);

            var produtosResultado = await _painelInvestimentoRepository.ObterProdutosPorRiscoAsync(riscoResultado.Dado!);
            if (!produtosResultado.Sucesso)
                return ResultadoDto<List<ProdutoDto>>.Falha(produtosResultado.Erro!);

            return ResultadoDto<List<ProdutoDto>>.Ok(produtosResultado.Dado!);
        }

        private static ResultadoSimulacaoDto CalcularSimulacao(RequestSimulacaoInvestimentoDto entrada, double rentabilidadeProduto)
        {
            var taxaMensal = ConverterTaxaAnualParaMensal(rentabilidadeProduto);
            var fatorRentabilidade = Math.Pow(1 + taxaMensal, entrada.PrazoMeses);

            return new ResultadoSimulacaoDto
            {
                ValorFinal = double.Round(entrada.Valor * fatorRentabilidade, 2),
                RentabilidadeEfetiva = double.Round(fatorRentabilidade - 1, 5),
                PrazoMeses = entrada.PrazoMeses
            };
        }

        private static double ConverterTaxaAnualParaMensal(double taxaAnualDecimal)
        {
            const double mesesNoAno = 12.0;

            double fatorMensal = Math.Pow(1.0 + taxaAnualDecimal, 1.0 / mesesNoAno);

            double taxaMensalDecimal = fatorMensal - 1.0;

            return taxaMensalDecimal;
        }

        private static ResultadoDto<ProdutoDto> FiltrarMelhorProdutoPorRentabilidadeRisco(List<ProdutoDto> produtos, ushort pontuacaoRisco, uint prazoMeses)
        {
            EnumRiscoInvestimento riscoDesejadoEnum = ObterRiscoEnumPorPontuacao(pontuacaoRisco);

            var produtosComEnum = produtos.Select(p => new
            {
                Produto = p,
                RiscoEnum = Enum.Parse<EnumRiscoInvestimento>(p.Risco)
            });

            ProdutoDto? melhorProduto = produtosComEnum
                .Where(item => item.RiscoEnum <= riscoDesejadoEnum)
                .Where(item => item.Produto.PrazoMinimoResgateMeses < prazoMeses)
                .OrderByDescending(item => item.Produto.Rentabilidade)
                .ThenBy(item => item.Produto.PrazoMinimoResgateMeses)
                .ThenBy(item => item.Produto.Id)
                .Select(item => item.Produto)
                .FirstOrDefault();

            if (melhorProduto == null)
            {
                return ResultadoDto<ProdutoDto>.Falha(new ErroDto
                {
                    Codigo = ErrorCodes.ProdutoCompativelInexistente,
                    Mensagem = "Nenhum produto compatível encontrado para os critérios fornecidos."
                });
            }

            return ResultadoDto<ProdutoDto>.Ok(melhorProduto);
        }

        private static EnumRiscoInvestimento ObterRiscoEnumPorPontuacao(ushort pontuacaoRisco)
        {
            return pontuacaoRisco switch
            {
                >= 0 and < 25 => EnumRiscoInvestimento.Baixo,
                >= 25 and < 50 => EnumRiscoInvestimento.Medio,
                _ => EnumRiscoInvestimento.Alto,
            };
        }

        private static string DefinirPerfilPorPontuacao(ushort pontuacaoRisco)
        {
            return pontuacaoRisco <= 33 ? EnumPerfilRisco.Conservador.ToString() : (pontuacaoRisco <= 66 ? EnumPerfilRisco.Moderado.ToString() : EnumPerfilRisco.Agressivo.ToString());
        }

        private static ResultadoDto<string> DefinirRiscoPorPerfil(string perfil)
        {
            if (!Enum.TryParse<EnumPerfilRisco>(perfil, out var enumPerfil))
            {
                return ResultadoDto<string>.Falha(new ErroDto
                {
                    Codigo = ErrorCodes.PerfilRiscoInexistente,
                    Mensagem = "Perfil de risco inválido."
                });
            }

            return enumPerfil switch
            {
                EnumPerfilRisco.Conservador => ResultadoDto<string>.Ok(EnumRiscoInvestimento.Baixo.ToString()),
                EnumPerfilRisco.Moderado => ResultadoDto<string>.Ok(EnumRiscoInvestimento.Medio.ToString()),
                EnumPerfilRisco.Agressivo => ResultadoDto<string>.Ok(EnumRiscoInvestimento.Alto.ToString()),
                _ => ResultadoDto<string>.Falha(new ErroDto
                {
                    Codigo = ErrorCodes.PerfilRiscoInexistente,
                    Mensagem = "Perfil de risco inválido."
                }),
            };
        }

        private async Task<ResultadoDto<ProdutoDto>> ObterMelhorProdutoSimulacaoAsync(RequestSimulacaoInvestimentoDto entrada, ushort pontuacaoRisco)
        {
            var produtosResultado = await _painelInvestimentoRepository.ObterProdutosPorTipoAsync(entrada.TipoProduto);

            if (!produtosResultado.Sucesso)
                return ResultadoDto<ProdutoDto>.Falha(produtosResultado.Erro!);

            var melhorProdutoResultado = FiltrarMelhorProdutoPorRentabilidadeRisco(produtosResultado.Dado!, pontuacaoRisco, entrada.PrazoMeses);

            if (!melhorProdutoResultado.Sucesso)
                return ResultadoDto<ProdutoDto>.Falha(melhorProdutoResultado.Erro!);

            return ResultadoDto<ProdutoDto>.Ok(melhorProdutoResultado.Dado!);
        }
    }
}
