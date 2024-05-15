namespace Form.DTO
{
    /// <summary>
    /// Candidate form DTO.
    /// </summary>
    public class QuestionAnswerDTO
    {
        public string FormId { get; set; } = null!;
        public string Title { get; set; } = null!;

        /// <summary>
        /// list of questions and answers submitted by candidate.
        /// </summary>
        public List<QuestionDTO> questionDTOs { get; set; } = null!;
    }
}
