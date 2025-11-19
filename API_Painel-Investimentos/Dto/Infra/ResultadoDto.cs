using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.Infra
{
    [SwaggerSchema(Description = "DTO genérico que representa o resultado de uma operação, indicando sucesso, erro e o dado retornado.")]
    public record ResultadoDto<T>
    {
        [SwaggerParameter(Description = "Indica se a operação foi realizada com sucesso.")]
        public bool Sucesso { get; set; }

        [SwaggerParameter(Description = "Informações sobre o erro ocorrido, caso a operação não tenha sido bem-sucedida.")]
        public ErroDto? Erro { get; set; }

        [SwaggerParameter(Description = "Dado retornado pela operação, caso tenha sido bem-sucedida.")]
        public T? Dado { get; set; }

        private ResultadoDto()
        { }

        public static ResultadoDto<T> Ok(T dado)
        {
            return new ResultadoDto<T>
            {
                Sucesso = true,
                Dado = dado
            };
        }

        public static ResultadoDto<T> Falha(ErroDto erro)
        {
            return new ResultadoDto<T>
            {
                Sucesso = false,
                Erro = erro
            };
        }
    }
}
