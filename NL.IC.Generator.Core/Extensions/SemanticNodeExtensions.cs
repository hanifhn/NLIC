using System;
using System.Xml;
using NL.IC.Generator.Core.SemanticAnalysis;

namespace NL.IC.Generator.Core.Extensions
{
    internal static class SemanticNodeExtensions
    {
        public static bool Find(this SemanticNode semanticNode, string name)
        {
            return !string.IsNullOrWhiteSpace(semanticNode.Name)
                   && semanticNode.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool Match(this SemanticNode semanticNode, string semanticKey)
        {
            return !string.IsNullOrWhiteSpace(semanticNode.SemanticKey)
                   && semanticNode.SemanticKey.Equals(semanticKey, StringComparison.InvariantCultureIgnoreCase);
        }

        public static XmlNode XmlNode(this SemanticNode semanticNode, XmlDocument xmlDoc)
        {
            if (semanticNode.XmlGenerated())
            {
                return semanticNode.XmlNode;
            }

            var nodeName = semanticNode.NodeNameIsNodeValue
                ? semanticNode.Value
                : semanticNode.Name;

            if (semanticNode.Type == MediatorNodeType.attribute
                || string.IsNullOrWhiteSpace(nodeName))
            {
                return null;
            }

            semanticNode.XmlNode = xmlDoc.CreateElement(nodeName);

            foreach (var childSemanticNode in semanticNode.SemanticNodes)
            {
                if (childSemanticNode.Type == MediatorNodeType.attribute)
                {
                    semanticNode.XmlNode.Attributes?.Append(childSemanticNode.XmlAttribute(xmlDoc));
                }
                else
                {
                    semanticNode.XmlNode.AppendChild(childSemanticNode.XmlNode(xmlDoc));
                }
            }

            return semanticNode.XmlNode;
        }

        public static XmlAttribute XmlAttribute(this SemanticNode semanticNode, XmlDocument xmlDoc)
        {
            if (semanticNode.XmlGenerated()
                || semanticNode.Type != MediatorNodeType.attribute
                || string.IsNullOrWhiteSpace(semanticNode.Name))
                return null;

            var xmlAttribute = xmlDoc.CreateAttribute(semanticNode.Name);
            xmlAttribute.Value = semanticNode.Value;

            return xmlAttribute;
        }

        public static void SetRealParent(this SemanticNode semanticNode,
            SemanticGraph semanticGraph,
            XmlDocument xmlDoc)
        {
            var realParentNode = semanticGraph.FindCluster(semanticNode.MediatorNodeParent);

            var mediatorNodeParent = realParentNode.XmlNode ?? realParentNode.XmlNode(semanticGraph, xmlDoc);

            mediatorNodeParent.AppendChild(semanticNode.XmlNode ?? semanticNode.XmlNode(xmlDoc));
        }
    }
}
