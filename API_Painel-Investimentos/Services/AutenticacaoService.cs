using API_Painel_Investimentos.Dto.Autenticacao;
using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Enums;
using API_Painel_Investimentos.Interfaces;

namespace API_Painel_Investimentos.Services
{
    public class AutenticacaoService(IUsuarioRepository usuarioRepository, ITokenService tokenService) : IAutenticacaoService
    {
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;
        private readonly ITokenService _tokenService = tokenService;
        private const string _admin = nameof(UsuarioRoleEnum.Admin);

        public async Task<ResultadoDto<ResponseUsuarioDto>> CriarUsuarioAsync(RequestUsuarioDto entrada, string roleCriador)
        {
            if (entrada.Role.Equals(_admin, StringComparison.CurrentCultureIgnoreCase) && roleCriador != _admin)
                return ResultadoDto<ResponseUsuarioDto>.Falha(
                    new ErroDto
                    {
                        Codigo = ErrorCodes.RoleInvalida,
                        Mensagem = "Apenas usuários com o papel 'Admin' podem criar outros usuários com o papel 'Admin'."
                    });

            if (Enum.TryParse<UsuarioRoleEnum>(entrada.Role, ignoreCase: true, out var parsedRole))
                entrada.Role = parsedRole.ToString();
            else
                return ResultadoDto<ResponseUsuarioDto>.Falha(
                    new ErroDto
                    {
                        Codigo = ErrorCodes.RoleInexistente,
                        Mensagem = $"O papel '{entrada.Role}' é inexistente."
                    });

            return await _usuarioRepository.CriarUsuarioBancoAsync(entrada);
        }

        public async Task<ResultadoDto<ResponseTokenUsuarioDto>> GerarTokenUsuarioAsync(RequestTokenUsuarioDto entrada)
        {
            var resultadoRole = await _usuarioRepository.ObterUsuarioRolePorCredenciaisAsync(entrada);

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
