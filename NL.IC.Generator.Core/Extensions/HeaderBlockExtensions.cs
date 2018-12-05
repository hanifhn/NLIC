using Microsoft.Toolkit.Extensions;
using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using System.Collections.Generic;
using System.Linq;

namespace NL.IC.Generator.Core.Extensions
{
    public static class HeaderBlockExtensions
    {
        public static string Text(this HeaderBlock header)
        {
            return header.ToSafeString().Trim(' ', '*');
        }

        public static ParagraphBlock Pragraph(this HeaderBlock header, MarkdownDocument document)
        {
            int paragraphIndex = document.Blocks.IndexOf(header) + 1;
            return document.Blocks.Count - 1 > paragraphIndex &&
                document.Blocks[paragraphIndex].Type == MarkdownBlockType.Paragraph
                    ? document.Blocks[paragraphIndex] as ParagraphBlock
                    : null;
        }

        public static HeaderBlock[] SubHeaders(this HeaderBlock header, MarkdownDocument document)
        {
            List<HeaderBlock> subHeaders = new List<HeaderBlock>();

            var headers = document.Headers().ToList();
            int headerIndex = headers.IndexOf(header);
            var headersAfterHeader = headers.GetRange(headerIndex + 1, headers.Count - (headerIndex + 1));

            foreach (var nextHeader in headersAfterHeader)
            {
                if (nextHeader.HeaderLevel <= header.HeaderLevel)
                {
                    break;
                }
                else if (nextHeader.HeaderLevel == header.HeaderLevel + 1)
                {
                    subHeaders.Add(nextHeader);
                }
            }

            return subHeaders.ToArray();
        }
    }
}
