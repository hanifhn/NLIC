using System.Collections.Generic;
using System.Xml.Serialization;

namespace NL.IC.Generator.Core.SemanticAnalysis
{
    public class SemanticNode : MediatorNode
    {
        [XmlIgnore]
        public SemanticCluster Parent { get; set; }

        [XmlAttribute]
        public string SemanticKey { get; set; }

        [XmlAttribute]
        public bool ValueIsIncrimental { get; set; }

        [XmlAttribute]
        public string Value { get; set; }

        public List<SemanticNode> SemanticNodes { get; set; }
    }
}
