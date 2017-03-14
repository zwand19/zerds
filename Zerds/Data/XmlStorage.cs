using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.System;
using Zerds.GameObjects;
using Zerds.Items;

namespace Zerds.Data
{
    public static class XmlStorage
    {
        private const string FileName = "abc";
        public static SaveGameCollection SavedData { get; set; }

        public static void Initialize()
        {
            Task.Run(async () =>
            {
                SavedData = await Get();
            });
        }

        public static async Task<bool> SavePlayer(Player player)
        {
            try
            {
                if (SavedData == null)
                    SavedData = new SaveGameCollection {Players = new List<SavedPlayer>()};
                SavedData.Players = SavedData.Players.Where(s => s.Name != player.Name).ToList();
                SavedData.Players.Add(new SavedPlayer(player));
                await Save();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static List<Item> GetPlayerInventory(string name)
        {
            var data = SavedData?.Players.FirstOrDefault(d => d.Name == name);
            return data?.Inventory.Select(Item.FromSaveData).ToList();
        }

        private static async Task<SaveGameCollection> Get()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(SaveGameCollection));

                var storageFolder = ApplicationData.Current.LocalFolder;

                var file = await storageFolder.GetFileAsync(FileName);
                var dataReader = await FileIO.ReadTextAsync(file);

                using (TextReader reader = new StringReader(dataReader))
                {
                    var savedDataObj = serializer.Deserialize(reader);
                    var data = (SaveGameCollection) savedDataObj;
                    return data;
                }
            }
            catch (Exception e)
            {
                return new SaveGameCollection {Players = new List<SavedPlayer>()};
            }
        }

        private static async Task Save()
        {
            // Create sample file; replace if exists.
            var storageFolder = ApplicationData.Current.LocalFolder;

            var file = await storageFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            var xmlSerializer = new XmlSerializer(SavedData.GetType());

            using (var textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, SavedData);
                var str = textWriter.ToString();

                await FileIO.WriteTextAsync(file, str);
            }
        }
    }
}
