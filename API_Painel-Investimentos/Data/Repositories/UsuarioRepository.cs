using API_Painel_Investimentos.Data.Contexts;
using API_Painel_Investimentos.Data.Entities;
using API_Painel_Investimentos.Dto.Autenticacao;
using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Enums;
using API_Painel_Investimentos.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API_Painel_Investimentos.Data.Repositories
{
    public class UsuarioRepository(DbUsuarioContext context) : IUsuarioRepository
    {
        private readonly DbUsuarioContext _context = context;
        private readonly PasswordHasher<UsuarioEntity> _hasher = new();

        public async Task<ResultadoDto<ResponseUsuarioDto>> CriarUsuarioBancoAsync(RequestUsuarioDto entrada)
        {
            var usuario = await _context.Users
                .SingleOrDefaultAsync(u => u.UserName == entrada.Usuario);

            if(usuario is not null)
                return ResultadoDto<ResponseUsuarioDto>.Falha(new ErroDto
                {
                    Codigo = ErrorCodes.UsuarioExistente,
                    Mensagem = "Já existe um usuário com este nome."
                });

            var usuarioEntity = new UsuarioEntity
            {
                UserName = entrada.Usuario,
                Role = (int)Enum.Parse<UsuarioRoleEnum>(entrada.Role)
            };

            usuarioEntity.PasswordHash = _hasher.HashPassword(usuarioEntity, entrada.Senha);

            _context.Users.Add(usuarioEntity);

            await _context.SaveChangesAsync();

            return ResultadoDto<ResponseUsuarioDto>.Ok(new ResponseUsuarioDto
            {
                Usuario = entrada.Usuario,
                Role = entrada.Role
            });
        }

        public async Task<ResultadoDto<int>> ObterUsuarioRolePorCredenciaisAsync(RequestTokenUsuarioDto credenciais)
        {
            var usuario = await _context.Users
                .SingleOrDefaultAsync(u => u.UserName == credenciais.Usuario);

            if (usuario is null)
                return ResultadoDto<int>.Falha(new ErroDto
                {
                    Codigo = ErrorCodes.CredenciaisInvalidas,
                    Mensagem = "Credenciais Inválidas."
                });

            var verification = _hasher.VerifyHashedPassword(usuario, usuario.PasswordHash!, credenciais.Senha);

            if (verification == PasswordVerificationResult.Success || verification == PasswordVerificationResult.SuccessRehashNeeded)
                return ResultadoDto<int>.Ok(usuario.Role);

            return ResultadoDto<int>.Falha(new ErroDto
            {
                Codigo = ErrorCodes.CredenciaisInvalidas,
                Mensagem = "Credenciais Inválidas."
            });
        }
    }
}
