using System.ComponentModel.DataAnnotations;

namespace Form.DTO
{
    public class QuestionDTO : IValidatableObject
    {
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
            var validationErrors = new List<ValidationResult>();
            var validQuestionType = new List<string>()
            {
                "Paragraph".ToLower(),
                "YesNo".ToLower(),
                "DropDown".ToLower(),
                "MultipleChoice".ToLower(),
                "Date".ToLower(),
                "Number".ToLower()
            };
            //Validation based on question type.
            if (!validQuestionType.Contains(type.ToLower()))
            {
                validationErrors.Add(new ValidationResult("question type can be only one of the following values:Paragraph,YesNo,DropDown,MultipleChoice,Date,Number", new[] { nameof(type) }));
            }
            if (type == "YesNo".ToLower())
            {
                if (answer != "Yes".ToLower()  || answer != "No".ToLower())
                    validationErrors.Add(new ValidationResult("You can only enter value Yes or No as answer", new[] { nameof(type) }));
            }
            if (type == "DropDown".ToLower() && (choices == null || choices.Count == 0))
            {
                validationErrors.Add(new ValidationResult("DropDown question must have at least one choice", new[] { nameof(choices) }));
            }
            if ((type == "DropDown".ToLower() && !string.IsNullOrEmpty(answer)) || (type == "MultipleChoice".ToLower() && !string.IsNullOrEmpty(answer)))
            {
                validationErrors.Add(new ValidationResult($"For {type} question answer field must be null.", new[] { nameof(choices) }));
            }
            if (type == "MultipleChoice".ToLower() && (choices == null || choices.Count < 2))
            {
                validationErrors.Add(new ValidationResult("Multiple choice questions must have at least two choice", new[] { nameof(choices) }));
            }

            if ((type == "DropDown".ToLower() || type == "MultipleChoice".ToLower()) && (answers == null || answers.Count == 0))
            {
                validationErrors.Add(new ValidationResult("Please select at least one answer", new[] { nameof(choices) }));
            }
            return validationErrors;
        }
    }

}
