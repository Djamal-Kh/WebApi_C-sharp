using ApplicationAnimal.Services.Employees.Command.UpdateCommands.PaySalariesEmployee;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DomainAnimal;

namespace Infrastructure.BackgroundServices
{
    public class SalaryPaymentForEmployee : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly HybridCache _cache;

        public SalaryPaymentForEmployee(IServiceScopeFactory scopeFactory,
            HybridCache cache)
        {
            _scopeFactory = scopeFactory;
            _cache = cache;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int delayTimesMS = 120000;
            
            while(!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(delayTimesMS, stoppingToken);

                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopeOfEmployeeService = scope.ServiceProvider.GetRequiredService<PaySalariesEmployeeHandler>();

                    var result = await scopeOfEmployeeService.Handle(stoppingToken);

                    if (result.IsSuccess)
                        await _cache.RemoveByTagAsync(EmployeeConstants.EMPLOYEE_CACHE_TAG);
                }
            }
        }
    }
}
