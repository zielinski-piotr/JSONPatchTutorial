using System.Collections.Generic;
using System.ComponentModel.DataAnnotations ;

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
        
        public class Update
        {
            [Required]
            public string Name { get; set; }
            [Required]
            public string Color { get; set; }
            [Required]
            public decimal Area { get; set; }
        }
        
        public class Request
        {
            [Required]
            public string Name { get; set; }
            [Required]
            public string Color { get; set; }
            [Required]
            public decimal Area { get; set; }
        }
    }
}