using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.SimulacaoInvestimento
{
    [SwaggerSchema(Description = "DTO que representa os resultados de uma simulação de investimento, incluindo valor final previsto, rentabilidade efetiva e prazo em meses.")]
    public record ResultadoSimulacaoDto
    {
        [SwaggerParameter(Description = "Valor final estimado ao término do período da simulação.")]
        public double ValorFinal { get; set; }

        [SwaggerParameter(Description = "Rentabilidade efetiva obtida na simulação.")]
        public double RentabilidadeEfetiva { get; set; }

        [SwaggerParameter(Description = "Prazo da aplicação considerado na simulação, em meses.")]
        public short PrazoMeses { get; set; }
    }
}
