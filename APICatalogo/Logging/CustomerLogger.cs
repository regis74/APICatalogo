using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace APICatalogo.Logging
{
    public class CustomerLogger : ILogger
    {
        //private string name;
        //private CustomLoggerProviderConfiguration loggerConfig;

        //public CustomerLogger(string name, CustomLoggerProviderConfiguration loggerConfig)
        //{
        //    this.name = name;
        //    this.loggerConfig = loggerConfig;
        //}

        readonly string loggerName;
        readonly CustomLoggerProviderConfiguration loggerConfig;

        public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
        {
            loggerName = name;
            loggerConfig = config;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == loggerConfig.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, 
                                Exception exception, Func<TState, Exception,string> formatter)
        {
            string mensagem = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";
            
            EscreverTextoNoArquivo(mensagem);
        }

        private void EscreverTextoNoArquivo(string mensagem)
        {
            string caminhoArquivoLog = @"C:\Temp\log.txt";
            using (StreamWriter streamWriter = new StreamWriter(caminhoArquivoLog, true))
            {
                try
                {
                    streamWriter.WriteLine(mensagem);
                    streamWriter.Close();
                }
                catch (Exception ex)
                {

                    throw ex;

                }
            }
        }

    }
}
