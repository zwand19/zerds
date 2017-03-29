using System;

namespace Zerds.Items
{
    public class BootItem : Item
    {
        public int SprintBonus { get; set; }
        public int SprintManaPerSecond { get; set; }
        public int Speed { get; set; }

        public BootItem(ItemRarities rarity) : base(rarity, ItemTypes.Boots, "boots")
        {
            AnimationFile = "Zerds/bare-feet.png";
            Name = $"{Rarity} Boots";
            
            SprintBonus = 175;
            SprintManaPerSecond = 15;
            Speed = 200;
            var ran = Globals.Random.NextDouble();
            if (ran < 0.2)
            {
                SprintBonus += 25;
                SprintManaPerSecond += 5;
                Speed += 5;
            }
            else if (ran < 0.4)
            {
                SprintBonus += 50;
                SprintManaPerSecond += 7;
                Speed += 5;
            }
            else if (ran < 0.48)
            {
                SprintBonus -= 10;
                SprintManaPerSecond -= 5;
                Speed += 15;
            }
            else if (ran < 0.48)
            {
                SprintBonus -= 10;
                SprintManaPerSecond -= 5;
                Speed -= 10;
            }
            else if (ran < 0.7)
            {
                SprintBonus -= 50;
                SprintManaPerSecond -= 7;
                Speed -= 5;
            }
            else if (ran < 0.8)
            {
                SprintBonus -= 65;
                SprintManaPerSecond -= 6;
            }
            else if (ran < 0.85)
            {
                SprintBonus -= 80;
                SprintManaPerSecond -= 10;
                Speed += 5;
            }
            else if (ran < 0.9)
            {
                SprintBonus += 100;
                SprintManaPerSecond += 11;
                Speed -= 50;
            }
            ran = Globals.Random.NextDouble();
            if (ran < 0.15)
                SprintManaPerSecond--;
            else if (ran < 0.3)
                SprintManaPerSecond++;

            Speed += 5 * (int)rarity;
        }

        public override string Description1()
        {
            return $"+{Speed} Movement Speed";
        }

        public override string Description2()
        {
            return $"+{SprintBonus} Sprint Speed (-{SprintManaPerSecond} mana per sec)";
        }

        public override string ToSaveString()
        {
            return $"{Speed}-{SprintManaPerSecond}-{SprintBonus}";
        }

        public override void LoadSaveString(string str)
        {
            Speed = int.Parse(str.Substring(0, str.IndexOf("-", StringComparison.Ordinal)));
            var start = str.IndexOf("-", StringComparison.Ordinal) + 1;
            SprintManaPerSecond = int.Parse(str.Substring(start, str.LastIndexOf("-", StringComparison.Ordinal) - start));
            SprintBonus = int.Parse(str.Substring(str.LastIndexOf("-", StringComparison.Ordinal) + 1));
        }

        public override string InformalName()
        {
            return $"{Rarity} Boots";
        }
    }
}
