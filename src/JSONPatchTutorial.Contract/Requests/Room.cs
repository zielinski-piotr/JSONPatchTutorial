namespace JSONPatchTutorial.Contract.Requests
{
    public static class Room
    {
        public class Patch
        {
            public string Name { get; set; }
            public string Color { get; set; }
            public decimal Area { get; set; }
        }
    }
}