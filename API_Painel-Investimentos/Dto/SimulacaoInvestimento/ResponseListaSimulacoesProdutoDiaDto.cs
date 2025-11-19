using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.SimulacaoInvestimento
{
    [SwaggerSchema(Description = "DTO que representa a resposta contendo uma lista de resumos de simulações de investimento por produto e por dia.")]
    public record ResponseListaSimulacoesProdutoDiaDto
    {
        [SwaggerParameter(Description = "Lista de objetos que resumem as simulações por produto para um dia específico (contém produto, data, quantidade de simulacões e média do valor final).")]
        public required List<SimulacaoInvestimentoProdutoDiaDto> Simulacoes { get; set; }
    }
}
