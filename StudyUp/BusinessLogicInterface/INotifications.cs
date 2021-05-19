using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicInterface
{
    public interface INotifications
    {
        void NotifyMaterial(Deck deck, Group group);
        void NotifyExams(Exam exam, Group group);
        void NotifyComments(FlashcardComment comment, User receiver);
    }
}
