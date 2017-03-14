using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Zerds.GameObjects;

namespace Zerds.Data
{
    [XmlType(AnonymousType = true)]
    public class SavedPlayer
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [XmlArray(ElementName = "Inventory")]
        [XmlArrayItem(ElementName = "SavedItem")]
        public List<SavedItem> Inventory { get; set; }

        public SavedPlayer()
        {
            
        }

        public SavedPlayer(Player player)
        {
            Name = player.Name;
            Inventory = player.Items.Select(i => new SavedItem(i)).ToList();
        }
    }
}
