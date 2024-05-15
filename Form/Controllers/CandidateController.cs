using AutoMapper;
using Form.Data;
using Form.DTO;
using Form.Model;
using Microsoft.AspNetCore.Mvc;

namespace Form.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CandidateController : ControllerBase
    {
        #region Private Variables.
        private readonly IMapper _mapper;
        private readonly CosmosDBContext _context;
        private readonly ILogger<CandidateController> _logger;
        #endregion

        #region Constructors.
        public CandidateController(IMapper mapper, CosmosDBContext context, ILogger<CandidateController> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }
        #endregion

        /// <summary>
        /// API call to post candidateform
        /// </summary>
        /// <param name="dto">QuestionAnswerDTO /param>
        /// <returns>returns candidate form which contains list of the question and also answers of it.</returns>
        [HttpPost("PostCandidateForm")]
        public async Task<ActionResult<QuestionAnswerDTO>> PostCandidateForm([FromBody] QuestionAnswerDTO dto)
        {
            try
            {
                var candidateForm = _mapper.Map<Response>(dto);
                var response = await _context.SaveCandidateForm(candidateForm);
                var questionAnswersDTO = _mapper.Map<QuestionAnswerDTO>(response);
                return Ok(questionAnswersDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// API call to get questions based on question type in questionaire form.
        /// </summary>
        /// <param name="id">Questionaire form id</param>
        /// <param name="type">Question type to search for in the Questionaire form.</param>
        /// <returns>List of questions based on questiont type.</returns>
        #region API calls.
        [HttpGet("{id}/{type}")]
        public async Task<ActionResult<List<QuestionDTO>>> GetQuestionsBasedOnQuestionTypeForSpecificForm(string id, string type)
        {
            try
            {
                var listOfQuestionsWithCertainTypeInsideForm = await _context.GetQuestionsBasedOnQuestionTypeForSpecificForm(id, type);
                var applicationLayerQuestionsRepresentations = _mapper.Map<List<QuestionDTO>>(listOfQuestionsWithCertainTypeInsideForm);
                return Ok(applicationLayerQuestionsRepresentations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete Candidate form.
        /// </summary>
        /// <param name="id">Candidate form Id</param>
        /// <returns>returns boolean value indicating whether the form is deleted or not.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteCandidateForm(string id)
        {
            try
            {
                return Ok((int)await _context.DeleteCandidateForm(id));
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
