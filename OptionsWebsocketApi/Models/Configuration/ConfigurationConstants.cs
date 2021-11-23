namespace OptionsWebsocketApi.Models.Configuration
{
    public static class ConfigurationConstants
    {
        #region Service

        public const string ServiceName = "Service:Name";


        #endregion

        #region Logging

        public const string LoggingSerilogConsoleEnabled = "Logging:Serilog:ConsoleEnabled";
        public const string LoggingSerilogConsoleMinLevel = "Logging:Serilog:ConsoleMinLevel";
        public const string LoggingSerilogExcludeBoilerplate = "Logging:Serilog:ExcludeBoilerplate";

        #endregion

    }
}
