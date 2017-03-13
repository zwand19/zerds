using System.Collections.Generic;
using System.Linq;
using Zerds.GameObjects;

namespace Zerds.Data
{
    public class SaveGameData
    {
        public string Name { get; set; }
        public List<SaveGameItem> Inventory { get; set; }

        public SaveGameData(Player player, string name)
        {
            Name = name;
            Inventory = player.Items.Select(i => new SaveGameItem(i)).ToList();
        }
    }
}
