using System.Linq;
using JSONPatchTutorial.Seeding;

namespace JSONPatchTutorial.Data
{
    public static class DbInitializer
    {
        public static void Initialize(JsonPatchDbContext context)
        {
            context.Database.EnsureCreated();

            AddHouses(context);

            AddAddresses(context);

            context.SaveChanges();
        }

        private static void AddHouses(JsonPatchDbContext context)
        {
            if (context.Houses.Any())
            {
                return;
            }
            
            foreach (var house in Houses.GetSeededHouses())
            {
                context.Houses.Add(house);
            }
        }
        
        private static void AddAddresses(JsonPatchDbContext context)
        {
            if (context.Addresses.Any())
            {
                return;
            }
            
            foreach (var address in Addresses.GetSeeded())
            {
                context.Addresses.Add(address);
            }
        }
    }
}