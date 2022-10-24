﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Contracts.Fra
{
    public interface IWordForm
    {
        long? Id { get; }
        abstract POS POS { get; }
        string Lemma { get; }
        string GraphicalForm { get; }
    }

    public abstract class WordForm2 : IWordForm
    {
        public long? Id { get; set; }

        public abstract POS POS { get; }
        public string Lemma { get; set; }
        public string GraphicalForm { get; set; }

        //public int GlaffEntryCount { get; set; }
        //public int PronunciationCount { get; set; }
    }

    public class NounForm : WordForm2
    {
        public override POS POS => POS.Noun;
        public Gender? Gender { get; set; }
        public Number? Number { get; set; }
    }

}
