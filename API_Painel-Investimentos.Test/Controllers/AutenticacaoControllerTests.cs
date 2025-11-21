using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using API_Painel_Investimentos.Controllers;
using API_Painel_Investimentos.Interfaces;
using API_Painel_Investimentos.Enums;
using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Dto.Autenticacao;

namespace API_Painel_Investimentos.Test.Controllers
{
    public class AutenticacaoControllerTests
    {
        private readonly Mock<IAutenticacaoService> _mockAutenticacaoService;
        private readonly AutenticacaoController _controller;

        public AutenticacaoControllerTests()
        {
            _mockAutenticacaoService = new Mock<IAutenticacaoService>();
            _controller = new AutenticacaoController(_mockAutenticacaoService.Object);
        }

        [Fact]
        public async Task PostTokenUsuarioAsync_CredenciaisValidas_RetornaOkComToken()
        {
            var request = new RequestTokenUsuarioDto { Usuario = "teste@mail.com", Senha = "senha123" };

            var tokenResponse = new ResponseTokenUsuarioDto
            {
                Token = "jwt.token.mock"
            };

            var resultadoSucesso = ResultadoDto<ResponseTokenUsuarioDto>
                .Ok(tokenResponse);

            _mockAutenticacaoService
                .Setup(s => s.GerarTokenUsuarioAsync(request))
                .ReturnsAsync(resultadoSucesso);

            var resultado = await _controller.PostTokenUsuarioAsync(request);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var dadoRetornado = Assert.IsType<ResponseTokenUsuarioDto>(okResult.Value);
            Assert.Equal("jwt.token.mock", dadoRetornado.Token);
        }

        [Fact]
        public async Task PostTokenUsuarioAsync_CredenciaisInvalidas_RetornaUnauthorized()
        {
            var request = new RequestTokenUsuarioDto { Usuario = "errado@mail.com", Senha = "errado" };

            var erro = new ErroDto
            {
                Codigo = ErrorCodes.CredenciaisInvalidas,
                Mensagem = "Usuário ou senha inválidos."
            };

            var resultadoFalha = ResultadoDto<ResponseTokenUsuarioDto>
                .Falha(erro);

            _mockAutenticacaoService
                .Setup(s => s.GerarTokenUsuarioAsync(request))
                .ReturnsAsync(resultadoFalha);

            var resultado = await _controller.PostTokenUsuarioAsync(request);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(resultado);
            Assert.Equal(401, unauthorizedResult.StatusCode);
            var erroRetornado = Assert.IsType<ErroDto>(unauthorizedResult.Value);
            Assert.Equal(ErrorCodes.CredenciaisInvalidas, erroRetornado.Codigo);
        }

        [Fact]
        public async Task PostTokenUsuarioAsync_RoleInexistente_RetornaForbidden()
        {
            var request = new RequestTokenUsuarioDto { Usuario = "user@semrole.com", Senha = "senha" };

            var erro = new ErroDto
            {
                Codigo = ErrorCodes.RoleInexistente,
                Mensagem = "Role do usuário inválida ou inexistente."
            };

            var resultadoFalha = ResultadoDto<ResponseTokenUsuarioDto>
                .Falha(erro);

            _mockAutenticacaoService
                .Setup(s => s.GerarTokenUsuarioAsync(request))
                .ReturnsAsync(resultadoFalha);

            var resultado = await _controller.PostTokenUsuarioAsync(request);

            var statusCodeResult = Assert.IsType<ObjectResult>(resultado);
            Assert.Equal(403, statusCodeResult.StatusCode);
            var erroRetornado = Assert.IsType<ErroDto>(statusCodeResult.Value);
            Assert.Equal(ErrorCodes.RoleInexistente, erroRetornado.Codigo);
        }

        [Theory]
        [InlineData("Admin")]
        [InlineData("ColaboradorTI")]
        public async Task PostUsuarioAsync_UsuarioAutorizado_RetornaOkComUsuarioCriado(string roleCriador)
        {
            var request = new RequestUsuarioDto
            {
                Usuario = "novo@mail.com",
                Senha = "senha",
                Role = "Cliente"
            };

            var usuarioResponse = new ResponseUsuarioDto
            {
                Usuario = "novo@mail.com",
                Role = "Cliente"
            };

            var resultadoSucesso = ResultadoDto<ResponseUsuarioDto>
                .Ok(usuarioResponse);

            SetupControllerUser(_controller, roleCriador);

            _mockAutenticacaoService
                .Setup(s => s.CriarUsuarioAsync(It.IsAny<RequestUsuarioDto>(), roleCriador))
                .ReturnsAsync(resultadoSucesso);

            var resultado = await _controller.PostUsuarioAsync(request);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var dadoRetornado = Assert.IsType<ResponseUsuarioDto>(okResult.Value);
            Assert.Equal("novo@mail.com", dadoRetornado.Usuario);

            _mockAutenticacaoService.Verify(s => s.CriarUsuarioAsync(It.IsAny<RequestUsuarioDto>(), roleCriador), Times.Once);
        }

        [Fact]
        public async Task PostUsuarioAsync_RoleInvalidaParaCriacao_RetornaForbidden()
        {
            const string roleCriador = "Admin";
            var request = new RequestUsuarioDto
            {
                Usuario = "novo@mail.com",
                Senha = "senha",
                Role = "Cliente"
            };

            var erro = new ErroDto
            {
                Codigo = ErrorCodes.RoleInvalida,
                Mensagem = "Não é permitido criar usuários com essa Role."
            };

            var resultadoFalha = ResultadoDto<ResponseUsuarioDto>
                .Falha(erro);

            SetupControllerUser(_controller, roleCriador);

            _mockAutenticacaoService
                .Setup(s => s.CriarUsuarioAsync(It.IsAny<RequestUsuarioDto>(), roleCriador))
                .ReturnsAsync(resultadoFalha);

            var resultado = await _controller.PostUsuarioAsync(request);

            var statusCodeResult = Assert.IsType<ObjectResult>(resultado);
            Assert.Equal(403, statusCodeResult.StatusCode);
            var erroRetornado = Assert.IsType<ErroDto>(statusCodeResult.Value);
            Assert.Equal(ErrorCodes.RoleInvalida, erroRetornado.Codigo);
        }

        private static void SetupControllerUser(ControllerBase controller, string role)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, "1"),
                new(ClaimTypes.Name, "TestUser"),
                new(ClaimTypes.Role, role)
            };

            if (role != null)
            {
                claims.Add(new Claim("role", role));
            }

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
        }
    }
}