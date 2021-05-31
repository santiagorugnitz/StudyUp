using Domain;

namespace BusinessLogicInterface
{
    public interface IExamCardLogic
    {
        ExamCard AddExamCard(int examId, ExamCard examCard, string token);
        bool DeleteExamCard(int id, string token);
        ExamCard EditExamCard(string token, int examCardId, ExamCard newExamcard);
    }
}
