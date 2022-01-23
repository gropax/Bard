using Bard.Storage.Neo4j.Fra;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bard.Fra.Web.Controllers
{
    [ApiController]
    [Route("graph")]
    public class GraphController : Controller
    {
        private GraphStorage _graphStorage;
        public GraphController(GraphStorage graphStorage)
        {
            _graphStorage = graphStorage;
        }

        [HttpGet("word-forms/search")]
        public async Task<IActionResult> SearchWordForms([FromQuery] string q, int limit = 10)
        {
            var results = await _graphStorage.SearchWordForms(q, limit);
            return Ok(results);
        }

        [HttpGet("phon-graph-words/search")]
        public async Task<IActionResult> SearchPhonGraphWords([FromQuery] string q, int limit = 10)
        {
            var results = await _graphStorage.SearchPhonGraphWords(q, limit);
            return Ok(results);
        }
    }
}
