using Domain;

namespace BusinessLogicInterface
{
    public interface IExamCardLogic
    {
        ExamCard AddExamCard(int examId, ExamCard examCard, string token);
        ExamCard EditExamCard(string token, int examCardId, string newQuestion, bool newAnswer);
        bool DeleteExamCard(int id, string token);
    }
}
