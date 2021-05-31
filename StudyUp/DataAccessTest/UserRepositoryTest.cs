using DataAccess;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DataAccessTest
{
    [TestClass]
    public class UserRepositoryTest
    {
        DbContextOptions<DBContext> options;

        [TestInitialize]
        public void StartUp()
        {
            options = new DbContextOptionsBuilder<DBContext>()
                             .UseInMemoryDatabase(databaseName: "TestDB")
                             .Options;
        }

        [TestMethod]
        public void AddUser()
        {
            using (var context = new DBContext(options))
            {
                var repo = new Repository<User>(context);

                var user = new User()
                {
                    Username = "Bob",
                    Password = "12345678",
                    Email = "bob@mail.com",
                };
                repo.Add(user);
                context.SaveChanges();

                Assert.AreEqual("Bob", repo.GetAll().First().Username);
                context.Set<User>().Remove(user);
                context.SaveChanges();

            }
        }
        [TestMethod]
        public void GetUserById()
        {
            using (var context = new DBContext(options))
            {
                var repo = new Repository<User>(context);

                var user = new User()
                {
                    Id = 1,
                    Username = "Bob",
                    Password = "12345678",
                    Email = "bob@mail.com",
                };
                context.Set<User>().Add(user);
                context.SaveChanges();
                var res = repo.GetById(1);

                Assert.AreEqual(user.Email, res.Email);

                context.Remove(user);
                context.SaveChanges();
            }
        }


        [TestMethod]
        public void FindUserByCondition()
        {
            using (var context = new DBContext(options))
            {
                var repo = new Repository<User>(context);

                var user = new User()
                {
                    Id = 1,
                    Username = "Bob",
                    Password = "12345678",
                    Email = "bob@mail.com",
                };
                context.Set<User>().Add(user);
                context.SaveChanges();
                var res = repo.FindByCondition((User user) => user.Username == "Bob");

                Assert.AreEqual(res.Count > 0, true);


                context.Remove(user);
                context.SaveChanges();
            }
        }


        [TestMethod]
        public void UpdateAdmin()
        {
            using (var context = new DBContext(options))
            {
                var repo = new Repository<User>(context);

                var user = new User()
                {
                    Username = "Bob",
                    Password = "12345678",
                    Email = "bob@mail.com",
                };
                context.Set<User>().Add(user);
                context.SaveChanges();
                user.Username = "Bob1";
                repo.Update(user);

                Assert.AreEqual(1, repo.GetAll().Count());

                context.Remove(user);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void DeleteAdmin()
        {
            using (var context = new DBContext(options))
            {
                var repo = new Repository<User>(context);

                var user = new User()
                {
                    Username = "Bob",
                    Password = "12345678",
                    Email = "bob@mail.com",
                };
                context.Set<User>().Add(user);
                context.SaveChanges();
                
                repo.Delete(user);

                Assert.AreEqual(0, repo.GetAll().Count());

            }
        }

        [TestMethod]
        public void GetAllUsers()
        {
            using (var context = new DBContext(options))
            {
                var repo = new Repository<User>(context);

                var user1 = new User()
                {
                    Username = "Bob1",
                    Password = "12345678",
                    Email = "bob1@mail.com",
                };
                var user2 = new User()
                {
                    Username = "Bob2",
                    Password = "12345678",
                    Email = "bob2@mail.com",
                };
                context.Set<User>().Add(user1);
                context.Set<User>().Add(user2);
                context.SaveChanges();

                var res = repo.GetAll();

                Assert.AreEqual(true, res.Contains(user1));
                Assert.AreEqual(true, res.Contains(user2));
                Assert.AreEqual(2, res.Count());


                context.Remove(user1);
                context.Remove(user2);
                context.SaveChanges();
            }
        }

        
        [TestMethod]
        public void FindUserByEmailAndPass()
        {
            using (var context = new DBContext(options))
            {
                var repo = new UserRepository(context);

                var user1 = new User()
                {
                    Username = "Bob1",
                    Password = "12345678",
                    Email = "bob1@mail.com",
                };

                context.Set<User>().Add(user1);
                context.SaveChanges();

                var res = repo.GetUserByEmailAndPassword("bob1@mail.com", "12345678");

                Assert.AreEqual(user1, res);

                context.Remove(user1);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void FindUserByNameAndPass()
        {
            using (var context = new DBContext(options))
            {
                var repo = new UserRepository(context);

                var user1 = new User()
                {
                    Username = "Bob1",
                    Password = "12345678",
                    Email = "bob1@mail.com",
                };

                context.Set<User>().Add(user1);
                context.SaveChanges();

                var res = repo.GetUserByNameAndPassword("Bob1", "12345678");

                Assert.AreEqual(user1, res);

                context.Remove(user1);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void FindUserByToken()
        {
            using (var context = new DBContext(options))
            {
                var repo = new UserRepository(context);

                var user1 = new User()
                {
                    Username = "Bob1",
                    Password = "12345678",
                    Email = "bob1@mail.com",
                    Token = "token"
                };

                context.Set<User>().Add(user1);
                context.SaveChanges();

                var res = repo.GetUserByToken("token");

                Assert.AreEqual(user1, res);

                context.Remove(user1);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void FindUserByNullToken()
        {
            using (var context = new DBContext(options))
            {
                var repo = new UserRepository(context);

                var res = repo.GetUserByToken(null);

                Assert.AreEqual(null, res);
            }
        }
    }
}
