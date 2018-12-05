using Microsoft.Toolkit.Parsers.Markdown;
using System.Collections.Generic;
using NL.IC.Generator.Core.Extensions;
using NL.IC.Generator.Core.Mediating;

namespace NL.IC.Generator.Core.SemanticAnalysis
{
    public class SemanticAnalyser
    {
        public SemanticGraph Analyse(MarkdownDocument document,
            IntermediateContractDefinition intermediateContractDefinition)
        {
            SemanticGraph semanticGraph = document.SemanticGraph();
            semanticGraph.Name = intermediateContractDefinition.Name;
            semanticGraph.Type = intermediateContractDefinition.Type;

            EnrichSemanticGraph(semanticGraph, intermediateContractDefinition);

            return semanticGraph;
        }

        private void EnrichSemanticGraph(SemanticGraph semanticGraph,
            IntermediateContractDefinition intermediateContractDefinition)
        {
            EnrichSemanticGraph(semanticGraph.SemanticClusters, intermediateContractDefinition);

            foreach (var intermediateContractCluster in intermediateContractDefinition.SemanticClusters)
            {
                if (semanticGraph.FindCluster(intermediateContractCluster.Name) == null)
                {
                    semanticGraph.SemanticClusters.Add(intermediateContractCluster);
                }
            }
        }

        private void EnrichSemanticGraph(IEnumerable<SemanticCluster> semanticClusters,
            IntermediateContractDefinition intermediateContractDefinition)
        {
            foreach (var semanticCluster in semanticClusters)
            {
                var matchedCluster = intermediateContractDefinition.Match(semanticCluster.SemanticKey);
                if (matchedCluster != null)
                {
                    ProjectCluster(semanticCluster, matchedCluster);
                }
                else if (semanticCluster.Parent != null)
                // Means this cluster should be mapped to child cluster of IntermediateContract with no SemanticKey.
                // This solution is smelly :( there should be a better way.
                {
                    var matchedParentCluster = intermediateContractDefinition.Match(semanticCluster.Parent.SemanticKey);
                    matchedCluster = matchedParentCluster?.ChildClusterWithoutSemanticKey()
                                     ??
                                     matchedParentCluster?.ChildClusterWithoutMediatorParent();
                    if (matchedCluster != null)
                    {
                        ProjectCluster(semanticCluster, matchedCluster);
                    }
                }

                EnrichSemanticGraph(semanticCluster.SemanticClusters, intermediateContractDefinition);
            }
        }

        private static void ProjectCluster(SemanticCluster semanticCluster,
            SemanticCluster matchedCluster)
        {
            semanticCluster.SemanticNodes = matchedCluster.SemanticNodes;
            semanticCluster.MediatorNodeParent = matchedCluster.MediatorNodeParent;
            semanticCluster.Name = matchedCluster.Name;
            semanticCluster.Type = matchedCluster.Type;
            semanticCluster.NodeNameIsNodeValue = matchedCluster.NodeNameIsNodeValue;
        }
    }
}
