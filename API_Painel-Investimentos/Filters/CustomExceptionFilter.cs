using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API_Painel_Investimentos.Filters
{
    public class CustomExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            var erro = new ErroDto
            {
                Codigo = ErrorCodes.ErroInterno,
                Mensagem = "Ocorreu um erro interno no servidor."
            };

            context.Result = new ObjectResult(erro)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;

            return Task.CompletedTask;
        }
    }
}
