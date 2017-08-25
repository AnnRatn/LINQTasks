using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace LINQTasks
{
    public class BugTracker
    {
        private readonly List<Bug> _bugs = new List<Bug>();
        private readonly List<User> _users = new List<User>();

        public IReadOnlyCollection<User> Users => _users;
        public IReadOnlyCollection<Bug> Bugs => _bugs;


        public User CreateUser(string name, UserType userType)
        {
            var user = new User(name, userType);
            _users.Add(user);
            return user;
        }

        public Bug CreateBug(string info, User createdBy, Priority priority = Priority.Normal)
        {
            var bug = new Bug(info, createdBy, priority);
            _bugs.Add(bug);
            return bug;
        }

        /// <summary>
        /// Возвращает все открытые ошибки
        /// </summary>
        public IEnumerable<Bug> GetOpenBugs()
        {
            IEnumerable<Bug> bugs = _bugs.Where(b => b.Status != Status.Closed);
            return bugs;
        }

        /// <summary>
        /// Возвращает все открытые ошибки с приоритетом не ниже priority
        /// </summary>
        public IEnumerable<Bug> GetOpenBugs(Priority priority)
        {
            IEnumerable<Bug> bugs = _bugs.Where(b => (b.Priority >= priority) && (b.Status != Status.Closed));
            return bugs;
        }

        /// <summary>
        /// Возвращает все ошибки назначенные на определенного пользователя 
        /// </summary>
        public IEnumerable<Bug> GetBugsByUser(User assignedTo)
        {
            IEnumerable<Bug> bugs = _bugs.Where(b => b.AssignedTo == assignedTo);
            return bugs;
        }

        /// <summary>
        /// Возвращает ошибки сгруппированные по приоритету
        /// </summary>
        public IEnumerable<IGrouping<Priority, Bug>> GetBugsGroupeByPriority()
        {
           IEnumerable<IGrouping<Priority, Bug>> bugs = Bugs.GroupBy(b => b.Priority);
            return bugs;
        }

        /// <summary>
        /// Возвращается количество ошибок для каждого статуса
        /// </summary>
        public IEnumerable<Tuple<Status, int>> GetBugsCount()
        {
            IEnumerable<IGrouping<Status, Bug>> bugs = Bugs.GroupBy(b => b.Status);
            IEnumerable<Tuple<Status, int>> bug =
                from gr in bugs
                select new Tuple<Status, int>(gr.Key, gr.Count());
            return bug;

        }

        /// <summary>
        /// Возвращает все ошибки назначенные их создателю
        /// </summary>
        public IEnumerable<Bug> GetBugsAssignedToAuthor()
        {
            IEnumerable<Bug> bugs = Bugs.Where(b => b.AssignedTo == b.CreatedBy);
            return bugs;
        }

        /// <summary>
        /// Возвращает пользователей на которых не назначена ни одна ошибка
        /// </summary>
        public IEnumerable<User> GetFreeUsers()
        {
            IEnumerable<User> ar =
                from u in Users
                join b in Bugs on u equals b.AssignedTo
                select u;
            IEnumerable<User> users =
                from u in Users
                where !ar.Contains(u)
                select u;
            return users;

        }

        /// <summary>
        /// Возвращает для каждого пользователя список назначенных ему ошибок
        /// Для пользоваетлеq на которых не назначено ни одной ошибки возвращается пустой список
        /// </summary>
        public IEnumerable<Tuple<User, IEnumerable<Bug>>> GetUsersBugs()
        {
            IEnumerable<Tuple<User, IEnumerable<Bug>>> users_bugs = 
                from u in Users
                join b in Bugs on u equals b.AssignedTo
                into ar
                select new Tuple<User, IEnumerable<Bug>>(u, ar);
            return users_bugs;
        }

        /// <summary>
        /// Возвращает все ошибки отсортированные по статусу и приоритету (в рамках одинакового статуса)
        /// </summary>
        public IEnumerable<Bug> GetSortedBugs()
        {
            IEnumerable<Bug> bug = Bugs
                .OrderBy(b => b.Status)
                .ThenBy(b => b.Priority);
            return bug;
        }
    }
}
