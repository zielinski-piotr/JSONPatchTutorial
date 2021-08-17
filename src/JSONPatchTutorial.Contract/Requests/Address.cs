namespace JSONPatchTutorial.Contract.Requests
{
    public static class Address
    {
        public class Patch
        {
            public string Street { get; set; }
            public string HouseNumber { get; set; }
            public string FlatNumber { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
        }
    }
}