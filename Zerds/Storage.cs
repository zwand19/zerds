using System.Collections.Generic;
using Zerds.Entities;
using Zerds.Items;
using Windows.Storage;

namespace Zerds
{
    public static class Storage
    {
        public static void SaveZerd(Zerd zerd)
        {

        }
        public static List<Zerd> LoadZerds()
        {
            return new List<Zerd>();
        }
    }

    public class SerializableZerd
    {
        public List<SerializableItem> Inventory { get; set; }

        public SerializableZerd(Zerd zerd)
        {
            
        }
    }
    
    public class SerializableItem
    {
        public SerializableItem(Item item)
        {
            
        }
    }
}
