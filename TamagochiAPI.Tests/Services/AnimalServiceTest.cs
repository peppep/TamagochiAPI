using NSubstitute;
using NUnit.Framework;
using TamagochiAPI.Common.Models;
using TamagochiAPI.Common.OutputData;
using TamagochiAPI.Configs;
using TamagochiAPI.DAL.Wrappers;
using TamagochiAPI.Services;

namespace TamagochiAPI.Tests.Services
{
	[TestFixture]
	public class AnimalServiceTest
	{
		private const uint UserId = 1;
		private const uint AnimalId = 1;
		private const string AnimalName = "puppy";

		private IAnimalService m_animalService;
		private IAnimalsWrapper m_animalsWrapper;
		private IUserService m_usersService;
		private IConfigService m_configService;

		[SetUp]
		public void SetUp()
		{
			m_animalsWrapper = Substitute.For<IAnimalsWrapper>();
			m_usersService = Substitute.For<IUserService>();
			m_configService = Substitute.For<IConfigService>();
			m_animalService = new AnimalService(m_animalsWrapper, m_usersService, m_configService);
		}

		[TestCase(ResultCode.SessionClosed)]
		[TestCase(ResultCode.UserNotFound)]
		public void ShouldNotAddAnimalIfUserNotExistsOrSessionClosed(ResultCode code)
		{
			m_usersService.GetUser(Arg.Any<uint>(), true).Returns(new ResultInfo<User> { ResultCode = code });

			var res = m_animalService.AddAnimal(AnimalName, UserId, AnimalType.Cat);

			Assert.AreEqual(code, res.ResultCode);
		}

		[Test]
		public void ShouldNotAddAnimalIfNameIsUsed()
		{
			m_usersService.GetUser(Arg.Any<uint>(), true).Returns(new ResultInfo<User> { ResultCode = ResultCode.Ok });
			m_animalsWrapper.GetAnimalByName(Arg.Any<string>()).Returns(new Animal());

			var res = m_animalService.AddAnimal(AnimalName, UserId, AnimalType.Cat);

			Assert.AreEqual(ResultCode.NameRestricted, res.ResultCode);
		}

		[Test]
		public void ShouldAddAnimalIfAllRestrictionPassed()
		{
			m_usersService.GetUser(Arg.Any<uint>(), true).Returns(new ResultInfo<User> { ResultCode = ResultCode.Ok });

			var res = m_animalService.AddAnimal(AnimalName, UserId, AnimalType.Cat);

			m_animalsWrapper.Received().AddAnimal(Arg.Any<Animal>());
			Assert.AreEqual(ResultCode.Ok, res.ResultCode);
		}

		[Test]
		public void ShouldReturnAnimal()
		{
			m_animalsWrapper.GetAnimalById(Arg.Any<uint>()).Returns(new Animal());

			var res = m_animalService.GetAnimal(AnimalId);
			Assert.AreEqual(ResultCode.Ok, res.ResultCode);
		}

		[Test]
		public void ShouldReturnAnimalNotFound()
		{
			var res = m_animalService.GetAnimal(AnimalId);
			Assert.IsTrue(res.IsEmpty());
			Assert.AreEqual(ResultCode.AnimalNotFound, res.ResultCode);
		}
	}
}