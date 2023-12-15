using FluentAssertions;
using LegacyApp;
using LegacyApp.DataAccess;
using LegacyApp.Repositories;
using LegacyApp.Services;
using NSubstitute;

namespace RefactoringTest.UnitTests;

public class UserServiceTests
{
    private readonly UserService userService;
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();
    private readonly IClientRepository _clientRepository = Substitute.For<IClientRepository>();
    private readonly IUserDataAccess _userDataAccess = Substitute.For<IUserDataAccess>();
    private readonly IUserCreditService _userCreditService = Substitute.For<IUserCreditService>();

    public UserServiceTests()
    {
        userService = new UserService(
            _dateTimeProvider,
            _clientRepository,
            _userCreditService,
            _userDataAccess
            );
    }
    
    [Fact]
    public void AddUserShouldCreateUser()
    {
        var testClient = new ClientBuilder()
            .WithTestValues()
            .Build();
        
        _dateTimeProvider.DateTimeNow.Returns(new DateTime(2023, 12, 14));
        _clientRepository.GetById(testClient.Id).Returns(testClient);
        _userCreditService.GetCreditLimit("Tre", "Jones", new DateTime(1982, 1, 1))
            .Returns(600);

        var result = userService.AddUser("Tre", "Jones",
            "Test@Email.com", new DateTime(1982, 1, 1), testClient.Id);

        result.Should().BeTrue();
    }

}
