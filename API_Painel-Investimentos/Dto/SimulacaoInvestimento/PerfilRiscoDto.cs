using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.SimulacaoInvestimento
{
    [SwaggerSchema(Description = "DTO que representa um perfil de risco.")]
    public record PerfilRiscoDto
    {
        [SwaggerParameter(Description = "Nome do perfil de risco.")]
        public required string NomePerfil { get; set; }
        [SwaggerParameter(Description = "Descrição do perfil de risco.")]
        public required string DescricaoPerfil { get; set; }
    }
}
