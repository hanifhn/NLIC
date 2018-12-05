using System;
using System.Linq;
using System.Xml;
using NL.IC.Generator.Core.SemanticAnalysis;

namespace NL.IC.Generator.Core.Extensions
{
    internal static class SemanticClusterExtensions
    {
        public static SemanticCluster ChildClusterWithoutSemanticKey(this SemanticCluster semanticCluster)
        {
            return semanticCluster.SemanticClusters.FirstOrDefault(
                cluster => string.IsNullOrWhiteSpace(cluster.SemanticKey));
        }

        public static SemanticCluster ChildClusterWithoutMediatorParent(this SemanticCluster semanticCluster)
        {
            return semanticCluster.SemanticClusters.FirstOrDefault(
                cluster => string.IsNullOrWhiteSpace(cluster.MediatorNodeParent));
        }

        public static XmlNode XmlNode(this SemanticCluster semanticCluster, SemanticGraph semanticGraph, XmlDocument xmlDoc)
        {
            if (semanticCluster.XmlGenerated())
            {
                return semanticCluster.XmlNode;
            }

            var nodeName = semanticCluster.NodeNameIsNodeValue
                ? semanticCluster.Value
                : semanticCluster.Name;

            if (semanticCluster.Type == MediatorNodeType.attribute
                || string.IsNullOrWhiteSpace(nodeName))
            {
                return null;
            }

            semanticCluster.XmlNode = xmlDoc.CreateElement(nodeName);

            foreach (var childClusterNode in semanticCluster.SemanticClusters)
            {
                if (childClusterNode.Type == MediatorNodeType.attribute)
                {
                    throw new Exception($"The cluster {semanticCluster.SemanticKey} - {semanticCluster.Name} type cannot be attribute.");
                }

                if (childClusterNode.HasDifferentParent())
                {
                    childClusterNode.SetRealParent(semanticGraph, xmlDoc);
                }
                else
                {
                    var newChildNode = childClusterNode.XmlNode(semanticGraph, xmlDoc);
                    if (newChildNode != null)
                    {
                        semanticCluster.XmlNode.AppendChild(newChildNode);
                    }
                }
            }

            foreach (var childSemanticNode in semanticCluster.SemanticNodes)
            {
                if (childSemanticNode.Type == MediatorNodeType.attribute)
                {
                    semanticCluster.XmlNode.Attributes?.Append(childSemanticNode.XmlAttribute(xmlDoc));
                }
                else if (childSemanticNode.HasDifferentParent())
                {
                    childSemanticNode.SetRealParent(semanticGraph, xmlDoc);
                }
                else
                {
                    var newChildNode = childSemanticNode.XmlNode(xmlDoc);
                    if (newChildNode != null)
                    {
                        semanticCluster.XmlNode.AppendChild(newChildNode);
                    }
                }
            }

            return semanticCluster.XmlNode;
        }

        public static void SetRealParent(this SemanticCluster semanticCluster,
            SemanticGraph semanticGraph,
            XmlDocument xmlDoc)
        {
            var realParentNode = semanticGraph.FindCluster(semanticCluster.MediatorNodeParent)
                                 ??
                                 semanticGraph.FindNode(semanticCluster.MediatorNodeParent);

            var mediatorNodeParent = realParentNode.XmlNode ??
                                     (realParentNode is SemanticCluster cluster
                                         ? cluster.XmlNode(semanticGraph, xmlDoc)
                                         : realParentNode.XmlNode(xmlDoc));

            mediatorNodeParent.AppendChild(semanticCluster.XmlNode ?? semanticCluster.XmlNode(semanticGraph, xmlDoc));
        }
    }
}
