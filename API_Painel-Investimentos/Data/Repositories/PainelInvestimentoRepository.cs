using API_Painel_Investimentos.Data.Contexts;
using API_Painel_Investimentos.Data.Entities;
using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Dto.SimulacaoInvestimento;
using API_Painel_Investimentos.Enums;
using API_Painel_Investimentos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Painel_Investimentos.Data.Repositories
{
    public class PainelInvestimentoRepository(DbPainelInvestimentoContext context) : IPainelInvestimentoRepository
    {
        private readonly DbPainelInvestimentoContext _context = context;

        public Task CriarCliente(ClienteDto cliente)
        {
            _context.Clientes.Add(new ClienteEntity
            {
                Id = cliente.Id,
                Pontuacao = cliente.Pontuacao,
                Investimentos = []
            });

            return _context.SaveChangesAsync();
        }

        public async Task GravarSimulacaoInvestimento(uint clienteId, uint produtoId, double valorInvestido, ResponseSimulacaoInvestimentoDto simulacao)
        {
            var simulacaoEntity = new SimulacaoEntity
            {
                ClienteId = clienteId,
                Cliente = null!,
                ProdutoId = produtoId,
                Produto = null!,
                ValorInvestido = valorInvestido,
                PrazoMeses = simulacao.ResultadoSimulacao.PrazoMeses,
                ValorFinal = simulacao.ResultadoSimulacao.ValorFinal,
                Rentabilidade = simulacao.ResultadoSimulacao.RentabilidadeEfetiva,
                DataSimulacao = simulacao.DataSimulacao
            };

            _context.Simulacoes.Add(simulacaoEntity);

            await _context.SaveChangesAsync();
        }

        public async Task<ResultadoDto<ClienteDto>> ObterClienteAsync(uint clienteId)
        {
            var cliente = await _context.Clientes.FindAsync(clienteId);

            if (cliente == null)
                return ResultadoDto<ClienteDto>.Falha(new ErroDto
                {
                    Codigo = ErrorCodes.ClienteInexistente,
                    Mensagem = $"Cliente com ID {clienteId} não foi encontrado."
                });

            return ResultadoDto<ClienteDto>.Ok(new ClienteDto { Id = cliente.Id, Pontuacao = cliente.Pontuacao });
        }

        public async Task<ResultadoDto<PerfilRiscoDto>> ObterDetalhesPerfilRiscoAsync(string nomePerfil)
        {
            var perfil = await _context.PerfisRisco.Where(p => p.NomePerfil == nomePerfil).FirstOrDefaultAsync();

            if (perfil == null)
                return ResultadoDto<PerfilRiscoDto>.Falha(new ErroDto
                {
                    Codigo = ErrorCodes.PerfilRiscoInexistente,
                    Mensagem = $"Perfil de risco '{nomePerfil}' não foi encontrado."
                });

            return ResultadoDto<PerfilRiscoDto>.Ok(new PerfilRiscoDto
            {
                NomePerfil = perfil!.NomePerfil,
                DescricaoPerfil = perfil.DescricaoPerfil
            });
        }

        public async Task<ResultadoDto<List<InvestimentoClienteDto>>> ObterHistoricoInvestimentosClienteAsync(uint clienteId)
        {
            var investimentos = await _context.Simulacoes
                .Include(s => s.Produto)
                .Where(i => i.ClienteId == clienteId)
                .Select(i => new InvestimentoClienteDto
                {
                    Id = i.Id,
                    Tipo = i.Produto.Tipo,
                    Valor = i.ValorInvestido,
                    Rentabilidade = i.Rentabilidade,
                    Data = DateOnly.FromDateTime(i.DataSimulacao)
                }).ToListAsync();

            if (investimentos.Count == 0)
                return ResultadoDto<List<InvestimentoClienteDto>>.Falha(new ErroDto
                {
                    Codigo = ErrorCodes.InvestimentoInexistente,
                    Mensagem = $"Nenhum investimento foi encontrado para o cliente com ID {clienteId}."
                });

            return ResultadoDto<List<InvestimentoClienteDto>>.Ok(investimentos);
        }

        public async Task<ResultadoDto<List<ProdutoDto>>> ObterProdutosPorRiscoAsync(string riscoProduto)
        {
            var produtos = await _context.Produtos.Where(p => p.Risco == riscoProduto).ToListAsync();

            if (produtos.Count == 0)
                return ResultadoDto<List<ProdutoDto>>.Falha(new ErroDto
                {
                    Codigo = ErrorCodes.ProdutoInexistente,
                    Mensagem = $"Nenhum produto com o risco '{riscoProduto}' foi encontrado."
                });

            var produtosDto = produtos.Select(p => new ProdutoDto { Nome = p.Nome, Tipo = p.Tipo, Rentabilidade = p.Rentabilidade, Risco = p.Risco, PrazoMinimoResgateMeses = p.PrazoMinimoResgateMeses }).ToList();

            return ResultadoDto<List<ProdutoDto>>.Ok(produtosDto);
        }

        public async Task<ResultadoDto<List<ProdutoDto>>> ObterProdutosPorTipoAsync(string tipoProduto)
        {
            var produtos = await _context.Produtos.Where(p => p.Tipo == tipoProduto).ToListAsync();

            if(produtos.Count == 0)
                return ResultadoDto<List<ProdutoDto>>.Falha(new ErroDto
                {
                    Codigo = ErrorCodes.ProdutoInexistente,
                    Mensagem = $"Nenhum produto do tipo '{tipoProduto}' foi encontrado."
                });

            var produtosDto = produtos.Select(p => new ProdutoDto { Id = p.Id, Nome = p.Nome, Tipo = p.Tipo, Rentabilidade = p.Rentabilidade, Risco = p.Risco, PrazoMinimoResgateMeses = p.PrazoMinimoResgateMeses }).ToList();

            return ResultadoDto<List<ProdutoDto>>.Ok(produtosDto);
        }

        public async Task<ResultadoDto<List<SimulacaoInvestimentoProdutoDiaDto>>> ObterSimulacoesPorProdutoPorDiaAsync()
        {
            var simulacoes = await _context.Simulacoes
                .GroupBy(s => new { s.Produto!.Nome, Data = s.DataSimulacao.Date })
                .Select(g => new SimulacaoInvestimentoProdutoDiaDto
                {
                    Produto = g.Key.Nome,
                    Data = DateOnly.FromDateTime(g.Key.Data),
                    QuantidadeSimulacoes = g.Count(),
                    MediaValorFinal = g.Average(s => s.ValorFinal)
                }).ToListAsync();

            if (simulacoes.Count == 0)
                return ResultadoDto<List<SimulacaoInvestimentoProdutoDiaDto>>.Falha(new ErroDto
                {
                    Codigo = ErrorCodes.SimulacaoInexistente,
                    Mensagem = "Nenhuma simulação de investimento foi encontrada."
                });

            return ResultadoDto<List<SimulacaoInvestimentoProdutoDiaDto>>.Ok(simulacoes);
        }

        public async Task<ResultadoDto<List<SimulacaoInvestimentoDto>>> ObterTodasSimulacoesInvestimentoAsync()
        {
            var simulacoes = await _context.Simulacoes
                .Include(s => s.Cliente)
                .Include(s => s.Produto)
                .Select(s => new SimulacaoInvestimentoDto
                {
                    Id = s.Id,
                    ClienteId = s.Cliente.Id,
                    Produto = s.Produto!.Nome,
                    ValorInvestido = s.ValorInvestido,
                    ValorFinal = s.ValorFinal,
                    PrazoMeses = s.PrazoMeses,
                    DataSimulacao = s.DataSimulacao
                }).ToListAsync();

            if (simulacoes.Count == 0)
                return ResultadoDto<List<SimulacaoInvestimentoDto>>.Falha(new ErroDto
                {
                    Codigo = ErrorCodes.SimulacaoInexistente,
                    Mensagem = "Nenhuma simulação de investimento foi encontrada."
                });

            return ResultadoDto<List<SimulacaoInvestimentoDto>>.Ok(simulacoes);
        }
    }
}
