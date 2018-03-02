using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiquorCabinet.UnitTests
{
    public class UnitTestLoggerFactory
    {
        public static ILogger<T> CreateLogger<T>()
        {
            using (var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider())
            {
                return serviceProvider.GetService<ILoggerFactory>().CreateLogger<T>();
            }
        }
    }
}
