using System;

namespace JSONPatchTutorial.Domain
{
    public class Address
    {
        public static Address Create(Guid id, string street, string houseNumber, string city, string country,
            string flatNumber = null)
        {
            return new Address
            {
                Id = id == Guid.Empty ? throw new ArgumentException(nameof(id)) : id,
                Street = street ?? throw new ArgumentNullException(nameof(street)),
                HouseNumber = houseNumber ?? throw new ArgumentNullException(nameof(houseNumber)),
                City = city ?? throw new ArgumentNullException(nameof(city)),
                Country = country ?? throw new ArgumentNullException(nameof(country)),
                FlatNumber = flatNumber
            };
        }

        public Guid Id { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string FlatNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}