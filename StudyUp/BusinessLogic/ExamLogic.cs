using BusinessLogicInterface;
using DataAccessInterface;
using Domain;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic
{
    public class ExamLogic : IExamLogic
    {
        private IRepository<Exam> examRepository;
        private IRepository<User> userRepository;
        private IRepository<ExamCard> examCardRepository;
        private IRepository<Group> groupRepository;
        private IUserRepository userTokenRepository;
        private INotifications notificationsInterface;

        public ExamLogic(IRepository<Exam> repository, IRepository<User> userRepository,
            IUserRepository userTokenRepository, IRepository<ExamCard> examCardRepository,
            IRepository<Group> groupRepository, INotifications notificationsInterface)
        {
            this.groupRepository = groupRepository;
            this.examRepository = repository;
            this.userRepository = userRepository;
            this.userTokenRepository = userTokenRepository;
            this.examCardRepository = examCardRepository;
            this.notificationsInterface = notificationsInterface;
        }

        public Exam AddExam(Exam exam, string userToken)
        {
            User user = UserByToken(userToken);

            if (user.IsStudent)
                throw new InvalidException(ExamMessage.NOT_A_TEACHER);

            IEnumerable<Exam> sameName = examRepository.GetAll().Where(x => x.Name.Equals(exam.Name)
            && x.Author.Username.Equals(user.Username));

            if (sameName != null && (sameName.Count() > 0))
                throw new AlreadyExistsException(ExamMessage.EXAM_ALREADY_EXISTS);
            else if (exam.Name is null || exam.Name.Trim() is "")
                throw new InvalidException(ExamMessage.EMPTY_NAME_MESSAGE);
            else if ((int)exam.Difficulty < 0 || (int)exam.Difficulty > 2)
                throw new InvalidException(ExamMessage.INVALID_DIFFICULTY);
            else if (exam.Subject is null || exam.Subject.Trim() is "")
                throw new InvalidException(ExamMessage.EMPTY_SUBJECT_MESSAGE);

            exam.Author = user;
            examRepository.Add(exam);

            user.Exams.Add(exam);
            userRepository.Update(user);

            return exam;
        }

        public Exam AssignExam(string token, int groupId, int examId)
        {
            User user = UserByToken(token);
            Exam exam = examRepository.GetById(examId);
            Group group = groupRepository.GetById(groupId);

            if (group is null)
                throw new NotFoundException(GroupMessage.GROUP_NOT_FOUND);

            if (exam is null)
                throw new NotFoundException(ExamMessage.EXAM_NOT_FOUND);

            if (!(exam.Group is null))
                throw new AlreadyExistsException(ExamMessage.ALREADY_ASSIGNED);

            if (!group.Creator.Equals(user))
                throw new InvalidException(ExamMessage.NOT_AUTHORIZED);

            ICollection<ExamCard> cards = this.examCardRepository.FindByCondition(
                ex => ex.Exam.Equals(exam));

            if (cards.Count < 1)
                throw new InvalidException(ExamMessage.NO_EXAMCARDS);

            this.notificationsInterface.NotifyExams(exam, group);

            exam.Group = group;
            examRepository.Update(exam);

            group.AssignedExams.Add(exam);
            groupRepository.Update(group);
            return exam;
        }

        public void AssignResults(int examId, string token, int time, int correctAnswers)
        {
            User user = UserByToken(token);

            Exam exam = this.examRepository.GetById(examId);

            if (exam == null)
                throw new NotFoundException(ExamMessage.EXAM_NOT_FOUND);

            UserExam userExam = null;

            try
            {
                userExam = exam.AlreadyPerformed.Find(ue => ue.UserId == user.Id
                    && ue.ExamId == exam.Id);
            }
            catch (NullReferenceException) { }

            if (userExam != null && userExam.Score != null)
                throw new InvalidException(ExamMessage.ALREADY_COMPLETED);

            double score = CalculateScore(time, correctAnswers, exam.ExamCards.Count());

            AddExamPerformance(userExam, exam, user, score);
        }

        private void AddExamPerformance(UserExam userExam, Exam exam, User user, double score)
        {
            if (userExam == null)
            {
                exam.AlreadyPerformed.Add(new UserExam()
                {
                    User = user,
                    UserId = user.Id,
                    Exam = exam,
                    ExamId = exam.Id,
                    Score = score
                });
            }
            else
            {
                userExam.Score = score;
            }

            examRepository.Update(exam);
        }

        private double CalculateScore(int time, int correctAnswers, int totalQuestions)
        {
            if (totalQuestions == 0) return 0;
            var answersPercentage = (double)correctAnswers*2 - totalQuestions;
            return answersPercentage / ((double)time / 60);
        }

        public Exam GetExamById(int id, string token)
        {
            Exam exam = this.examRepository.GetById(id);

            if (exam != null)
                return exam;
            else
                throw new NotFoundException(ExamMessage.EXAM_NOT_FOUND);
        }

        public List<Tuple<string, double>> GetResults(int examId, string token)
        {
            Exam exam = this.examRepository.GetById(examId);

            if (exam == null)
                throw new NotFoundException(ExamMessage.EXAM_NOT_FOUND);

            var result = new List<Tuple<string, double>>();

            foreach (var perform in exam.AlreadyPerformed)
            {
                if (perform.Score != null)
                    result.Add(new Tuple<string, double>(perform.User.Username, (double)perform.Score));
            }

            return result;
        }

        public IEnumerable<Exam> GetTeachersExams(string token)
        {
            ICollection<Exam> toReturn = new List<Exam>();
            User user = userTokenRepository.GetUserByToken(token);

            if (user is null)
                throw new NotFoundException(UserMessage.USER_NOT_FOUND);

            if (user.IsStudent)
                throw new InvalidException(ExamMessage.CANNOT_GET_EXAMS);

            var teachersExams = examRepository.FindByCondition(t => t.Author.Equals(user));
            return teachersExams;
        }

        private User UserByToken(string token)
        {
            User user = userTokenRepository.GetUserByToken(token);

            if (user is null)
                throw new NotAuthenticatedException(UnauthenticatedMessage.UNAUTHENTICATED_USER);
            else
                return user;
        }
    }
}
