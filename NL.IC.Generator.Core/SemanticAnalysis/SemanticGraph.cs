using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using com.sun.tools.doclets.@internal.toolkit.builders;

namespace NL.IC.Generator.Core.SemanticAnalysis
{
    public class SemanticGraph: MediatorNode
    {
        public List<SemanticCluster> SemanticClusters { get; set; } = new List<SemanticCluster>();
    }
}
