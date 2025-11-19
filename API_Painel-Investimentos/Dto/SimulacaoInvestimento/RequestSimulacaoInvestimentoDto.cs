using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.SimulacaoInvestimento
{
    [SwaggerSchema(Description = "DTO que representa os dados necessários para solicitar uma simulação de investimento.")]
    public record RequestSimulacaoInvestimentoDto
    {
        [SwaggerParameter(Description = "Identificador do cliente para quem a simulação será realizada.")]
        public int ClienteId { get; set; }

        [SwaggerParameter(Description = "Valor do investimento a ser simulado, em reais.")]
        public double Valor { get; set; }

        [SwaggerParameter(Description = "Prazo da aplicação em meses.")]
        public short PrazoMeses { get; set; }

        [SwaggerParameter(Description = "Tipo de produto financeiro (ex.: 'CDB', 'LCI', 'TesouroDireto').")]
        public required string TipoProduto { get; set; }
    }
}
