using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyRates.Repository.Seeder
{
    public static class Seeder
    {
        public static async Task SeedData(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            var existingUser = await userManager.FindByEmailAsync("user@example.pl");
            if (existingUser == null)
            {
                var user = new IdentityUser
                {
                    UserName = "user@example.pl",
                    Email = "user@example.pl",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "User123!");
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Błąd tworzenia użytkownika: {error.Description}");
                    }
                }
                else
                {
                    Console.WriteLine("Utworzono użytkownika user@example.pl");
                }
            }
            else
            {
                Console.WriteLine("Użytkownik user@example.pl już istnieje");
            }
        }
    }
}