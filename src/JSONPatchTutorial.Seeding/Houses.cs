using System;
using System.Collections.Generic;
using System.Linq;
using JSONPatchTutorial.Domain;

namespace JSONPatchTutorial.Seeding
{
    public static class Houses
    {
        public static readonly House House1 = House.Create(
            Guid.Parse("7B4283FE-D046-4766-8015-AD4E50DF4F67"),
            "First House",
            "Red",
            25m,
            Addresses.Address1);

        public static readonly House House2 = House.Create(
            Guid.Parse("2D6DEA12-F724-45AD-ADFB-C04703A41805"),
            "Second House",
            "Red",
            25m,
            Addresses.Address2,
            new List<Room>
            {
                new Room()
                {
                    Id = Guid.Parse("B6A448F4-4B77-4591-9530-D145B770A2E6"), Area = 11, Color = "Green",
                    Name = "Restroom"
                },
                new Room()
                {
                    Id = Guid.Parse("0A1826A1-7846-4F8A-A44E-A0EFB93C25BE"), Area = 20, Color = "Pink",
                    Name = "Kids Room"
                }
            });

        public static readonly House House3 = House.Create(
            Guid.Parse("37F05632-8C35-4AFE-A472-F3A57924D0B8"),
            "Third House",
            "Red",
            25m,
            Addresses.Address3,
            new List<Room>
            {
                new Room()
                {
                    Id = Guid.Parse("DB756D1D-7175-4FA6-8CA5-C70EBBCC099F"), Area = 11, Color = "Green",
                    Name = "Restroom"
                }
            });

        public static readonly House House4 = House.Create(
            Guid.Parse("4C6113DE-2384-422C-986C-71FD5F23D7EF"), "Fourth House",
            "Red",
            25m,
            Addresses.Address4);

        public static readonly House House5 = House.Create(
            Guid.Parse("BE96327B-EC6A-477A-A4DB-476B49767E23"),
            "Fifth House",
            "Red",
            25m,
            Addresses.Address5);

        public static readonly House House6 = House.Create(
            Guid.Parse("D4259385-813A-44D3-A114-5A898833CF2F"),
            "Sixth House",
            "Red",
            25m,
            Addresses.Address6);

        public static readonly House House7 = House.Create(
            Guid.Parse("1C261F07-E875-4B06-9D3D-03661F367352"),
            "Seventh House",
            "Red",
            25m,
            Addresses.Address7);

        public static readonly House House8 = House.Create(
            Guid.Parse("C83ECBAD-1193-48C2-B3A8-0E268148379F"),
            "Eighth House",
            "Red",
            25m,
            Addresses.Address8);

        public static readonly House House9 = House.Create(
            Guid.Parse("21C5A65D-5856-47C5-8CD3-04075C6271C1"),
            "Ninth House",
            "Red",
            25m,
            Addresses.Address9);

        public static readonly House House10 = House.Create(
            Guid.Parse("AFC0D0FD-9321-4757-8906-D33B99DEC91C"),
            "Tenth House",
            "Red",
            25m,
            Addresses.Address10);

        public static readonly House House11 = House.Create(
            Guid.Parse("D93291CC-2434-48C0-BF50-FF65505C1AA7"),
            "Tenth House",
            "Red",
            25m,
            null);

        public static IEnumerable<House> GetSeededHouses()
        {
            return typeof(Houses)
                .GetFields()
                .Select(field => field.GetValue(null)).Cast<House>();
        }
    }
}