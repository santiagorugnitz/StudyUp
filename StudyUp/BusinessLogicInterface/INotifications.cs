using Domain;

namespace BusinessLogicInterface
{
    public interface INotifications
    {
        void NotifyComments(FlashcardComment comment, User receiver);
        void NotifyExams(Exam exam, Group group);
        void NotifyMaterial(Deck deck, Group group);
    }
}
