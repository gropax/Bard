using Bard.Fra.Web.Contracts;
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

        [HttpPost("word-forms/pronunciation")]
        public async Task<IActionResult> GetPronunciation([FromBody] GetPronunciationDto queryDto)
        {
            var results = await _graphStorage.GetPronunciation(queryDto.GraphicalForm);
            return Ok(results);
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

        [HttpGet("final-rhymes")]
        public async Task<IActionResult> GetFinalRhymingWords(
            string graphemes,
            string phonemes,
            string filter = null,
            string sortDir = null,
            int page = 0,
            int pageSize = 10)
        {
            var results = await _graphStorage.GetFinalRhymingWords(graphemes, phonemes, sortDir, page, pageSize);
            return Ok(results);
        }
    }
}
