using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.IC.Generator.Core.NLP
{
    public class Sentence
    {
        public SentenceType SentenceType { get; set; }

        public bool GramaticallyWrong { get; set; }

        public bool Understandabl { get; set; }

        public string Text { get; set; }

        public List<string> Clauses { get; set; }
    }
}
