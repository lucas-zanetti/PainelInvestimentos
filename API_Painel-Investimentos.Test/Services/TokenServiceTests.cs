using Moq;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using API_Painel_Investimentos.Services;
using API_Painel_Investimentos.Configuration;
using API_Painel_Investimentos.Enums;

namespace API_Painel_Investimentos.Test.Services
{
    public class TokenServiceTests
    {
        private readonly TokenService _service;
        private readonly JwtConfiguration _config;
        private readonly Mock<IOptions<JwtConfiguration>> _optionsMock;

        public TokenServiceTests()
        {
            _config = new JwtConfiguration
            {
                Key = "uma-chave-secreta-forte-e-longa-para-teste-de-jwt-1234567890",
                Issuer = "PainelInvestimentosIssuer",
                Audience = "PainelInvestimentosAudience",
                ExpirationTimeMinutes = 60
            };

            _optionsMock = new Mock<IOptions<JwtConfiguration>>();
            _optionsMock.Setup(x => x.Value).Returns(_config);

            _service = new TokenService(_optionsMock.Object);
        }

        [Theory]
        [InlineData(0, "Admin")]
        [InlineData(2, "ColaboradorComercial")]
        public void GerarTokenUsuario_DeveRetornarTokenComClaimsCorretas_QuandoRoleValida(int roleInt, string expectedRoleString)
        {
            const string usuario = "teste@exemplo.com";

            var resultado = _service.GerarTokenUsuario(usuario, roleInt);

            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Dado);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(resultado.Dado);

            var usuarioClaim = token.Claims.FirstOrDefault(c => c.Type == "usuario");
            var roleClaim = token.Claims.FirstOrDefault(c => c.Type == "role");

            Assert.NotNull(usuarioClaim);
            Assert.NotNull(roleClaim);
            Assert.Equal(usuario, usuarioClaim.Value);
            Assert.Equal(expectedRoleString, roleClaim.Value);

            Assert.Equal(_config.Issuer, token.Issuer);
            Assert.Equal(_config.Audience, token.Audiences.First());

            var limiteInferior = DateTime.UtcNow.AddMinutes(_config.ExpirationTimeMinutes).AddSeconds(-5);
            var limiteSuperior = DateTime.UtcNow.AddMinutes(_config.ExpirationTimeMinutes).AddSeconds(5);
            Assert.InRange(token.ValidTo, limiteInferior, limiteSuperior);
        }

        [Fact]
        public void GerarTokenUsuario_DeveRetornarFalha_QuandoRoleInvalida()
        {
            const string usuario = "teste@exemplo.com";
            const int roleInvalida = 999;

            var resultado = _service.GerarTokenUsuario(usuario, roleInvalida);

            Assert.False(resultado.Sucesso);
            Assert.Null(resultado.Dado);
            Assert.NotNull(resultado.Erro);
            Assert.Equal(ErrorCodes.RoleInexistente, resultado.Erro!.Codigo);
            Assert.Equal("Role inexistente.", resultado.Erro.Mensagem);
        }
    }
}