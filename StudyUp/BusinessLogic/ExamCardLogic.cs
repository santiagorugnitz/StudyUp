using BusinessLogicInterface;
using DataAccessInterface;
using Domain;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            User user = userTokenRepository.GetUserByToken(token);

            if (user is null)
                throw new NotAuthenticatedException(UnauthenticatedMessage.UNAUTHENTICATED_USER);

            if (user.IsStudent)
                throw new InvalidException(ExamMessage.NOT_A_TEACHER);

            Exam exam = examRepository.GetById(examId);

            if (exam is null)
                throw new NotFoundException(ExamMessage.EXAM_NOT_FOUND);

            if (!exam.Author.Equals(user))
                throw new InvalidException(ExamCardMessage.INVALID_AUTHOR);

            IEnumerable<ExamCard> sameQuestion = examCardRepository.GetAll().Where(a => a.Exam.Id == examId && 
            a.Question.ToUpper().Equals(examCard.Question.ToUpper()));

            if (sameQuestion != null && (sameQuestion.Count() > 0))
                throw new AlreadyExistsException(ExamCardMessage.EXAMCARD_ALREADY_EXISTS);

            examCard.Exam = exam;
            examCardRepository.Add(examCard);

            exam.ExamCards.Add(examCard);
            examRepository.Update(exam);

            return examCard;
        }
    }
}
