using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooApi.Middlewares;

namespace WebApiAnimal.Tests.Middlewares
{
    public class ExceptionHandlingMiddlewareTests
    {
        private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _logger;
        private readonly DefaultHttpContext _context;

        public ExceptionHandlingMiddlewareTests()
        {
            _logger = new Mock<ILogger<ExceptionHandlingMiddleware>>();
            _context = new DefaultHttpContext();
        }

        // Разберись как надо будет написать
    }
}
