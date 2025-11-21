using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Dto.SimulacaoInvestimento;

namespace API_Painel_Investimentos.Interfaces
{
    public interface IPainelInvestimentoRepository
    {
        Task<ResultadoDto<ClienteDto>> ObterClienteAsync(uint clienteId);
        Task CriarCliente(ClienteDto cliente);
        Task<ResultadoDto<List<ProdutoDto>>> ObterProdutosPorTipoAsync(string tipoProduto);
        Task<ResultadoDto<List<ProdutoDto>>> ObterProdutosPorRiscoAsync(string riscoProduto);
        Task GravarSimulacaoInvestimento(ClienteDto cliente, ResponseSimulacaoInvestimentoDto simulacao);
        Task<ResultadoDto<PerfilRiscoDto>> ObterDetalhesPerfilRiscoAsync(string nomePerfil);
        Task<ResultadoDto<List<SimulacaoInvestimentoDto>>> ObterTodasSimulacoesInvestimentoAsync();
        Task<ResultadoDto<List<SimulacaoInvestimentoProdutoDiaDto>>> ObterSimulacoesPorProdutoPorDiaAsync();
        Task<ResultadoDto<List<InvestimentoClienteDto>>> ObterHistoricoInvestimentosClienteAsync(uint clienteId);
    }
}
