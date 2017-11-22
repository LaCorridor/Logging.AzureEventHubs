using System;
using System.Collections.Generic;
using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Logging;

namespace LaCorridor.Logging.AzureEventHubs
{
    public class EHLogger : ILogger
    {
        private readonly LogLevel _logLevel;
        private readonly EventHubClient _eventHubClient;
        private readonly string _category;
        private ILoggerScope _loggerScope;

        public EHLogger(EventHubClient eventHubClient, LogLevel logLevel, string category)
        {
            _logLevel = logLevel;
            _eventHubClient = eventHubClient;
            _category = category;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            LoggerScope newScope = new LoggerScope(_loggerScope, state);
            if (newScope != null)
            {
                _loggerScope = newScope;
            }
            return _loggerScope;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _logLevel;
        }

        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (_eventHubClient == null)
            {
                return;
            }

            if (!IsEnabled(logLevel))
            {
                return;
            }

            string message = formatter(state, exception);
            // Get Scope String
            string scope = GetScopeString();
            LogEntry logEntry = new LogEntry(logLevel, eventId, message, _category, scope);
            EventData eventData = logEntry.ToEventData();

            if (eventData != null)
            {
                await _eventHubClient.SendAsync(eventData);
            }
        }

        private string GetScopeString()
        {
            if (_loggerScope == null)
            {
                return null;
            }

            List<string> scopeList = new List<string>();
            ILoggerScope pointer = _loggerScope;
            while (pointer != null)
            {
                string scopeItem = pointer.ToString();
                if (!string.IsNullOrEmpty(scopeItem))
                {
                    scopeList.Add(scopeItem);
                }
                pointer = pointer.Parent;
            }
            scopeList.Reverse();
            string scopeProperty = string.Join(" => ", scopeList);
            if (string.IsNullOrEmpty(scopeProperty))
            {
                return null;
            }
            return scopeProperty;
        }
    }
}
