namespace DUTComputerLabs.API.Dtos
{
    public class ComputerLabForList
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Condition { get; set; }

        public int Computers { get; set; }

        public int DamagedComputers { get; set; }

        public int Aircons { get; set; }
    }
}