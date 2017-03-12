using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Zerds.Data
{
    public static class XmlStorage
    {
        public static async Task<bool> SaveData(SaveGameData data)
        {
            // Create sample file; replace if exists.
            var storageFolder = ApplicationData.Current.LocalFolder;
            var sampleFile = await storageFolder.CreateFileAsync("sample.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, "Swift as a shadow");
            return true;
        }

        public static async Task<SaveGameData> GetData()
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var sampleFile = await storageFolder.GetFileAsync("sample.txt");
            var text = await FileIO.ReadTextAsync(sampleFile);
            return new SaveGameData {Name = text};
        }
    }
}
