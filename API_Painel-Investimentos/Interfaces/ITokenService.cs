using API_Painel_Investimentos.Dto.Infra;

namespace API_Painel_Investimentos.Interfaces
{
    public interface ITokenService
    {
        ResultadoDto<string> GerarTokenUsuario(string usuario, int role);
    }
}
