using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Dto.SimulacaoInvestimento;

namespace API_Painel_Investimentos.Interfaces
{
    public interface IPainelInvestimentoService
    {
        Task<ResultadoDto<ResponseSimulacaoInvestimentoDto>> SimularInvestimentoAsync(RequestSimulacaoInvestimentoDto entrada);
        Task<ResultadoDto<List<SimulacaoInvestimentoDto>>> ObterSimulacoesInvestimentoAsync();
        Task<ResultadoDto<List<SimulacaoInvestimentoProdutoDiaDto>>> ObterSimulacoesInvestimentoProdutoDiaAsync();
        Task<ResultadoDto<ResponsePerfilRiscoDto>> ObterPerfilRiscoAsync(uint clienteId);
        Task<ResultadoDto<List<ProdutoDto>>> ObterProdutosPerfilAsync(string perfil);
        Task<ResultadoDto<List<InvestimentoClienteDto>>> ObterHistoricoInvestimentosClienteAsync(uint clienteId);
    }
}
