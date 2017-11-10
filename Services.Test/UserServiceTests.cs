using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Services.Common;
using Services.DataAccess;
using System.ComponentModel.DataAnnotations;


namespace Services.Tests
{
    /// <summary>
    /// User Service methods test
    /// </summary>
    [TestClass()]
    public class UserServiceTests
    {
        // User initialization
        private User _user;

        // Repository initialization
        private Mock<IUserRepository> _repositoryMoq;

        /// <summary>
        /// Initialize data before running the tests
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _user = new User
            {
                Login = "Hacker",
                Password = "123",
                EMail = "bendy@gmail.com"
            };

            // Mock the products with Moq
            _repositoryMoq = new Mock<IUserRepository>();

        }

        /// <summary>
        /// Successfully registers a user.
        /// </summary>
        [TestMethod()]
        public void Successful_RegisterTest()
        {
            _repositoryMoq.Setup(r => r.Create(It.IsAny<User>()))
                .Callback<User>(u => u.Id += 10);
            var service = new UserService(_repositoryMoq.Object);
            service.Register(_user);

            Assert.AreNotEqual(default(int), _user.Id);
            _repositoryMoq
                .Verify(r => r.Create(It.IsAny<User>()), () => Times.Exactly(1));
        }

        /// <summary>
        /// Tries to register a user with an empty login.
        /// </summary>
        [TestMethod()]
        [DataRow(null)]
        [DataRow("")]
        public void IncorrectLogin_RegisterTest(string login)
        {
            _user.Login = login;
            var service = new UserService(_repositoryMoq.Object);

            Assert.ThrowsException<ValidationException>(() => service.Register(_user));
        }

        /// <summary>
        /// Tries to register a user with a short password.
        /// </summary>
        [TestMethod()]
        [DataRow("g")]
        [DataRow("j6")]
        [DataRow("ff")]
        public void IncorrectPassword_RegisterTest(string password)
        {
            _user.Password = password;
            var service = new UserService(_repositoryMoq.Object);

            Assert.ThrowsException<ValidationException>(() => service.Register(_user));
        }

        /// <summary>
        /// Tries to register a user with completely empty info.
        /// </summary>
        [TestMethod()]
        public void NullUser_RegisterTest_()
        {
            _user = null;
            var service = new UserService(_repositoryMoq.Object);

            Assert.ThrowsException<ArgumentNullException>(() => service.Register(_user));
        }

        /// <summary>
        /// Successfully authorizes a user.
        /// </summary>
        /// <param name="login">Login to operate with</param>
        /// <param name="password">Password to operate with</param>
        [TestMethod()]
        [DataRow("SLawrence", "sheepsheep")]
        [DataRow("JoeyD", "bendyismybae")]
        [DataRow("Alice", "gimmemahboris")]
        public void SuccessfullTest_LoginTest(string login, string password)
        {
            _user.Login = login;
            _user.Password = password;

            _repositoryMoq.Setup(r => r.Get(login))
                .Returns(new User { Password = password });
            var service = new UserService(_repositoryMoq.Object);
            var result = service.Login(login, password);

            Assert.IsTrue(result);
            _repositoryMoq.Verify(r => r.Get(login), () => Times.Exactly(1));
        }

        /// <summary>
        /// Fails to login because the user doesn't exist in database
        /// </summary>
        /// <param name="login">Login to operate with</param>
        /// <param name="password">Password to operate with</param>
        [TestMethod()]
        [DataRow("henry666", "bendeh")]
        [DataRow("jacksepticeye", "butismodeller")]
        public void LoginFailBecauseOfNonexistanceOfUsers_LoginTest(string login, string password)
        {
            _user.Login = login;
            _user.Password = password;

            _repositoryMoq.Setup(r => r.Get(login))
                .Returns<User>(null);
            var service = new UserService(_repositoryMoq.Object);
            var result = service.Login(login, password);

            Assert.IsFalse(result);
        }

        /// <summary>
        /// Tries to login with empty logins
        /// </summary>
        /// <param name="login">Login to operate with</param>
        [TestMethod()]
        [DataRow(null)]
        [DataRow("")]
        public void IncorrectLogin_LoginTest(string login)
        {
            _user.Login = login;
            var service = new UserService(_repositoryMoq.Object);

            Assert.ThrowsException<ValidationException>(() => service.Login(login, "notcoolpassword"));
        }

        /// <summary>
        /// Tries to login with empty password
        /// </summary>
        /// <param name="password">Password to operate with</param>
        [TestMethod()]
        [DataRow(null)]
        [DataRow("")]
        public void IncorrectPassword_LoginTest(string password)
        {
            _user.Password = password;
            var service = new UserService(_repositoryMoq.Object);

            Assert.ThrowsException<ValidationException>(() => service.Login("somecoollogin", password));
        }

        /// <summary>
        /// Returns a user.
        /// </summary>
        [TestMethod()]
        public void Successful_GetUserByIdTest()
        {
            _repositoryMoq.Setup(r => r.Get(It.IsAny<int>()))
                .Returns(_user);

            var service = new UserService(_repositoryMoq.Object);
            var result = service.GetUser(1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(User));
            Assert.AreSame(result, _user);
            _repositoryMoq.Verify(r => r.Get(1), () => Times.Exactly(1));
        }

        /// <summary>
        /// Tries to return a user that doesn't actually exist.
        /// </summary>
        [TestMethod()]
        public void NoSuchUserInDB_GetUserByIdTest()
        {
            _repositoryMoq.Setup(r => r.Get(It.IsAny<int>()))
                .Returns<User>(null);

            var service = new UserService(_repositoryMoq.Object);
            var result = service.GetUser(6);

            Assert.IsNull(result);
            _repositoryMoq.Verify(r => r.Get(6), () => Times.Exactly(1));
        }

        /// <summary>
        /// Returns a user by their login.
        /// </summary>
        [TestMethod()]
        public void Successful_GetUserByLoginTest()
        {
            _repositoryMoq.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(_user);

            var service = new UserService(_repositoryMoq.Object);
            var result = service.GetUserByLogin("JoeyD");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(User));
            Assert.AreSame(result, _user);
            _repositoryMoq.Verify(r => r.Get("JoeyD"), () => Times.Exactly(1));
        }

        /// <summary>
        /// Tries to return a user with empty id.
        /// </summary>
        /// <param name="login">Login to operate with.</param>
        [TestMethod()]
        [DataRow(null)]
        [DataRow("")]
        public void IncorrectLoginDarling_GetUserByLoginTest(string login)
        {
            var service = new UserService(_repositoryMoq.Object);

            Assert.ThrowsException<ValidationException>(() => service.GetUserByLogin(login));
        }

        /// <summary>
        /// Tries to return a user that doesn't exist in a data base.
        /// </summary>
        [TestMethod()]
        public void NoSuchUserInDB_GetUserByLoginTest()
        {
            _repositoryMoq.Setup(r => r.Get(It.IsAny<string>()))
                .Returns<User>(null);

            var service = new UserService(_repositoryMoq.Object);
            var result = service.GetUserByLogin("itsmemario");

            Assert.IsNull(result);
            _repositoryMoq.Verify(r => r.Get("itsmemario"), () => Times.Exactly(1));
        }

        /// <summary>
        /// Deletes a user by id.
        /// </summary>
        [TestMethod()]
        public void UnregisterTest()
        {
            _repositoryMoq.Setup(r => r.Delete(It.IsAny<int>()));

            var service = new UserService(_repositoryMoq.Object);
            service.Unregister(11);

            _repositoryMoq.Verify(r => r.Delete(11));
        }

        /// <summary>
        /// Checks a user by email.
        /// </summary>
        /// <param name="email">Email to check</param>
        [TestMethod()]
        [DataRow("bendeh@gmail.com")]
        [DataRow("funtimefreddy@mail.ru")]
        [DataRow("ballora87@yandex.ru")]
        public void Successful_CheckByEmailTest(string email)
        {
            _user.EMail = email;
            _repositoryMoq.Setup(r => r.GetByEmail(It.IsAny<string>()))
                .Returns(_user);

            var service = new UserService(_repositoryMoq.Object);
            var result = service.CheckUserByEmail(email);

            Assert.IsTrue(result);
            _repositoryMoq.Verify(r => r.GetByEmail(It.IsAny<string>()), () => Times.Exactly(1));
        }

        /// <summary>
        /// Tries to check a user with incorrect email
        /// </summary>
        /// <param name="email">Email to check</param>
        [TestMethod()]
        [DataRow("mrmeeseeks")]
        [DataRow("")]
        public void IncorrectEmail_CheckByEmailTest(string email)
        {
            _user.EMail = email;
            _repositoryMoq.Setup(r => r.GetByEmail(It.IsAny<string>()))
                .Returns(_user);

            var service = new UserService(_repositoryMoq.Object);
            var result = service.CheckUserByEmail(email);

            Assert.IsFalse(result);
            _repositoryMoq.Verify(r => r.GetByEmail(It.IsAny<string>()), () => Times.Exactly(0));
        }
    }
}