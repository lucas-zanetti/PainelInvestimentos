using API_Painel_Investimentos.Dto.Infra;
using API_Painel_Investimentos.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API_Painel_Investimentos.Filters
{
    public class SchemaValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var erros = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var mensagemErro = string.Join(" ", erros);

                context.Result = new BadRequestObjectResult(new ErroDto
                {
                    Codigo = ErrorCodes.FalhaSchemaController,
                    Mensagem = mensagemErro
                });
            }
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
