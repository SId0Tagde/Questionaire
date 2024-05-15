using Form.DTO;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Form.Model
{
    /// <summary>
    /// Domain class which represents candidate response to the form.
    /// </summary>
    public class Response
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = null!;
        public string FormId { get; set; } = null!;
        public string Title { get; set; } = null!;
        /// <summary>
        /// list of questions and answers submitted by candidate.
        /// </summary>
        public List<QuestionDTO> questionDTOs { get; set; } = null!;
    }
}
