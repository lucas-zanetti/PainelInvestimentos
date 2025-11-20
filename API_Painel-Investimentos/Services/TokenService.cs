using API_Painel_Investimentos.Configuration;
using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Enums;
using API_Painel_Investimentos.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Painel_Investimentos.Services
{
    public class TokenService(IOptions<JwtConfiguration> config) : ITokenService
    {
        private readonly JwtConfiguration _jwtConfig = config.Value;

        public ResultadoDto<string> GerarTokenUsuario(string usuario, int role)
        {
            var resultadoRole = PegarRole(role);
            if (!resultadoRole.Sucesso)
                return resultadoRole;

            var chaveSecreta = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));

            var credenciais = new SigningCredentials(chaveSecreta, SecurityAlgorithms.HmacSha256);

            var opcoesToken = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                expires: DateTime.Now.AddMinutes(_jwtConfig.ExpirationTimeMinutes),
                signingCredentials: credenciais,
                claims:
                [
                    new Claim("usuario", usuario),
                    new Claim("role", resultadoRole.Dado!)
                ]
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(opcoesToken);

            return ResultadoDto<string>.Ok(tokenString);
        }

        private static ResultadoDto<string> PegarRole(int role)
        {
            if (!Enum.IsDefined(typeof(UsuarioRoleEnum), role))
            {
                return ResultadoDto<string>.Falha(new ErroDto
                {
                    Codigo = ErrorCodes.RoleInvalida,
                    Mensagem = "Role inválida."
                });
            }

            return ResultadoDto<string>.Ok(((UsuarioRoleEnum)role).ToString());
        }
    }
}
