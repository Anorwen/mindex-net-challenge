using challenge.Models;
using code_challenge.Tests.Integration.Extensions;
using code_challenge.Tests.Integration.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            // Arrange
            String employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            Compensation expected = CreateCompensation(employeeId);

            // Execute
            Compensation actual = AddCompensation(expected);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.EmployeeId, actual.EmployeeId);
            Assert.AreEqual(expected.EffectiveDate, actual.EffectiveDate);
            Assert.AreEqual(expected.Salary, actual.Salary);

        }

        [TestMethod]
        public void GetCompensationById_Returns_Ok()
        {
            // Arrange
            String employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            Compensation expected = CreateCompensation(employeeId);
            AddCompensation(expected);

            // Execute
            var response = _httpClient.GetAsync($"api/compensation/{employeeId}").Result;
            Compensation actual = response.DeserializeContent<Compensation>();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.EmployeeId, actual.EmployeeId);
            Assert.AreEqual(expected.EffectiveDate, actual.EffectiveDate);
            Assert.AreEqual(expected.Salary, actual.Salary);
        }

        [TestMethod]
        public void GetCompensationById_Returns_NotFound()
        {
            // Arrange
            String employeeId = "Invalid-Id";

            // Exeucte
            var response = _httpClient.GetAsync($"api/compensation/{employeeId}").Result;

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        /**
         * Use the Post route to save a Compensation object.
         */
        private Compensation AddCompensation(Compensation compensation)
        {
            // Arrange
            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync($"api/compensation",
                new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;
            return response.DeserializeContent<Compensation>();
        }

        /**
         * Create a compensation object for the employee.
         */
        private Compensation CreateCompensation(String employeeId)
        {
            return new Compensation()
            {
                EmployeeId = employeeId,
                EffectiveDate = DateTime.Now,
                Salary = 20
            };
        }
    }
}
