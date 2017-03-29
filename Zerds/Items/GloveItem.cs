namespace Zerds.Items
{
    public class GloveItem : Item
    {
        public int SpellDamage { get; set; }

        public GloveItem(ItemRarities rarity, double look = -1) : base(rarity, ItemTypes.Glove, "hands")
        {
            look = look < 0 ? Globals.Random.NextDouble() : look;
            if (look < 0.95)
            {
                AnimationFile = "Zerds/bare-hands.png";
                Name = $"{Rarity} Wraps";
            }
            else
            {
                AnimationFile = "Zerds/red-hands.png";
                Name = $"{Rarity} Devil Hands";
            }

            SpellDamage = (int) Helpers.RandomInRange(7, 11) + (int) rarity;
        }

        public override string Description1()
        {
            return $"+{SpellDamage}% Spell Damage";
        }

        public override string Description2()
        {
            return "";
        }

        public override string ToSaveString()
        {
            return SpellDamage.ToString();
        }

        public override void LoadSaveString(string str)
        {
            SpellDamage = int.Parse(str);
        }

        public override string InformalName()
        {
            return $"{Rarity} Gloves";
        }
    }
}
