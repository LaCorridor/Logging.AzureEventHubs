using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LaCorridor.Logging.AzureEventHubs
{
    public static class LoggerExtensions
    {
        [Obsolete("Call AddEventHub() instead.", false)]
        public static ILoggerFactory AddEventHubLogger(this ILoggerFactory factory, string eventHubConnectionString, LogLevel minLogLevel = LogLevel.Warning)
        {
            return AddEventHub(factory, eventHubConnectionString, minLogLevel);
        }

        public static ILoggerFactory AddEventHub(this ILoggerFactory factory, string eventHubConnectionString, LogLevel minLogLevel = LogLevel.Warning)
        {
            factory.AddProvider(new EHLoggerProvider(minLogLevel, eventHubConnectionString));
            return factory;
        }

        public static ILoggerFactory AddEventHub(this ILoggerFactory factory, IConfiguration configuration)
        {
            Tuple<string, LogLevel> p = GetParametersFromConfiguration(configuration);
            return AddEventHub(factory, p.Item1, p.Item2);
        }

#if NETSTANDARD2_0
        public static ILoggingBuilder AddEventHub(this ILoggingBuilder builder, string eventHubConnectionString, LogLevel minLogLevel = LogLevel.Warning)
        {
            builder.Services.AddSingleton<ILoggerProvider, EHLoggerProvider>((serviceProvider) =>
            {
                return new EHLoggerProvider(minLogLevel, eventHubConnectionString);
            });

            return builder;
        }

        public static ILoggingBuilder AddEventHub(this ILoggingBuilder builder, IConfiguration configuration)
        {
            Tuple<string, LogLevel> p = GetParametersFromConfiguration(configuration);
            return AddEventHub(builder, p.Item1, p.Item2);
        }
#endif

        private static Tuple<string, LogLevel> GetParametersFromConfiguration(IConfiguration configuration)
        {
            IConfigurationSection eventHubSection = configuration?.GetSection("Logging:EventHubs");
            if (eventHubSection == null)
            {
                throw new NullReferenceException("Logging:EventHub section is missing from the configuration.");
            }
            string connectionString = Arguments.IsNotNullOrEmpty(eventHubSection["ConnectionString"], "Logging:EventHubs:ConnectionString");
            string minLogLevel = eventHubSection["LogLevel"];
            if (string.IsNullOrEmpty(minLogLevel))
            {
                minLogLevel = LogLevel.Warning.ToString();
            }
            LogLevel logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), minLogLevel, ignoreCase: false);

            return new Tuple<string, LogLevel>(connectionString, logLevel);
        }
    }
}
