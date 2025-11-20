using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace API_Painel_Investimentos.Data.Entities
{
    public class UsuarioEntity : IdentityUser
    {
        [Required]
        public int Role { get; set; }
    }
}
