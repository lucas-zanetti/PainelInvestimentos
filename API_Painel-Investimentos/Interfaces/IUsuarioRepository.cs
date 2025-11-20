using API_Painel_Investimentos.Dto.Autenticacao;
using API_Painel_Investimentos.Dto.Infra;

namespace API_Painel_Investimentos.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<ResultadoDto<int>> ObterUsuarioRolePorCredenciais(RequestTokenUsuarioDto credenciais);
    }
}
