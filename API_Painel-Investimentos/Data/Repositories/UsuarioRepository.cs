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

        public async Task<ResultadoDto<int>> ObterUsuarioRolePorCredenciais(RequestTokenUsuarioDto credenciais)
        {
            var usuario = await _context.Users
                .SingleOrDefaultAsync(u => u.UserName == credenciais.Usuario);

            if (usuario is null)
                return ResultadoDto<int>.Falha(new ErroDto
                {
                    Codigo = ErrorCodes.CredenciaisInvalidas,
                    Mensagem = "Credenciais Inválidas."
                });

            var hasher = new PasswordHasher<UsuarioEntity>();
            var verification = hasher.VerifyHashedPassword(usuario, usuario.PasswordHash!, credenciais.Senha);

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
