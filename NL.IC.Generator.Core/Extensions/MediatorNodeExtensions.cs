using System.Xml;
using NL.IC.Generator.Core.SemanticAnalysis;

namespace NL.IC.Generator.Core.Extensions
{
    internal static class MediatorNodeExtensions
    {
        public static bool XmlGenerated(this MediatorNode mediatorNode)
        {
            return mediatorNode.XmlNode != null;
        }

        public static XmlNode XmlNode(this MediatorNode mediatorNode, XmlDocument xmlDoc)
        {
            if (mediatorNode.XmlGenerated()
                || mediatorNode.Type == MediatorNodeType.attribute)
            {
                return null;
            }

            mediatorNode.XmlNode = (XmlNode) xmlDoc.CreateElement(mediatorNode.Name);
            return mediatorNode.XmlNode;
        }

        public static bool HasDifferentParent(this MediatorNode mediatorNode)
        {
            return !string.IsNullOrWhiteSpace(mediatorNode.MediatorNodeParent);
        }
    }
}
