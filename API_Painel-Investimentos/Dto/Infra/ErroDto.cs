using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.Infra
{
    [SwaggerSchema(Description = "DTO que representa um erro ocorrido na aplicação, contendo o código e a mensagem de erro.")]
    public record ErroDto
    {
        [SwaggerParameter(Description = "Código identificador do erro.")]
        public string Codigo { get; set; } = null!;

        [SwaggerParameter(Description = "Mensagem descritiva do erro.")]
        public string Mensagem { get; set; } = null!;
    }
}
