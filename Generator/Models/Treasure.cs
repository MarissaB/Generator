using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Generator.Models
{
    public class Treasure
    {
        public int TreasureId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Display(Name = "")]
        public string? Image { get; set; }
        [DefaultValue(Rarity.Common)]
        public Rarity Rarity { get; set; }
        [DefaultValue(Category.Misc)]
        public Category Category { get; set; }
        [DefaultValue(Size.Medium)]
        public Size Size { get; set; }
    }

    public enum Rarity
    {
        Common = 1,
        Uncommon = 2, 
        Rare = 3,
        Epic = 4,
        Legendary = 5
    }

    public enum Category
    {
        Armor = 1, 
        Book = 2,
        Potion = 3,
        Ring = 4, Rod = 5, 
        Scroll = 6,
        Staff = 7,
        Wand = 8,
        Weapon = 9,
        Misc = 10,
        Tool = 11, Material = 12, Food = 13, Clothing = 14, Poison = 15, Gem = 16
    }

    public enum Size
    {
        Small = 1,
        Medium = 2,
        Large = 3
    }
}