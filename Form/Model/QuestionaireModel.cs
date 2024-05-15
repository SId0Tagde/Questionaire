using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Form.Model
{
    /// <summary>
    /// Domain class which represents employer form.
    /// </summary>
    public class QuestionaireModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = null!;


        public string Title { get; set; } = null!;
        /// <summary>
        /// list of questions but don't specify answers for the question.
        /// </summary>
        public List<QuestionModel> questions { get; set; } = null!;
    }
}
