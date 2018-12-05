using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NL.IC.Generator.Core.Extensions;
using NL.IC.Generator.Core.SemanticAnalysis;

namespace NL.IC.Generator.Core.Mediating
{
    public class Mediator
    {
        public XmlDocument Mediate(SemanticGraph semanticGraph)
        {
            var intermediateContract = new XmlDocument();
            var xmlRootNode = semanticGraph.XmlNode(intermediateContract);
            foreach (var semanticCluster in semanticGraph.SemanticClusters)
            {
                if (semanticCluster.HasDifferentParent())
                {
                    semanticCluster.SetRealParent(semanticGraph, intermediateContract);
                }
                else
                {
                    var newChildNode = semanticCluster.XmlNode(semanticGraph, intermediateContract);
                    if (newChildNode != null)
                    {
                        xmlRootNode.AppendChild(newChildNode);
                    }
                }
            }

            intermediateContract.AppendChild(xmlRootNode);

            return intermediateContract;
        }

        private static IEnumerable<SemanticCluster> ClustersWithDifferentParent(SemanticCluster[] semanticClusters)
        {
            var clustersWithDifferentParent =
                new List<SemanticCluster>(semanticClusters.Where(semanticCluster =>
                    !string.IsNullOrWhiteSpace(semanticCluster.MediatorNodeParent)));

            foreach (var semanticCluster in semanticClusters)
            {
                clustersWithDifferentParent.AddRange(ClustersWithDifferentParent(semanticCluster.SemanticClusters.ToArray()));
            }

            return clustersWithDifferentParent;
        }
    }
}
