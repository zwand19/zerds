using System.Collections.Generic;
using System.Xml.Serialization;

namespace Zerds.Data
{
    [XmlType(AnonymousType = true)]
    public class SaveGameCollection
    {
        [XmlArray(ElementName = "Players")]
        [XmlArrayItem(ElementName = "SavedPlayer")]
        public List<SavedPlayer> Players { get; set; }
    }
}
