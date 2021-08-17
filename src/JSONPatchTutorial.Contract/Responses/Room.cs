using System;

namespace JSONPatchTutorial.Contract.Responses
{
    public static class Room
    {
        public class ListItem
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Color { get; set; }
            public decimal Area { get; set; }
        }
    }
}