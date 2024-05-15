using Form.Model;

namespace Form.DTO
{
    /// <summary>
    /// This class represents employer form DTO.
    /// </summary>
    public class QuestionaireDTO
    {
        public string Title { get; set; } = null!;
        /// <summary>
        /// list of questions but don't specify answers for the question.
        /// </summary>
        public List<QuestionDTO> questionDTOs { get; set; } = null!;
    }
}
