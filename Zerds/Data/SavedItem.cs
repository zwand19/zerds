using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Zerds.Items;

namespace Zerds.Data
{
    [XmlType(AnonymousType = true)]
    public class SavedItem
    {
        [XmlArray(ElementName = "AbilityUpgrades")]
        [XmlArrayItem(ElementName = "SaveGameAbilityUpgrade")]
        public List<SaveGameAbilityUpgrade> AbilityUpgrades { get; set; }

        [XmlArray(ElementName = "SkillUpgrades")]
        [XmlArrayItem(ElementName = "SaveGameSkillUpgrade")]
        public List<SaveGameSkillUpgrade> SkillUpgrades { get; set; }

        [XmlElement(ElementName = "Rarity")]
        public int Rarity { get; set; }

        [XmlElement(ElementName = "Name")]
        public int Type { get; set; }

        public SavedItem()
        {
            
        }

        public SavedItem(Item item)
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