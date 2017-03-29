namespace Zerds.Items
{
    public class WandItem : Item
    {
        public WandItem(ItemRarities rarity) : base(rarity, ItemTypes.Wand, "wand")
        {
            Name = $"{Rarity} Wand";
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
            return $"{Rarity} Wand";
        }
    }
}
