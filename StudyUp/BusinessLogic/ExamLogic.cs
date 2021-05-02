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
    public class ExamLogic : IExamLogic
    {
        private IRepository<Exam> examRepository;
        private IRepository<User> userRepository;
        private IRepository<ExamCard> examCardRepository;
        private IUserRepository userTokenRepository;

        public ExamLogic(IRepository<Exam> repository, IRepository<User> userRepository,
            IUserRepository userTokenRepository, IRepository<ExamCard> examCardRepository)
        {
            this.examRepository = repository;
            this.userRepository = userRepository;
            this.userTokenRepository = userTokenRepository;
            this.examCardRepository = examCardRepository;
        }

        public Exam AddExam(Exam exam, string userToken)
        {
            User user = userTokenRepository.GetUserByToken(userToken);

            if (user is null)
                throw new NotAuthenticatedException(UnauthenticatedMessage.UNAUTHENTICATED_USER);

            if (user.IsStudent)
                throw new InvalidException(ExamMessage.NOT_A_TEACHER);

            IEnumerable<Exam> sameName = examRepository.GetAll().Where(x => x.Name.Equals(exam.Name)
            && x.Author.Username.Equals(user.Username));

            if (sameName != null && (sameName.Count() > 0))
                throw new AlreadyExistsException(ExamMessage.EXAM_ALREADY_EXISTS);
            else if (exam.Name.Trim() is null)
                throw new InvalidException(ExamMessage.EMPTY_NAME_MESSAGE);
            else if ((int)exam.Difficulty < 0 || (int)exam.Difficulty > 2)
                throw new InvalidException(ExamMessage.INVALID_DIFFICULTY);
            else if (exam.Subject is null)
                throw new InvalidException(ExamMessage.EMPTY_SUBJECT_MESSAGE);

            exam.Author = user;
            examRepository.Add(exam);

            user.Exams.Add(exam);
            userRepository.Update(user);

            return exam;
        }

        public Exam GetExamById(int id, string token)
        {
            User user = userTokenRepository.GetUserByToken(token);

            if (user is null)
                throw new NotFoundException(UserMessage.USER_NOT_FOUND);

            Exam exam = this.examRepository.GetById(id);

            if (exam !=null && !exam.Author.Equals(user))
                throw new InvalidException(ExamMessage.INVALID_USER);

            if (exam != null)
                return exam;
            else
                throw new NotFoundException(ExamMessage.EXAM_NOT_FOUND);
        }

        public IEnumerable<Exam> GetTeachersExams(string token)
        {
            ICollection<Exam> toReturn = new List<Exam>();
            User user = userTokenRepository.GetUserByToken(token);

            if (user is null)
                throw new NotFoundException(UserMessage.USER_NOT_FOUND);

            if (user.IsStudent is true)
                throw new InvalidException(ExamMessage.CANNOT_GET_EXAMS);

            var teachersExams = examRepository.FindByCondition(t => t.Author.Equals(user));
            return teachersExams;
        }
    }
}
