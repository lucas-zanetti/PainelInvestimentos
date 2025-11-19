using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.SimulacaoInvestimento
{
    [SwaggerSchema(Description = "DTO que representa a resposta de uma simulação de investimento, incluindo o produto validado, o resultado da simulação e a data/hora em que foi gerada.")]
    public record ResponseSimulacaoInvestimentoDto
    {
        [SwaggerParameter(Description = "Dados do produto financeiro validado para a simulação (id, nome, tipo, rentabilidade e risco).")]
        public required ProdutoDto ProdutoValidado { get; set; }

        [SwaggerParameter(Description = "Resultados calculados da simulação, como valor final, rentabilidade efetiva e prazo em meses.")]
        public required ResultadoSimulacaoDto ResultadoSimulacao { get; set; }

        [SwaggerParameter(Description = "Data e hora em que a simulação foi realizada.")]
        public DateTime DataSimulacao { get; set; }
    }
}
