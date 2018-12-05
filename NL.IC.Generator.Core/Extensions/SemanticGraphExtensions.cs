using System.Collections.Generic;
using NL.IC.Generator.Core.SemanticAnalysis;

namespace NL.IC.Generator.Core.Extensions
{
    internal static class SemanticGraphExtensions
    {
        public static SemanticCluster FindCluster(this SemanticGraph semanticGraph, string name)
        {
            return FindCluster(semanticGraph.SemanticClusters, name);
        }

        private static SemanticCluster FindCluster(IEnumerable<SemanticCluster> semanticClusters, string name)
        {
            SemanticCluster match = null;

            foreach (var semanticClaster in semanticClusters)
            {
                match = semanticClaster.Find(name) 
                    ? semanticClaster
                    : FindCluster(semanticClaster.SemanticClusters, name);

                if (match != null)
                {
                    break;
                }
            }

            return match;
        }

        public static SemanticNode FindNode(this SemanticGraph semanticGraph, string name)
        {
            return FindNode(semanticGraph.SemanticClusters, name);
        }

        private static SemanticNode FindNode(IEnumerable<SemanticCluster> semanticClusters, string name)
        {
            if (semanticClusters == null)
            {
                return null;
            }

            SemanticNode match = null;

            foreach (var semanticClaster in semanticClusters)
            {
                match = FindNode(semanticClaster.SemanticNodes, name);
                if (match != null)
                {
                    break;
                }

                match = FindNode(semanticClaster.SemanticClusters, name);
                if (match != null)
                {
                    break;
                }
            }

            return match;
        }

        private static SemanticNode FindNode(IEnumerable<SemanticNode> semanticNodes, string name)
        {
            if (semanticNodes == null)
            {
                return null;
            }

            SemanticNode match = null;

            foreach (var semanticNode in semanticNodes)
            {
                match = semanticNode.Find(name)
                    ? semanticNode
                    : FindNode(semanticNode.SemanticNodes, name);

                if (match != null)
                {
                    break;
                }
            }

            return match;
        }

        public static SemanticCluster Match(this SemanticGraph semanticGraph, string semanticKey)
        {
            return Match(semanticGraph.SemanticClusters, semanticKey);
        }

        private static SemanticCluster Match(IEnumerable<SemanticCluster> semanticClusters, string semanticKey)
        {
            SemanticCluster match = null;

            foreach (var semanticClaster in semanticClusters)
            {
                match = semanticClaster.Match(semanticKey)
                    ? semanticClaster
                    : Match(semanticClaster.SemanticClusters, semanticKey);

                if (match != null)
                {
                    break;
                }
            }

            return match;
        }
    }
}
