using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.Autenticacao
{
    [SwaggerSchema(Description = "DTO que representa o token retornado após a autenticação de um usuário.")]
    public record ResponseTokenUsuarioDto
    {
        [SwaggerParameter(Description = "Token JWT de autenticação do usuário.")]
        public required string Token { get; set; }
    }
}
