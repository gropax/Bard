using Bard.Storage.Neo4j.Fra;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bard.Fra.Web.Controllers
{
    [ApiController]
    [Route("word-forms")]
    public class WordFormsController : Controller
    {
        private GraphStorage _graphStorage;
        public WordFormsController(GraphStorage graphStorage)
        {
            _graphStorage = graphStorage;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q, int limit = 10)
        {
            var results = await _graphStorage.SearchWordForms(q, limit);
            return Ok(results);
        }
    }
}
