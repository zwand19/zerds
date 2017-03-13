using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.System;

namespace Zerds.Data
{
    public static class XmlStorage
    {
        public static async Task<bool> SaveData<T>(T data)
        {
            // Create sample file; replace if exists.
            var storageFolder = ApplicationData.Current.LocalFolder;
            var username = await GetUserName();

            var file = await storageFolder.CreateFileAsync(username, CreationCollisionOption.ReplaceExisting);
            var xmlSerializer = new XmlSerializer(data.GetType());

            using (var textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, data);
                var str = textWriter.ToString();

                await FileIO.WriteTextAsync(file, str);
                return true;
            }
        }

        public static async Task<T> GetData<T>()
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var username = await GetUserName();

            var file = await storageFolder.GetFileAsync(username);
            var data = await FileIO.ReadTextAsync(file);

            var serializer = new XmlSerializer(typeof(T));
            
            using (TextReader reader = new StringReader(data))
                return (T)serializer.Deserialize(reader);
        }

        private static async Task<string> GetUserName()
        {
            var users = await User.FindAllAsync();

            var current =
                users.FirstOrDefault(
                    p => p.AuthenticationStatus == UserAuthenticationStatus.LocallyAuthenticated && p.Type == UserType.LocalUser);

            // user may have username
            var data = await current.GetPropertyAsync(KnownUserProperties.AccountName);
            var displayName = (string)data;

            //or may be authinticated using hotmail 
            if (string.IsNullOrEmpty(displayName))
            {
                var a = (string)await current.GetPropertyAsync(KnownUserProperties.FirstName);
                var b = (string)await current.GetPropertyAsync(KnownUserProperties.LastName);
                displayName = string.Format("{0} {1}", a, b);
            }

            return displayName;
        }
    }
}
