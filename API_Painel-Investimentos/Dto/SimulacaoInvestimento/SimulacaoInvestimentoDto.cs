using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.SimulacaoInvestimento
{
    [SwaggerSchema(Description = "DTO que representa uma simulação de investimento, incluindo identificadores, produto, valores e data da simulação.")]
    public record SimulacaoInvestimentoDto
    {
        [SwaggerParameter(Description = "Identificador único da simulação.")]
        public ulong Id { get; set; }

        [SwaggerParameter(Description = "Identificador do cliente ao qual a simulação pertence.")]
        public uint ClienteId { get; set; }

        [SwaggerParameter(Description = "Nome do produto financeiro simulado.")]
        public required string Produto { get; set; }

        [SwaggerParameter(Description = "Valor inicialmente investido na simulação.")]
        public double ValorInvestido { get; set; }

        [SwaggerParameter(Description = "Valor final estimado pela simulação.")]
        public double ValorFinal { get; set; }

        [SwaggerParameter(Description = "Prazo da simulação em mêses.")]
        public ushort PrazoMeses { get; set; }

        [SwaggerParameter(Description = "Data e hora em que a simulação foi realizada.")]
        public DateTime DataSimulacao { get; set; }
    }
}
