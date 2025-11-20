using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.Autenticacao
{
    [SwaggerSchema(Description = "DTO que representa os dados de criação de um usuário.")]
    public record ResponseUsuarioDto
    {
        [SwaggerParameter(Description = "Nome do usuário ou identificador utilizado na autenticação.")]
        public required string Usuario { get; set; }

        [SwaggerParameter(Description = "Papel ou perfil do usuário (por exemplo: 'Admin', 'User').")]
        public required string Role { get; set; }
    }
}
