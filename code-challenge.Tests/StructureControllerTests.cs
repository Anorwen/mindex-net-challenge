using challenge.Models;
using code_challenge.Tests.Integration.Extensions;
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
    public class StructureControllerTests
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
        public void GetStructureById_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedNumber = 4;
            var expectedEmployeeFirstName = "John";
            var expectedEmployeeLastName = "Lennon";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/structure/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var structure = response.DeserializeContent<ReportingStructure>();
            var employee = structure.Employee;
            Assert.AreEqual(expectedNumber, structure.NumberOfReports);
            Assert.AreEqual(expectedEmployeeFirstName, employee.FirstName);
            Assert.AreEqual(expectedEmployeeLastName, employee.LastName);
        }

        [TestMethod]
        public void GetStructureById_Returns_NotFound()
        {
            // Arrange
            var employeeId = "Invalid-Id";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/structure/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
