using System;

namespace JSONPatchTutorial.Domain
{
    public class Room
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public decimal Area { get; set; }
    }
}