using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Dto.Telemetria;
using API_Painel_Investimentos.Enums;
using API_Painel_Investimentos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Painel_Investimentos.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TelemetriaController(ITelemetriaService telemetriaService) : ControllerBase
    {
        private readonly ITelemetriaService _telemetriaService = telemetriaService;
        private const string _admin = nameof(UsuarioRoleEnum.Admin);
        private const string _colaboradorTI = nameof(UsuarioRoleEnum.ColaboradorTI);

        [Authorize(Roles = _admin + "," + _colaboradorTI)]
        [HttpGet("telemetria", Name = "GetTelemetria")]
        [ProducesResponseType(typeof(ResponseTelemetriaServicoDto), 200)]
        [ProducesResponseType(typeof(ErroDto), 400)]
        [ProducesResponseType(typeof(ErroDto), 500)]
        public async Task<ActionResult> GetTelemetria()
        {
            var resultado = await _telemetriaService.ObterDadosTelemetria();

            if (!resultado.Sucesso)
                return NotFound(resultado.Erro);

            return Ok(resultado.Dado);
        }
    }
}
