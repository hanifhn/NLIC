using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using System.Linq;
using NL.IC.Generator.Core.NLP;
using NL.IC.Generator.Core.SemanticAnalysis;

namespace NL.IC.Generator.Core.Extensions
{
    internal static class MarkdownDocumentExtensions
    {
        public static HeaderBlock[] Headers(this MarkdownDocument document)
        {
            return document.Blocks.Where(block => block is HeaderBlock headerBlock).OfType<HeaderBlock>().ToArray();
        }

        public static HeaderBlock[] Headers(this MarkdownDocument document, int level)
        {
            return document.Blocks.Where(block => block is HeaderBlock headerBlock
                                                  && headerBlock.HeaderLevel == level).OfType<HeaderBlock>().ToArray();
        }

        public static SemanticGraph SemanticGraph(this MarkdownDocument document)
        {
            var semanticGraph = new SemanticGraph();

            foreach (var header in document.Headers(1))
            {
                var semanticCluster = new SemanticCluster()
                {
                    Parent = null,
                    SemanticKey = header.Text(),
                    Paragraph = new Paragraph()
                    {
                        OriginalText = header.Pragraph(document)?.ToString()
                    }
                };

                BuildSemanticClusters(ref semanticCluster, document, header.SubHeaders(document));
                semanticGraph.SemanticClusters.Add(semanticCluster);
            }

            return semanticGraph;
        }

        private static void BuildSemanticClusters(ref SemanticCluster semanticCluster, MarkdownDocument document, HeaderBlock[] headers)
        {
            foreach (var header in headers)
            {
                var subSemanticCluster = new SemanticCluster()
                {
                    Parent = semanticCluster,
                    SemanticKey = header.Text(),
                    Paragraph = new Paragraph()
                    {
                        OriginalText = header.Pragraph(document)?.ToString()
                    }
                };
                BuildSemanticClusters(ref subSemanticCluster, document, header.SubHeaders(document));
                semanticCluster.SemanticClusters.Add(subSemanticCluster);
            }
        }
    }
}
