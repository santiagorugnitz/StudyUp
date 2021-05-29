using BusinessLogicInterface;
using DataAccessInterface;
using Domain;
using Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic
{
    public class ExamCardLogic : IExamCardLogic
    {
        private IRepository<Exam> examRepository;
        private IRepository<User> userRepository;
        private IRepository<ExamCard> examCardRepository;
        private IUserRepository userTokenRepository;

        public ExamCardLogic(IRepository<Exam> repository, IRepository<User> userRepository,
            IUserRepository userTokenRepository, IRepository<ExamCard> examCardRepository)
        {
            this.examRepository = repository;
            this.userRepository = userRepository;
            this.userTokenRepository = userTokenRepository;
            this.examCardRepository = examCardRepository;
        }

        public ExamCard AddExamCard(int examId, ExamCard examCard, string token)
        {
            User user = UserByToken(token);

            if (user.IsStudent)
                throw new InvalidException(ExamMessage.NOT_A_TEACHER);

            Exam exam = examRepository.GetById(examId);

            if (exam is null)
                throw new NotFoundException(ExamMessage.EXAM_NOT_FOUND);

            if (!exam.Author.Equals(user))
                throw new InvalidException(ExamCardMessage.INVALID_AUTHOR);

            IEnumerable<ExamCard> sameQuestion = examCardRepository.GetAll().Where(
                a => a.Exam.Id == examId && a.Question.ToUpper().Equals(examCard.Question.ToUpper()));

            if (sameQuestion != null && (sameQuestion.Count() > 0))
                throw new AlreadyExistsException(ExamCardMessage.EXAMCARD_ALREADY_EXISTS);

            examCard.Exam = exam;
            examCardRepository.Add(examCard);

            exam.ExamCards.Add(examCard);
            examRepository.Update(exam);

            return examCard;
        }

        public bool DeleteExamCard(int id, string token)
        {
            ExamCard examCard = examCardRepository.GetById(id);

            if (examCard is null)
                throw new NotFoundException(ExamCardMessage.EXAMCARD_NOT_FOUND);

            User user = this.userRepository.GetById(examCard.Exam.Author.Id);

            if (user!=null && !user.Token.Equals(token))
                throw new InvalidException(ExamCardMessage.NOT_AUTHORIZED_TO_DELETE);

            Exam exam = examCard.Exam;
            exam.ExamCards.Remove(examCard);
            examRepository.Update(exam);
            userRepository.Update(exam.Author);
            return true;
        }

        public ExamCard EditExamCard(string token, int examCardId, string newQuestion, bool newAnswer)
        {
            ExamCard examcard = examCardRepository.GetById(examCardId);
            User user = UserByToken(token);

            if (examcard is null)
                throw new NotFoundException(ExamCardMessage.EXAMCARD_NOT_FOUND);

            else if (user.Id != examcard.Exam.Author.Id)
                throw new InvalidException(ExamCardMessage.NOT_AUTHORIZED_TO_EDIT);

            else if (examcard != null && user != null)
            {
                examcard.Question = newQuestion;
                examcard.Answer = newAnswer;
                examCardRepository.Update(examcard);
            }

            ExamCard updatedExamcard = examCardRepository.GetById(examCardId);
            return updatedExamcard;
        }

        private User UserByToken(string token)
        {
            User user = userTokenRepository.GetUserByToken(token);

            if (user is null)
                throw new InvalidException(UnauthenticatedMessage.UNAUTHENTICATED_USER);
            else
                return user;
        }
    }
}
