using API_Painel_Investimentos.Dto.Autenticacao;
using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_Painel_Investimentos.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/")]
    public class AutenticacaoController(IAutenticacaoService autenticacaoService) : ControllerBase
    {
        private readonly IAutenticacaoService _autenticacaoService = autenticacaoService;

        [HttpPost("login", Name = "PostTokenUsuario")]
        [ProducesResponseType(typeof(ResponseTokenUsuarioDto), 200)]
        [ProducesResponseType(typeof(ErroDto), 400)]
        [ProducesResponseType(typeof(ErroDto), 500)]
        public async Task<ActionResult> PostTokenUsuario([FromBody] RequestTokenUsuarioDto entrada)
        {
            var resultado = await _autenticacaoService.GerarTokenUsuario(entrada);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erro);

            return Ok(resultado.Dado);
        }
    }
}
