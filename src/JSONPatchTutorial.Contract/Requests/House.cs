using System.Collections.Generic;

namespace JSONPatchTutorial.Contract.Requests
{
    public static class House
    {
        public class Patch
        {
            public string Name { get; set; }
            public string Color { get; set; }
            public decimal Area { get; set; }
            public Address.Patch Address { get; set; }
            public IEnumerable<Room.Patch> Rooms { get; set; }
        }
    }
}