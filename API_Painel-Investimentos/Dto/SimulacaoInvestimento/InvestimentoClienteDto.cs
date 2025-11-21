using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.SimulacaoInvestimento
{
    [SwaggerSchema(Description = "DTO que representa um investimento associado a um cliente, incluindo tipo, valor aplicado, rentabilidade e data.")]
    public record InvestimentoClienteDto
    {
        [SwaggerParameter(Description = "Identificador único do investimento do cliente.")]
        public ulong Id { get; set; }

        [SwaggerParameter(Description = "Tipo do investimento (ex.: 'CDB', 'LCI', 'Fundo', etc.).")]
        public required string Tipo { get; set; }

        [SwaggerParameter(Description = "Valor aplicado no investimento, na unidade monetária do sistema.")]
        public double Valor { get; set; }

        [SwaggerParameter(Description = "Rentabilidade do investimento.")]
        public double Rentabilidade { get; set; }

        [SwaggerParameter(Description = "Data associada ao investimento.")]
        public DateOnly Data { get; set; }
    }
}