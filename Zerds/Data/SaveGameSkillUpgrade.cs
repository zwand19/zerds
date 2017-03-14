using System.Xml.Serialization;

namespace Zerds.Data
{
    [XmlType(AnonymousType = true)]
    public class SaveGameSkillUpgrade
    {
        [XmlElement(ElementName = "Type")]
        public int Type { get; set; }

        [XmlElement(ElementName = "UpgradeAmount")]
        public int UpgradeAmount { get; set; }
    }
}
