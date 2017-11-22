using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LaCorridor.Logging.AzureEventHubs
{
    internal class LoggerScope : ILoggerScope
    {
        public ILoggerScope Parent { get; set; }

        private bool _isDisposed = false;

        private string _stringifiedState;

        public LoggerScope(ILoggerScope parent, object state)
        {
            Parent = parent;
            _stringifiedState = StringifyState(state);
        }

        private string StringifyState(object state)
        {
            try
            {
                if (state is string stringState)
                {
                    return stringState;
                }
                else if (state is IDictionary<string, object> dict)
                {
                    return JsonConvert.SerializeObject(dict, Formatting.None);
                }
            }
            catch
            {
                return state.ToString();
            }
            return null;
        }

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (_isDisposed)
            {
                return;
            }
            if (isDisposing)
            {
                // Dispose possibly memory consumption managed resource.
                _stringifiedState = null;
            }
            // Dispose any unmanaged resource dispite isDisposing here.
            _isDisposed = true;
        }

        public override string ToString()
        {
            if (!_isDisposed)
            {
                return _stringifiedState;
            }
            return null;
        }
    }
}
