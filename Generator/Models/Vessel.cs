namespace Generator.Models
{
    public class Vessel
    {
        public int VesselId { get; set; }
        public string? Name { get; set; }
        public int CreatureCapacity { get; set; }
        public int TreasureCapacity { get; set; }
        public string? Image { get; set; }
    }
}
