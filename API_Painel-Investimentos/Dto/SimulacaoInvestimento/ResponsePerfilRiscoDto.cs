using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.SimulacaoInvestimento
{
    [SwaggerSchema(Description = "DTO que representa o perfil de risco associado a um cliente, incluindo o nome do perfil, pontuação e descrição detalhada.")]
    public record ResponsePerfilRiscoDto
    {
        [SwaggerParameter(Description = "Identificador do cliente ao qual o perfil de risco pertence.")]
        public int ClienteId { get; set; }

        [SwaggerParameter(Description = "Nome do perfil de risco (ex.: 'Conservador', 'Moderado', 'Agressivo').")]
        public required string Perfil { get; set; }

        [SwaggerParameter(Description = "Pontuação numérica que representa o nível de risco do cliente.")]
        public short Pontuacao { get; set; }

        [SwaggerParameter(Description = "Descrição detalhada do perfil de risco e recomendações associadas.")]
        public required string Descricao { get; set; }
    }
}
