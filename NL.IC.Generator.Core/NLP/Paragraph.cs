using System.Collections.Generic;
using System.Xml.Serialization;

namespace NL.IC.Generator.Core.NLP
{
    public class Paragraph
    {
        public string OriginalText { get; set; }

        [XmlIgnore]
        public IEnumerable<Sentence> Sentences { get; set; }
    }
}
