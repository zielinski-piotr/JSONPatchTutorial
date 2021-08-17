using System;
using System.Collections.Generic;
using System.Linq;
using JSONPatchTutorial.Domain;

namespace JSONPatchTutorial.Seeding
{
    public static class Addresses
    {
        public static readonly Address Address1 = Address.Create(Guid.Parse("f98247b5-bc43-410f-92f0-38668ddb7e9b"),
            "Street1", "1", "City1", "Country1");

        public static readonly Address Address2 = Address.Create(Guid.Parse("65c1688a-8427-4635-9387-cbb436c81305"),
            "Street2", "2", "City1", "Country1");

        public static readonly Address Address3 = Address.Create(Guid.Parse("9bb47c15-cbfb-4edb-9595-b0f04c60b6bb"),
            "Street3", "3", "City1", "Country1");

        public static readonly Address Address4 = Address.Create(Guid.Parse("8d285b9f-db96-4b15-9de7-c3203f98b164"),
            "Street4", "4", "City1", "Country1", "1");

        public static readonly Address Address5 = Address.Create(Guid.Parse("da8e751b-3cca-4a2a-8e59-5e8392c571b3"),
            "Street5", "5", "City1", "Country1");

        public static readonly Address Address6 = Address.Create(Guid.Parse("78358f9b-24b9-4eea-bb2c-d34efaa24858"),
            "Street6", "6", "City1", "Country1");

        public static readonly Address Address7 = Address.Create(Guid.Parse("8d8b6f53-3098-4dd2-ad4e-b4c6d817d00a"),
            "Street7", "7", "City1", "Country1");

        public static readonly Address Address8 = Address.Create(Guid.Parse("8ad26a6d-beaa-4287-bbff-2e8c76eaabaa"),
            "Street8", "8", "City1", "Country1");

        public static readonly Address Address9 = Address.Create(Guid.Parse("eb064491-6fa7-4c77-bdc4-cdc0fd9add6a"),
            "Street9", "9", "City1", "Country1");

        public static readonly Address Address10 = Address.Create(Guid.Parse("e5a79133-9f1b-4107-a7b3-78731f66a6ed"),
            "Street10", "10", "City1", "Country1");
        
        public static IEnumerable<Address> GetSeeded()
        {
            return typeof(Addresses)
                .GetFields()
                .Select(field => field.GetValue(null)).Cast<Address>();
        }
    }
}