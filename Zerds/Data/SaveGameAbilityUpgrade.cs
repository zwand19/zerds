using System.Xml.Serialization;

namespace Zerds.Data
{
    [XmlType(AnonymousType = true)]
    public class SaveGameAbilityUpgrade
    {
        [XmlElement(ElementName = "Type")]
        public int Type { get; set; }
        
        [XmlElement(ElementName = "Text1")]
        public string Text1 { get; set; }

        [XmlElement(ElementName = "Text2")]
        public string Text2 { get; set; }

        [XmlElement(ElementName = "Amount")]
        public int Amount { get; set; }
    }
}
