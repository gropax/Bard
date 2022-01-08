using Bard.Storage.Neo4j;
using Bard.Storage.Neo4j.Fra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bard.Fra.Analysis
{
    public class MultiNodeBuilder
    {
        public MultiNode Build(WordForm wordForm)
        {
            var wordFormNode = new WordFormNodeType(wordForm.GlaffEntry.GraphicalForm);

            return new MultiNode(wordFormNode);
        }
    }
}
