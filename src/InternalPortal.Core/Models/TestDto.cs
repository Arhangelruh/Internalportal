namespace InternalPortal.Core.Models
{
    public class TestDto
    {
        public int Id { get; set; }

        public List<TestQuestions> Questions { get; set; }

        public List<TestQuestionAnswers> Answers { get; set; }
    }
}
