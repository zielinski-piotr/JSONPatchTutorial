using System;

namespace JSONPatchTutorial.Contract.Responses
{
    public static class Address
    {
        public class Response
        {
            public Guid Id { get; set; }
            public string Street { get; set; }
            public string HouseNumber { get; set; }
            public string FlatNumber { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
        }
    }
}