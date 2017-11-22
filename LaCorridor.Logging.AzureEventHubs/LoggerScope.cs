using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                else if (state is IEnumerable<KeyValuePair<string, object>> list)
                {
                    StringBuilder builder = new StringBuilder();
                    if (list.Any())
                    {
                        builder.Append("[");

                        foreach (KeyValuePair<string, object> pair in list)
                        {
                            builder.Append($@"{{""{pair.Key}"":""{pair.Value}""}},");
                        }

                        string result = builder.ToString();
                        result = result.Substring(0, result.Length - 1) + ']';
                        return result;
                    }
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
