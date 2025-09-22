using Infrastructure.ContextsDb;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Repositories
{
    /*  Только когда я дошел до написания тестов методов репозитория
        я понял, что можно было вынести в отдельный класс мокирование и 
        инициализацию различных сервисов, необходимых для функционирования тестов. 
        Т.е. стараюсь использовать принцип DRY*/

    public abstract class RepositoryTestBase : IDisposable
    {
        private readonly DbContextOptions<AppContextDB> _dbContextOptions;
        protected readonly AppContextDB DbContext;

        public RepositoryTestBase()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppContextDB>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            DbContext = new AppContextDB(_dbContextOptions);
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
