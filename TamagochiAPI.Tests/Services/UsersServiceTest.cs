using NSubstitute;
using NUnit.Framework;
using System;
using TamagochiAPI.Common;
using TamagochiAPI.Configs;
using TamagochiAPI.DAL.SQLite.Models;
using TamagochiAPI.DAL.Wrappers;
using TamagochiAPI.Services;

namespace TamagochiAPI.Tests.Services
{
	[TestFixture]
	public class UsersServiceTest
	{
		private IUserService m_userService;
		private IUsersWrapper m_userWrapper;
		private IConfigService m_configService;

		private const uint UserId = 1;
		private const string Nickname = "test_user";

		[SetUp]
		public void SetUp()
		{
			m_userWrapper = Substitute.For<IUsersWrapper>();
			m_configService = Substitute.For<IConfigService>();

			m_userService = new UsersService(m_userWrapper, m_configService);
		}

		[Test]
		public void ShouldNotLoginIfUserNotFound()
		{
			var res = m_userService.Login(UserId);

			Assert.AreEqual(ResultCode.UserNotFound, res.ResultCode);
			m_userWrapper.DidNotReceive().UpdateLoginTime(Arg.Any<uint>(), Arg.Any<DateTime>());
		}

		[Test]
		public void ShouldUpdateLoginTimeIfUserExists()
		{
			m_userWrapper.GetUserInfoById(Arg.Any<uint>())
				.Returns(
					new User
					{
						UserId = UserId,
						Name = Nickname
					});

			var res = m_userService.Login(UserId);

			Assert.AreEqual(ResultCode.Ok, res.ResultCode);
			m_userWrapper.Received().UpdateLoginTime(Arg.Any<uint>(), Arg.Any<DateTime>());
		}

		[Test]
		public void ShouldNotCreateUserIfSameNicknameExists()
		{
			m_userWrapper.GetUserInfoByNick(Arg.Any<string>()).Returns(new User { UserId = UserId });
			var res = m_userService.CreateUser(Nickname);

			Assert.AreEqual(ResultCode.NameRestricted, res.ResultCode);
			m_userWrapper.DidNotReceive().AddUser(Arg.Any<User>());
		}

		[Test]
		public void ShouldCreateNewUserIfNicknameIsNotUsed()
		{
			var res = m_userService.CreateUser(Nickname);

			Assert.AreEqual(ResultCode.Ok, res.ResultCode);
			m_userWrapper.Received().AddUser(Arg.Any<User>());
		}

		[Test]
		public void ShouldNotCheckUserRestrictionsAndReturnEmptyCollection()
		{
			var res = m_userService.GetUser(UserId);
			Assert.AreEqual(ResultCode.Ok, res.ResultCode);
			Assert.IsTrue(res.IsEmpty());
		}

		[Test]
		public void ShouldNotCheckUserRestrictionsAndReturnUserInfo()
		{
			m_userWrapper.GetUserInfoById(Arg.Any<uint>()).Returns(new User { UserId = UserId });

			var res = m_userService.GetUser(UserId);
			Assert.AreEqual(ResultCode.Ok, res.ResultCode);
			Assert.AreEqual(1, res.ResultData.Count);
			Assert.IsTrue(!res.IsEmpty());
		}

		[Test]
		public void ShouldReturnUserNotFoundAndEmptyCollection()
		{
			var res = m_userService.GetUser(UserId, true);
			Assert.AreEqual(ResultCode.UserNotFound, res.ResultCode);
			Assert.IsTrue(res.IsEmpty());
		}

		[Test]
		public void ShouldReturnSessionClosedAndUserInfo()
		{
			m_userWrapper.GetUserInfoById(Arg.Any<uint>()).Returns(new User { UserId = UserId });

			var res = m_userService.GetUser(UserId, true);
			Assert.AreEqual(ResultCode.SessionClosed, res.ResultCode);
			Assert.IsTrue(!res.IsEmpty());
		}
	}
}