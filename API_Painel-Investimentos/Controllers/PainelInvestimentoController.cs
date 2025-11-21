using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Dto.SimulacaoInvestimento;
using API_Painel_Investimentos.Enums;
using API_Painel_Investimentos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Painel_Investimentos.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/")]
    public class PainelInvestimentoController(IPainelInvestimentoService simulacaoService) : ControllerBase
    {
        private readonly IPainelInvestimentoService _simulacaoService = simulacaoService;
        private const string _admin = nameof(UsuarioRoleEnum.Admin);
        private const string _colaboradorTI = nameof(UsuarioRoleEnum.ColaboradorTI);
        private const string _colaboradorComercial = nameof(UsuarioRoleEnum.ColaboradorComercial);

        [Authorize (Roles = _admin + "," + _colaboradorTI + "," + _colaboradorComercial)]
        [HttpPost("simular-investimento", Name = "PostSimulacaoInvestimento")]
        [ProducesResponseType(typeof(ResponseSimulacaoInvestimentoDto), 200)]
        [ProducesResponseType(typeof(ErroDto), 400)]
        [ProducesResponseType(typeof(ErroDto), 401)]
        [ProducesResponseType(typeof(ErroDto), 403)]
        [ProducesResponseType(typeof(ErroDto), 500)]
        public async Task<ActionResult> PostSimulacaoInvestimentoAsync([FromBody] RequestSimulacaoInvestimentoDto entrada)
        {
            var resultado = await _simulacaoService.SimularInvestimentoAsync(entrada);
            
            if (!resultado.Sucesso)
                return BadRequest(resultado.Erro!);

            return Ok(resultado.Dado!);
        }

        [Authorize(Roles = _admin + "," + _colaboradorTI + "," + _colaboradorComercial)]
        [HttpPost("simulacoes", Name = "GetSimulacoes")]
        [ProducesResponseType(typeof(List<SimulacaoInvestimentoDto>), 200)]
        [ProducesResponseType(typeof(ErroDto), 401)]
        [ProducesResponseType(typeof(ErroDto), 403)]
        [ProducesResponseType(typeof(ErroDto), 404)]
        [ProducesResponseType(typeof(ErroDto), 500)]
        public async Task<ActionResult> GetSimulacoesInvestimentoAsync()
        {
            var resultado = await _simulacaoService.ObterSimulacoesInvestimentoAsync();

            if (!resultado.Sucesso)
                return NotFound(resultado.Erro!);

            return Ok(resultado.Dado!);
        }

        [Authorize(Roles = _admin + "," + _colaboradorTI + "," + _colaboradorComercial)]
        [HttpGet("simulacoes/por-produto-dia", Name = "GetSimulacoesProdutoDia")]
        [ProducesResponseType(typeof(List<SimulacaoInvestimentoProdutoDiaDto>), 200)]
        [ProducesResponseType(typeof(ErroDto), 401)]
        [ProducesResponseType(typeof(ErroDto), 403)]
        [ProducesResponseType(typeof(ErroDto), 404)]
        [ProducesResponseType(typeof(ErroDto), 500)]
        public async Task<ActionResult> GetSimulacoesInvestimentoProdutoDiaAsync()
        {
            var resultado = await _simulacaoService.ObterSimulacoesInvestimentoProdutoDiaAsync();

            if (!resultado.Sucesso)
                return NotFound(resultado.Erro!);

            return Ok(resultado.Dado!);
        }

        [Authorize(Roles = _admin + "," + _colaboradorTI + "," + _colaboradorComercial)]
        [HttpGet("perfil-risco/{clienteId}", Name = "GetPerfilRiscoCliente")]
        [ProducesResponseType(typeof(ResponsePerfilRiscoDto), 200)]
        [ProducesResponseType(typeof(ErroDto), 401)]
        [ProducesResponseType(typeof(ErroDto), 403)]
        [ProducesResponseType(typeof(ErroDto), 404)]
        [ProducesResponseType(typeof(ErroDto), 500)]
        public async Task<ActionResult> GetPerfilRiscoClienteAsync(uint clienteId)
        {
            var resultado = await _simulacaoService.ObterPerfilRiscoAsync(clienteId);

            if (!resultado.Sucesso)
                return NotFound(resultado.Erro!);

            return Ok(resultado.Dado!);
        }

        [Authorize(Roles = _admin + "," + _colaboradorTI + "," + _colaboradorComercial)]
        [HttpGet("produtos-recomendados/{perfil}", Name = "GetProdutosRecomendadosPerfil")]
        [ProducesResponseType(typeof(List<ProdutoDto>), 200)]
        [ProducesResponseType(typeof(ErroDto), 401)]
        [ProducesResponseType(typeof(ErroDto), 403)]
        [ProducesResponseType(typeof(ErroDto), 404)]
        [ProducesResponseType(typeof(ErroDto), 500)]
        public async Task<ActionResult> GetProdutosRecomendadosPerfilAsync(string perfil)
        {
            var resultado = await _simulacaoService.ObterProdutosPerfilAsync(perfil);

            if (!resultado.Sucesso)
                return NotFound(resultado.Erro!);

            return Ok(resultado.Dado!);
        }

        [Authorize(Roles = _admin + "," + _colaboradorTI + "," + _colaboradorComercial)]
        [HttpGet("investimentos/{clienteId}", Name = "GetHistoricoInvestimentosCliente")]
        [ProducesResponseType(typeof(List<InvestimentoClienteDto>), 200)]
        [ProducesResponseType(typeof(ErroDto), 401)]
        [ProducesResponseType(typeof(ErroDto), 403)]
        [ProducesResponseType(typeof(ErroDto), 404)]
        [ProducesResponseType(typeof(ErroDto), 500)]
        public async Task<ActionResult> GetHistoricoInvestimentosClienteAsync(uint clienteId)
        {
            var resultado = await _simulacaoService.ObterHistoricoInvestimentosClienteAsync(clienteId);

            if (!resultado.Sucesso)
                return NotFound(resultado.Erro!);

            return Ok(resultado.Dado!);
        }
    }
}
