using System.ComponentModel.DataAnnotations;

namespace Form.Model
{
    public class QuestionModel : IValidatableObject
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        //Type: Paragraph,YesNo,DropDown,MultipleChoice,Date,Number
        public string type { get; set; } = null!;
        public string question { get; set; } = null!;

        //Available choices for dropdown and multiple choice.
        public List<string> choices { get; set; } = null!;

        //Answers for multiple choice questions and dropDown.
        public List<string> answers { get; set; } = null!;

        //Answer for other type of questions.
        public string answer { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //Validation based on question type.
            if (type == "DropDown".ToLower() && (choices == null || choices.Count == 0))
            {
                yield return new ValidationResult("DropDown question must have at least one choice", new[] { nameof(choices) });
            }

            if (type == "MultipleChoice".ToLower() && (choices == null || choices.Count < 2))
            {
                yield return new ValidationResult("Multiple choice questions must have at least two choice", new[] { nameof(choices) });
            }

            if ((type == "DropDown".ToLower() || type == "MultipleChoice".ToLower()) && (answers == null || answers.Count == 0))
            {
                yield return new ValidationResult("Please select at least one answer", new[] { nameof(choices) });
            }
        }
    }
}
