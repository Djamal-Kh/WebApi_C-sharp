using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiAnimal.Tests.Filtres
{
    public class CacheAttributeTests
    {
        private readonly Mock<ILogger<CacheAttributeTests>> _mockLogger;
        public CacheAttributeTests() 
        {
            _mockLogger = new Mock<ILogger<CacheAttributeTests>>();
        }
    }
}
