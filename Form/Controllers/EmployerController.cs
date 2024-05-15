using AutoMapper;
using Form.Data;
using Form.DTO;
using Form.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Form.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerController : ControllerBase
    {
        #region Private Variables.
        private readonly IMapper _mapper;
        private readonly CosmosDBContext _context;
        private readonly ILogger<EmployerController> _logger;
        #endregion

        #region Constructors.
        public EmployerController(IMapper mapper, CosmosDBContext context, ILogger<EmployerController> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }
        #endregion

        #region API calls.
        // POST api/<EmployerController>
        /// <summary>
        /// API call for storing employer form.
        /// </summary>
        /// <param name="questionairDTO">QuestionaireDTO</param>
        /// <returns>Whether form is stored or not.</returns>
        [HttpPost]
        public async Task<ActionResult<bool>> Post([FromBody] QuestionaireDTO questionairDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var question = _mapper.Map<QuestionaireModel>(questionairDTO);
                    var isCreated = await _context.CreateEmployerFormAsync(question);
                    return new OkObjectResult(isCreated);
                }
                else
                {
                    return BadRequest(questionairDTO);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Edit employer form
        /// </summary>
        /// <param name="id">employer form id</param>
        /// <param name="updatedQuestion">updatedQuestion form</param>
        /// <returns>status code</returns>
        // PUT api/<EmployerController>/2b5e65ac-d53b-41d4-9934-fba11a8e01b9
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] QuestionaireDTO updatedQuestion)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var question = _mapper.Map<QuestionaireModel>(updatedQuestion);
                    var statusCode = await _context.EditEmployerFormAsync(id, question);
                    return StatusCode(((int)statusCode));
                }
                else
                {
                    return BadRequest(updatedQuestion);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// API call to delete employer form.
        /// </summary>
        /// <param name="id">employer form ID.</param>
        /// <returns>returns status code.</returns>
        // DELETE api/<EmployerController>/2b5e65ac-d53b-41d4-9934-fba11a8e01b9
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                return Ok((int)await _context.DeleteQuestionaireForm(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion
    }
}
