using API_Painel_Investimentos.Dto.Autenticacao;
using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Enums;
using API_Painel_Investimentos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_Painel_Investimentos.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/")]
    public class AutenticacaoController(IAutenticacaoService autenticacaoService) : ControllerBase
    {
        private readonly IAutenticacaoService _autenticacaoService = autenticacaoService;
        private const string _admin = nameof(UsuarioRoleEnum.Admin);
        private const string _colaboradorTI = nameof(UsuarioRoleEnum.ColaboradorTI);

        [AllowAnonymous]
        [HttpPost("login", Name = "PostTokenUsuario")]
        [ProducesResponseType(typeof(ResponseTokenUsuarioDto), 200)]
        [ProducesResponseType(typeof(ErroDto), 400)]
        [ProducesResponseType(typeof(ErroDto), 401)]
        [ProducesResponseType(typeof(ErroDto), 403)]
        [ProducesResponseType(typeof(ErroDto), 500)]
        public async Task<ActionResult> PostTokenUsuarioAsync([FromBody] RequestTokenUsuarioDto entrada)
        {
            var resultado = await _autenticacaoService.GerarTokenUsuarioAsync(entrada);

            if (!resultado.Sucesso && resultado.Erro!.Codigo == ErrorCodes.CredenciaisInvalidas)
                return Unauthorized(resultado.Erro);

            if (!resultado.Sucesso && resultado.Erro!.Codigo == ErrorCodes.RoleInexistente)
                return StatusCode(403, resultado.Erro);

            return Ok(resultado.Dado);
        }
        
        [Authorize(Roles = _admin + "," + _colaboradorTI)]
        [HttpPost("criar-usuario", Name = "PostUsuario")]
        [ProducesResponseType(typeof(ResponseUsuarioDto), 200)]
        [ProducesResponseType(typeof(ErroDto), 400)]
        [ProducesResponseType(typeof(ErroDto), 401)]
        [ProducesResponseType(typeof(ErroDto), 403)]
        [ProducesResponseType(typeof(ErroDto), 500)]
        public async Task<ActionResult> PostUsuarioAsync([FromBody] RequestUsuarioDto entrada)
        {
            var roleCriador = User.Claims
                    .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                    .Select(c => c.Value)
                    .First();

            var resultado = await _autenticacaoService.CriarUsuarioAsync(entrada, roleCriador);

            if (!resultado.Sucesso && resultado.Erro!.Codigo == ErrorCodes.RoleInvalida)
                return StatusCode(403, resultado.Erro);

            if (!resultado.Sucesso 
                && (resultado.Erro!.Codigo == ErrorCodes.RoleInexistente
                || resultado.Erro!.Codigo == ErrorCodes.UsuarioExistente))
                return BadRequest(resultado.Erro);

            return Ok(resultado.Dado);
        }
    }
}
