using API_Painel_Investimentos.Dto.Autenticacao;
using API_Painel_Investimentos.Dto.Infra;

namespace API_Painel_Investimentos.Interfaces
{
    public interface IAutenticacaoService
    {
        Task<ResultadoDto<ResponseTokenUsuarioDto>> GerarTokenUsuarioAsync(RequestTokenUsuarioDto entrada);
        Task<ResultadoDto<ResponseUsuarioDto>> CriarUsuarioAsync(RequestUsuarioDto entrada, string roleCriador);
    }
}
