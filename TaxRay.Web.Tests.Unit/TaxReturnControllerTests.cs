using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using TaxRay.Contracts;
using TaxRay.Data;
using TaxRay.Model;
using TaxRay.Tests.Helpers;
using TaxRay.Web.Controllers;
using System.Web.Http;
using System.Web.Http.Results;

namespace TaxRay.Web.Tests.Unit
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TaxReturnControllerTests
    {
        private const string URL = "http://localhost/api/TaxReturn/Get?pageNumber=1&pageSize=5";

        private TaxReturnController CreateController(IEnumerable<TaxReturn> tasksInRepository = null)
        {
            var taxes = (tasksInRepository == null) ? new[]
        {
                new TaxReturn{Id=1, Description = "ZZZ", Client = "John Smith"},
                new TaxReturn{Id=2, Description = "BBB", Client = "Marta Currado"}
            }.AsQueryable() : tasksInRepository.AsQueryable();

            var uow = MockRepository.GenerateStub<ITaxRayUow>();

            uow.Stub(x => x.Taxes.GetAll())
                .IgnoreArguments()
                .Return(taxes);

            uow.Stub(x => x.Taxes.GetAssignedTasks(null))
                .IgnoreArguments()
                .Return(taxes);

            var controller = new TaxReturnController(uow);
            HttpTestHelper.SetupControllerForTests(controller, URL, "TaxPagination", WebApiConfig.ControllerAction);
            return controller;
        }

        private IEnumerable<TaxReturn> CreateTaxes(int count)
        {
            var taxes = new List<TaxReturn>();

            while (taxes.Count < count)
            {
                if (taxes.Count == count - 1)
                {
                    var tax = new TaxReturn { Id = taxes.Count + 1, Description = "BBB", Client = "John Smith", AssignedToId = 2 };
                    taxes.Add(tax);
                }
                else
                {
                    var tax = new TaxReturn { Id = taxes.Count + 1, Description = "Fake Tax" + taxes.Count, Client = "John Smith" + taxes.Count, AssignedToId = 1 };
                    taxes.Add(tax);
                }

            }
            return taxes;
        }


        [TestMethod]
        public void Get_Returns_Everything_In_Repository()
        {
            //Arrange
            var controller = CreateController();

            //Act
            var result = controller.GetAll();

            //Assert
            Assert.AreEqual(2, result.Count());
        }

        //[TestMethod]
        //public void Get_NegativePage_Returns_FirstPage()
        //{
        //    //Arrange
        //    const int count = 100;
        //    const int pageSize = 15;

        //    var controller = CreateController(CreateTaxes(count));

        //    //Act
        //    var result = controller.GetAll(-1, pageSize);

        //    //Assert
        //    Assert.AreEqual(pageSize, result.Results.Count());
        //}

        //[TestMethod]
        //public void Get_LastPageNumber_Returns_LastPage()
        //{
        //    //Arrange
        //    const int count = 95;
        //    const int pageSize = 15;
        //    const int pageNumber = 6;
        //    const int expectedSize = 15;

        //    var controller = CreateController(CreateTaxes(count));

        //    //Act
        //    var result = controller.Get(pageNumber, pageSize);

        //    //Assert
        //    Assert.AreEqual(expectedSize, result.Results.Count());
        //}

        [TestMethod]
        public void Get_All()
        {
            //Arrange
            const int count = 5;

            var controller = CreateController(CreateTaxes(count));

            //Act
            var result = controller.GetAll();

            //Assert
            Assert.AreEqual(count, result.Count());
        }

        //[TestMethod]
        //public void Get_Orders_By_Description()
        //{
        //    //Arrange
        //    const int count = 5;
        //    const int pageSize = 4;
        //    const int pageNumber = 1;

        //    var controller = CreateController(CreateTaxes(count));

        //    //Act
        //    var result = controller.Get(pageNumber, pageSize);

        //    //Assert
        //    Assert.AreEqual("BBB", result.Results.First().Description);
        //}


        //[TestMethod]
        //public void Get_ModelWithYear_ReturnModelWithYear()
        //{
        //    //Arrange
        //    const int year = 2015;
        //    var taxes = new[]
        //    {
        //        new TaxReturn{Id=1, Description = "ZZZ",Year=year},
        //    };

        //    var controller = CreateController(taxes);

        //    //Act
        //    var result = controller.Get();

        //    //Assert
        //    Assert.AreEqual(year, result.Results.First().Year);
        //}

        //[TestMethod]
        //public void Get_ModelWithDate_FormatHasDashes()
        //{
        //    //TODO: Refactor to avoid overflow
        //    //Arrange
        //    var dueDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month + 2, DateTime.Today.Day);
        //    var taxes = new[]
        //    {
        //        new TaxReturn{Id=1, Description = "ZZZ",DueDate = dueDate},
        //    };

        //    var controller = CreateController(taxes);

        //    //Act
        //    var result = controller.Get();

        //    //Assert
        //    var expected = string.Format("{0}.{1}.{2}",
        //        dueDate.Day.ToString().PadLeft(2, '0'),
        //        dueDate.Month.ToString().PadLeft(2, '0'),
        //        dueDate.Year);

        //    Assert.AreEqual(expected, result.Results.First().DueDate);
        //}

        [TestMethod]
        public void GetAssigned_SuperAdmin_ReturnsEveryThing()
        {
            //Arrange
            var controller = CreateController();

            //Act
            var result = controller.GetAssigned();

            //Assert
            Assert.AreEqual(2, result.TotalCount);
        }

        [TestMethod]
        public void DeleteReturnsOk()
        {
            //Arrange
            var taxes = new[]
            {
                new TaxReturn{Id=1, Description = "ZZZ"},
                new TaxReturn{Id=2, Description = "AAA"},
                new TaxReturn{Id=3, Description = "BBB"},
                new TaxReturn{Id=4, Description = "CCC"},
                new TaxReturn{Id=5, Description = "DDD"}
            };
            var controller = CreateController(taxes);

            //Act
            IHttpActionResult actionResult = controller.Delete(1);
            
            
            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }

        [TestMethod]
        public void Dispose_CallsUowDispose()
        {
            var provider = DataHelper.CreateRepositoryProvider();
            var uow = MockRepository.GenerateMock<TaxRayUow>(provider);
            var controller = new TaxReturnController(uow);

            controller.Dispose();

            uow.AssertWasCalled(u=> u.Dispose());
        }
    }
}