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

        #region PainelInvestimento Errors
        public const string ClienteInexistente = "PNL-CLNT-INEX";
        public const string ProdutoInexistente = "PNL-PRDT-INEX";
        public const string PerfilRiscoInexistente = "PNL-PRFL-INEX";
        public const string ProdutoCompativelInexistente = "PNL-PRDT-COMP-INEX";
        public const string SimulacaoInexistente = "PNL-SIML-INEX";
        public const string InvestimentoInexistente = "PNL-INVST-INEX";
        #endregion

        #region Filter Errors
        public const string FalhaSchemaController = "FLT-SCHEMA-CTRL";
        public const string TelemetriaSemDados = "TLMT-SEM-DADOS";
        public const string ErroInterno = "FLT-ERRO-INT";
        #endregion
    }
}
