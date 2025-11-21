using Moq;
using API_Painel_Investimentos.Interfaces;
using API_Painel_Investimentos.Services;
using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Dto.Autenticacao;
using API_Painel_Investimentos.Enums;

namespace API_Painel_Investimentos.Test.Services
{
    public class AutenticacaoServiceTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly AutenticacaoService _service;
        private readonly ErroDto _erroCredenciaisInvalidas = new() { Codigo = ErrorCodes.CredenciaisInvalidas, Mensagem = "Credenciais inválidas." };

        public AutenticacaoServiceTests()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _tokenServiceMock = new Mock<ITokenService>();
            _service = new AutenticacaoService(_usuarioRepositoryMock.Object, _tokenServiceMock.Object);
        }

        [Fact]
        public async Task CriarUsuarioAsync_DeveFalhar_QuandoNaoAdminCriaAdmin()
        {
            var entrada = new RequestUsuarioDto { Usuario = "novoAdmin", Senha = "123", Role = "Admin" };
            var roleCriador = UsuarioRoleEnum.ColaboradorComercial.ToString();

            var resultado = await _service.CriarUsuarioAsync(entrada, roleCriador);

            Assert.False(resultado.Sucesso);
            Assert.Equal(ErrorCodes.RoleInvalida, resultado.Erro!.Codigo);
            _usuarioRepositoryMock.Verify(r => r.CriarUsuarioBancoAsync(It.IsAny<RequestUsuarioDto>()), Times.Never);
        }

        [Fact]
        public async Task CriarUsuarioAsync_DeveTerSucesso_QuandoAdminCriaAdmin()
        {
            var entrada = new RequestUsuarioDto { Usuario = "novoAdmin", Senha = "123", Role = "Admin" };
            var roleCriador = UsuarioRoleEnum.Admin.ToString();
            var responseEsperado = new ResponseUsuarioDto { Usuario = entrada.Usuario, Role = "Admin" };

            _usuarioRepositoryMock.Setup(r => r.CriarUsuarioBancoAsync(It.IsAny<RequestUsuarioDto>()))
                                  .ReturnsAsync(ResultadoDto<ResponseUsuarioDto>.Ok(responseEsperado));

            var resultado = await _service.CriarUsuarioAsync(entrada, roleCriador);

            Assert.True(resultado.Sucesso);
            Assert.Equal("Admin", resultado.Dado!.Role);
            _usuarioRepositoryMock.Verify(r => r.CriarUsuarioBancoAsync(It.IsAny<RequestUsuarioDto>()), Times.Once);
        }

        [Fact]
        public async Task CriarUsuarioAsync_DeveFalhar_QuandoRoleInexistente()
        {
            var entrada = new RequestUsuarioDto { Usuario = "invalido", Senha = "123", Role = "Gerente" };
            var roleCriador = UsuarioRoleEnum.Admin.ToString();

            var resultado = await _service.CriarUsuarioAsync(entrada, roleCriador);

            Assert.False(resultado.Sucesso);
            Assert.Equal(ErrorCodes.RoleInexistente, resultado.Erro!.Codigo);
            _usuarioRepositoryMock.Verify(r => r.CriarUsuarioBancoAsync(It.IsAny<RequestUsuarioDto>()), Times.Never);
        }
    }
}