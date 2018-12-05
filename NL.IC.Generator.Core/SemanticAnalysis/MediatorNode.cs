using System.Xml;
using System.Xml.Serialization;

namespace NL.IC.Generator.Core.SemanticAnalysis
{
    public class MediatorNode
    {
        [XmlAttribute("MediatorNode")]
        public string Name { get; set; }

        [XmlAttribute("MediatorNodeType")]
        public MediatorNodeType Type { get; set; }

        [XmlAttribute]
        public string MediatorNodeParent { get; set; }

        [XmlAttribute]
        public bool NodeNameIsNodeValue { get; set; }

        [XmlIgnore]
        public XmlNode XmlNode { get; set; }
    }
}
