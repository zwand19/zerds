namespace Zerds.Items
{
    public class HoodItem : Item
    {
        public HoodItem(ItemRarities rarity, double look = -1) : base(rarity, ItemTypes.Hood, "hood")
        {
            look = look < 0 ? Globals.Random.NextDouble() : look;
            if (look < 0.95)
            {
                AnimationFile = "Zerds/bare-head.png";
                Name = $"{Rarity} Headband";
            }
            else
            {
                AnimationFile = "Zerds/red-hood.png";
                Name = $"{Rarity} Red Hood";
            }
        }

        public override string Description1()
        {
            return "";
        }

        public override string Description2()
        {
            return "";
        }

        public override string ToSaveString()
        {
            return "";
        }

        public override void LoadSaveString(string str)
        {

        }

        public override string InformalName()
        {
            return $"{Rarity} Hood";
        }
    }
}
