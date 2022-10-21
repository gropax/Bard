using Bard.Fra.Analysis.Phonology;
using Bard.Fra.Analysis.Glaff;
using Bard.Storage.Neo4j;
using Bard.Storage.Neo4j.Fra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis
{
    public interface IAnalysisModule<T>
    {
        public bool Analyze(AnalysisResult<T> result);
    }

    public class AnalysisPipeline<T>
    {
        private IAnalysisModule<T>[] _modules;

        public AnalysisPipeline(params IAnalysisModule<T>[] modules)
        {
            _modules = modules;
        }

        public AnalysisResult<T> Analyze(T item)
        {
            var result = new AnalysisResult<T>(item);

            foreach (var module in _modules)
            {
                bool abort = false;
                try
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    abort = module.Analyze(result);

                    stopwatch.Stop();
                    result.AddDuration(module.GetType(), stopwatch.ElapsedMilliseconds);
                }
                catch (Exception e)
                {
                    result.AddError(new AnalysisError(module.GetType(), e));
                }

                // Stop further analysis if word is invalid
                if (abort)
                {
                    result.IsAborted = true;
                    break;
                }
            }

            return result;
        }
    }

    public class AnalysisResult<T>
    {
        public T Result { get; }
        public List<AnalysisError> Errors { get; }
        public List<AnalysisWarning> Warnings { get; }
        public Dictionary<Type, long> Durations { get; } = new Dictionary<Type, long>();
        public bool IsAborted { get; set; } = true;

        public AnalysisResult(T result)
        {
            Result = result;
        }

        public void AddError(AnalysisError error)
        {
            Errors.Add(error);
        }

        public void AddWarning(AnalysisWarning warning)
        {
            Warnings.Add(warning);
        }

        public void AddDuration(Type module, long ticks)
        {
            Durations.Add(module, ticks);
        }
    }

    public class AnalysisError
    {
        public Type AnalysisModule { get; }
        public string Message { get; }
        public Exception Exception { get; }

        public AnalysisError(Type analysisModule, string message)
        {
            AnalysisModule = analysisModule;
            Message = message;
        }

        private const string DEFAULT_MESSAGE = "An unexpected exception occured.";

        public AnalysisError(Type analysisModule, Exception exception)
        {
            AnalysisModule = analysisModule;
            Message = DEFAULT_MESSAGE;
            Exception = exception;
        }
    }

    public class AnalysisWarning
    {
        public Type AnalysisModule { get; }
        public string Message { get; }

        public AnalysisWarning(Type analysisModule, string message)
        {
            AnalysisModule = analysisModule;
            Message = message;
        }
    }
}
