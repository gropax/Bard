using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Contracts.Fra
{
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

}
