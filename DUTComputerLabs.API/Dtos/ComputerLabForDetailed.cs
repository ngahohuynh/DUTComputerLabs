namespace DUTComputerLabs.API.Dtos
{
    public class ComputerLabForDetailed
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Condition { get; set; }

        public int Computers { get; set; }

        public int DamagedComputers { get; set; }

        public int Aircons { get; set; }

        public string Owner { get; set; }

        public string OwnerPhoneNumber { get; set; }

        public string OwnerEmail { get; set; }
    }
}