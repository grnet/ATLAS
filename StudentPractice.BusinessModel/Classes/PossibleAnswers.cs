using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPractice.BusinessModel
{
    public class PossibleAnswer
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

    public class PossibleAnswers : List<PossibleAnswer>
    {
        public PossibleAnswers() : base() { }
        public PossibleAnswers(int capacity) : base(capacity) { }
        public PossibleAnswers(IEnumerable<PossibleAnswer> collection) : base(collection) { }
    }
}
