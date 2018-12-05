using System.Collections.Generic;
using System.Xml.Serialization;
using NL.IC.Generator.Core.NLP;

namespace NL.IC.Generator.Core.SemanticAnalysis
{
    public class SemanticCluster : SemanticNode
    {
        public List<SemanticCluster> SemanticClusters { get; set; }
            = new List<SemanticCluster>();

        public Paragraph Paragraph { get; set; }
    }
}