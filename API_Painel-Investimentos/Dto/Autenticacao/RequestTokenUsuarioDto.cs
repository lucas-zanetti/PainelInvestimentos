using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.Autenticacao
{
    [SwaggerSchema(Description = "DTO que representa os dados necessários para solicitar um token de autenticação de um usuário.")]
    public record RequestTokenUsuarioDto
    {
        [SwaggerParameter(Description = "Nome do usuário ou identificador usado na autenticação.")]
        public required string Usuario { get; set; }

        [SwaggerParameter(Description = "Senha do usuário. Deve ser tratada como informação sensível.")]
        public required string Senha { get; set; }
    }
}
