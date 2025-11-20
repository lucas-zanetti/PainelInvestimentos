using API_Painel_Investimentos.Dto.Autenticacao;
using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Interfaces;

namespace API_Painel_Investimentos.Services
{
    public class AutenticacaoService(IUsuarioRepository usuarioRepository, ITokenService tokenService) : IAutenticacaoService
    {
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<ResultadoDto<ResponseTokenUsuarioDto>> GerarTokenUsuario(RequestTokenUsuarioDto entrada)
        {
            var resultadoRole = await _usuarioRepository.ObterUsuarioRolePorCredenciais(entrada);

            if (!resultadoRole.Sucesso)
                return ResultadoDto<ResponseTokenUsuarioDto>.Falha(resultadoRole.Erro!);

            var resultadoToken = _tokenService.GerarTokenUsuario(entrada.Usuario, resultadoRole.Dado);
            
            if (!resultadoToken.Sucesso)
                return ResultadoDto<ResponseTokenUsuarioDto>.Falha(resultadoToken.Erro!);

            return ResultadoDto<ResponseTokenUsuarioDto>.Ok(
                new ResponseTokenUsuarioDto 
                { 
                    Token = resultadoToken.Dado!
                });
        }
    }
}
