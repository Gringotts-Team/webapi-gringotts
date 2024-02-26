using Microsoft.VisualStudio.TestTools.UnitTesting;
using gringotts_api.Controllers;
using gringotts_api.DataContext;
using gringotts_api.DTOs;
using gringotts_api.Services;
using gringotts_api.Services.Imp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace gringotts_api.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTest
    {
        private readonly GringottsDbContext _context;
        private readonly IAuthService _authService;
        public IConfiguration Configuration;
        private readonly UserController _userController;
        private readonly IUserService _userService;

        public UserControllerTest()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") 
                .Build();

            var connectionString = Configuration.GetConnectionString("gringottsDB");

            var options = new DbContextOptionsBuilder<GringottsDbContext>()
                .UseNpgsql(connectionString)
                .Options;

            _context = new GringottsDbContext(options);

            _authService = new AuthServiceImp(_context, Configuration);
            _userController = new UserController(_context, _authService, _userService );
        }

        [TestMethod()]
        public async Task PostLoginUserTest()
        {
            UserLoginDTO loginDTO = new UserLoginDTO { name = "Uruk", password = "uruk!_123" };


            var actionResult = await _userController.PostLoginUser(loginDTO);

            var okResult = actionResult.Result as OkObjectResult;


            Assert.AreEqual(200, okResult.StatusCode);
            
        }

        [TestMethod()]
        public async Task PostLoginInvalidCredentialsTest()
        {
            UserLoginDTO loginDTO = new UserLoginDTO { name = "JuampiCapo", password = "StefanoCapo" };
            var resultJson = JsonSerializer.Serialize(new { msg = "Invalid Credentials" } ) ;

            var resultController = await _userController.PostLoginUser(loginDTO);

            BadRequestObjectResult badRequest = (BadRequestObjectResult)resultController.Result;

            var json = JsonSerializer.Serialize(badRequest.Value);
            


            // Assert
            //Assert.IsInstanceOfType(result.GetType, typeof(BadRequestObjectResult));
            Assert.AreEqual(resultJson, json);

        }

        [TestMethod()]
        public void GetTokenValidatedTest()
        {
            // Placeholder test method, you can implement it based on your requirements
            Assert.Fail("Not implemented");
        }

        [TestMethod()]
        public void Generate_Token_Successfully()
        {
            // Arrange: Variables to be used in the test
            UserLoginDTO loginDTO = new UserLoginDTO { name = "Uruk", password = "uruk!_123" };

            // Act: Invoke the method you want to test
            string token = _authService.GenerateToken(loginDTO).Result;

            // Assert: Compare the results of the Act with the expected results
            Assert.IsNotNull(token);
        }
    }
}
