using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.SimulacaoInvestimento
{
    [SwaggerSchema(Description = "DTO que representa a resposta contendo uma lista de simulações de investimento. Cada item contém id, cliente, produto, valores e a data da simulação.")]
    public record ResponseListaSimulacoesInvestimentoDto
    {
        [SwaggerParameter(Description = "Lista de simulações de investimento retornadas pela API.")]
        public required List<SimulacaoInvestimentoDto> Simulacoes { get; set; }
    }
}
