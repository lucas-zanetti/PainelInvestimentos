using Swashbuckle.AspNetCore.Annotations;

namespace API_Painel_Investimentos.Dto.SimulacaoInvestimento
{
    [SwaggerSchema(Description = "DTO que representa informações essenciais de um cliente, incluindo identificador e pontuação.")]
    public record ClienteDto
    {
        [SwaggerParameter(Description = "Identificador único do cliente.")]
        public uint Id { get; set; }

        [SwaggerParameter(Description = "Pontuação do cliente (por exemplo, usada para perfil de risco ou scoring).")]
        public ushort Pontuacao { get; set; }
    }
}
