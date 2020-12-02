using Microsoft.Extensions.Logging;
using Polly;

namespace KeePass.Extensions
{
    internal static class PollyContextExtensions
    {
        private const string LoggerKey = "ILogger";

        /// <summary>
        /// Injects instance of <see cref="ILogger"/> into <see cref="Polly"/> <see cref="Context"/> instance to be used later by some policies.
        /// </summary>
        /// <param name="context">Instance of Polly's <see cref="Context"/> that given <see cref="ILogger"/> will be injected into</param>
        /// <param name="logger"><see cref="ILogger"/> instance to be injected into <see cref="Context"/></param>
        internal static Context WithLogger(this Context context, ILogger logger)
        {
            context[LoggerKey] = logger;
            return context;
        }

        /// <summary>
        /// Tries to return a instance of <see cref="ILogger"/> that could be injected into this instance of <see cref="Context"/>
        /// </summary>
        /// <param name="context"></param>
        /// <returns><see cref="ILogger"/> instance if any or <see langword="null"/> if none.</returns>
        internal static ILogger GetLogger(this Context context)
        {
            if (context.TryGetValue(LoggerKey, out object logger))
            {
                return logger as ILogger;
            }

            return null;
        }
    }
}