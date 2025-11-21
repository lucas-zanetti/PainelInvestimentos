using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.SimulacaoInvestimento
{
    [SwaggerSchema(Description = "DTO que representa um produto financeiro disponível para simulação, com identificador, nome, tipo, rentabilidade e nível de risco.")]
    public record ProdutoDto
    {
        [SwaggerParameter(Description = "Identificador único do produto.")]
        public uint Id { get; set; }

        [SwaggerParameter(Description = "Nome do produto financeiro.")]
        public required string Nome { get; set; }

        [SwaggerParameter(Description = "Tipo do produto (ex.: 'CDB', 'LCI', 'Fundo', 'TesouroDireto').")]
        public required string Tipo { get; set; }

        [SwaggerParameter(Description = "Rentabilidade do produto.")]
        public double Rentabilidade { get; set; }

        [SwaggerParameter(Description = "Nível de risco associado ao produto (ex.: 'Baixo', 'Médio', 'Alto').")]
        public required string Risco { get; set; }

        [SwaggerParameter(Description = "Prazo mínimo de resgate do produto, em mêses.")]
        public ushort PrazoMinimoResgateMeses { get; set; }
    }
}
