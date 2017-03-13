using System.Collections.Generic;
using System.Linq;
using Zerds.Items;

namespace Zerds.Data
{
    public class SaveGameItem
    {
        public List<SaveGameAbilityUpgrade> AbilityUpgrades { get; set; }
        public List<SaveGameSkillUpgrade> SkillUpgrades { get; set; }
        public int Rarity { get; set; }
        public int Type { get; set; }

        public SaveGameItem(Item item)
        {
            Rarity = (int)item.Rarity;
            Type = (int)item.Type;
            AbilityUpgrades =
                item.AbilityUpgrades.Select(
                    a => new SaveGameAbilityUpgrade {Amount = a.Amount, Text1 = a.Text1, Text2 = a.Text2, Type = (int) a.Type}).ToList();
            SkillUpgrades =
                item.SkillUpgrades.Select(s => new SaveGameSkillUpgrade {Type = (int) s.Type, UpgradeAmount = s.UpgradeAmount}).ToList();
        }
    }
}