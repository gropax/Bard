using Bard.Fra.Analysis.Phonology;
using Bard.Fra.Glaff;
using Bard.Storage.Neo4j;
using Bard.Storage.Neo4j.Fra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis
{
    public interface IAnalysisModule
    {
        public void Analyze(WordForm wordForm);
    }

    public class AnalysisPipeline
    {
        private IAnalysisModule[] _modules;

        public AnalysisPipeline(params IAnalysisModule[] modules)
        {
            _modules = modules;
        }

        public WordForm Analyze(GlaffEntry entry)
        {
            var wordForm = new WordForm(entry);

            foreach (var module in _modules)
            {
                module.Analyze(wordForm);

                // Stop further analysis if word is invalid
                if (!wordForm.IsValid)
                    break;
            }

            return wordForm;
        }
    }

    public class ChangeHistory
    {
        public class Change
        {
            public string OperationName { get; }
            public string ResultValue { get; }

            public Change(string operationName, string resultValue)
            {
                OperationName = operationName;
                ResultValue = resultValue;
            }
        }

        public string InitialValue { get; }
        private List<Change> _changes { get; } = new List<Change>();
        public int ChangeCount => _changes.Count;

        public ChangeHistory(string initialValue)
        {
            InitialValue = initialValue;
        }

        public void AddChange(string operationName, string resultValue)
        {
            _changes.Add(new Change(operationName, resultValue));
        }

        public string Format()
        {
            var builder = new StringBuilder();

            builder.Append(InitialValue);
            foreach (var change in _changes)
                builder.Append($" -[{change.OperationName}]-> {change.ResultValue}");

            return builder.ToString();
        }
    }

    public enum AnomalyType
    {
        NoPhoneme,
        Acronym,
        MultiplePronunciations,
        BadSyllabation,
        AlignmentFailed,
        SyllabificationError,
    }

    public interface IAnomaly
    {
        AnomalyType Type { get; }
    }

    public class GenericAnomaly : IAnomaly
    {
        public AnomalyType Type { get; }
        public GenericAnomaly(AnomalyType Type)
        {
            this.Type = Type;
        }
    }
}
