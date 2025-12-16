using ApplicationAnimal.Services.Animals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ApplicationAnimal.BackgroundServ
{
    public class DecrementAnimalEnergyAsync : BackgroundService
    {
        // Используется IServiceScopeFactory чтобы создавать кастомную область жизни службы для того чтобы избежать ошибки "Captive Dependency"
        private readonly ILogger<DecrementAnimalEnergyAsync> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public DecrementAnimalEnergyAsync(IServiceScopeFactory scopeFactory, ILogger<DecrementAnimalEnergyAsync> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int decrementValue = 10;
            int delayTimeMS = 60000;

            if (decrementValue <= 0 || delayTimeMS <= 0)
                throw new ArgumentOutOfRangeException();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(delayTimeMS, stoppingToken);

                // При каждом вызове DecrementAnimalEnergyAsync (Singleton) создается новая область IAnimalRepository (Scoped) для того чтобы Singleton не хранил в себе постоянно один и тот же Scoped
                using (var scope = _scopeFactory.CreateScope()) 
                {
                    var scopeOfAnimalRepository = scope.ServiceProvider.GetRequiredService<IAnimalRepository>();
                    _logger.LogInformation("Reduction of animal energy by value {decrementValue}", decrementValue);
                    await scopeOfAnimalRepository.DecrementAnimalEnergyAsync(decrementValue);
                }
            }
        }
    }
}
