using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Generator.Models
{
    public class Encounter
    {
        public int EncounterId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        [DefaultValue(1)]
        public int? Round { get; set; }
        /// <summary>
        /// The index for the Participant with the active turn
        /// </summary>
        public int? TurnIndex { get; set; }
        public ICollection<Participant>? Participants { get; set; }
    }

    public class Participant
    {
        public int ParticipantId { get; set; }
        public int EncounterId { get; set; }
        public string Name { get; set; }
        [Display(Name = "")]
        [DefaultValue("Default")]
        public string? Image { get; set; }
        [Display(Name = "Active")]
        [DefaultValue(true)]
        public bool IsActive { get; set; }
        [DefaultValue(0)]
        public int Initiative { get; set; }
        public int? MaxHealth { get; set; }
        [DefaultValue(0)]
        public int? RemovedHealth { get; set; }
        public int? RemainingHealth
        {
            get
            {
                return MaxHealth - RemovedHealth;
            }
        }
        public int? Armor { get; set; }
        public string? Note { get; set; }
        public Encounter? Encounter { get; set; }
    }
}
