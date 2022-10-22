using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Contracts.Fra
{
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
