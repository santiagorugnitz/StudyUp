using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicInterface
{
    public interface INotifications
    {
        void NotifyMaterial(int deckId, Group group);
        void NotifyExams(int examId, Group group);
        void NotifyComments(int commentId, User receiver);
    }
}
