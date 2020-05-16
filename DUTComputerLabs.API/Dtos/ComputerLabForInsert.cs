namespace DUTComputerLabs.API.Dtos
{
    public class ComputerLabForInsert
    {
        public string Name { get; set; }

        public string Condition { get; set; }

        public int Computers { get; set; }

        public int DamagedComputers { get; set; }

        public int Aircons { get; set; }

        public int? OwnerId { get; set; }
    }
}