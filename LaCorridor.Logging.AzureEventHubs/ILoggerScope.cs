using System;

namespace LaCorridor.Logging.AzureEventHubs
{
    internal interface ILoggerScope : IDisposable
    {
        ILoggerScope Parent { get; set; }
    }
}
