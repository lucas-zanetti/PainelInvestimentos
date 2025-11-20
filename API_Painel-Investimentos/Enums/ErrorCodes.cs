namespace API_Painel_Investimentos.Enums
{
    public static class ErrorCodes
    {
        #region Authentication Errors
        public const string CredenciaisInvalidas = "USR-CRED-INV";
        public const string RoleInvalida = "USR-ROLE-INV";
        public const string RoleInexistente = "USR-ROLE-INEX";
        public const string UsuarioExistente = "USR-EXIST";
        #endregion
    }
}
