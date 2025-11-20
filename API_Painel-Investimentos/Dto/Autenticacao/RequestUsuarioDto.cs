using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.Autenticacao
{
    [SwaggerSchema(Description = "DTO que representa os dados necessários para requisições relacionadas a usuário (autenticação/registro).")]
    public record RequestUsuarioDto
    {
        [SwaggerParameter(Description = "Nome do usuário ou identificador utilizado na autenticação.")]
        public required string Usuario { get; set; }

        [SwaggerParameter(Description = "Senha do usuário. Deve ser tratada como informação sensível e transmitida/armazenada de forma segura.")]
        public required string Senha { get; set; }

        [SwaggerParameter(Description = "Papel ou perfil do usuário (por exemplo: 'Admin', 'User').")]
        public required string Role { get; set; }
    }
}
