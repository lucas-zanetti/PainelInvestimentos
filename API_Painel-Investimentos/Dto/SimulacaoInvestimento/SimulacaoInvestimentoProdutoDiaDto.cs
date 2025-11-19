using System;
using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.SimulacaoInvestimento
{
    [SwaggerSchema(Description = "Resumo das simulações de investimento por produto em um dia: produto, data, quantidade de simulações e média do valor final.")]
    public record SimulacaoInvestimentoProdutoDiaDto
    {
        [SwaggerParameter(Description = "Nome do produto financeiro.")]
        public required string Produto { get; set; }

        [SwaggerParameter(Description = "Data da agregação (apenas data).")]
        public DateOnly Data { get; set; }

        [SwaggerParameter(Description = "Quantidade de simulações realizadas para o produto na data.")]
        public int QuantidadeSimulacoes { get; set; }

        [SwaggerParameter(Description = "Média do valor final das simulações.")]
        public double MediaValorFinal { get; set; }
    }
}
