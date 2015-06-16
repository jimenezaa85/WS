using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaxRay.Contracts;
using TaxRay.Model;
using TaxRay.Tests.Helpers;
using TaxRay.Web.Controllers;

namespace TaxRay.Web.Tests.Unit
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TaxReturnControllerRegression
    {
        private const string URL = "http://localhost/api/TaxReturn/Get?pageNumber=1&pageSize=5";

        private TaxReturnController CreateController(IEnumerable<TaxReturn> tasksInRepository = null)
        {

            var user = MockRepository.GenerateStub<IPrincipal>();
            user.Stub(x => x.IsInRole("SuperAdmin")).Return(true);
            //user.Identity();
            
            var identity = MockRepository.GenerateStub<IIdentity>();
            identity.Stub(p => p.Name).Return("TestUser").Repeat.Any();



            var taxes = (tasksInRepository == null) ? new[]
            {
                new TaxReturn{Id=1, Description = "ZZZ", Client = "John Smith", TaxPayer="Murphy Tran", Year=2014, DueDate=new DateTime(2015,01,01), Status="Closed", AssignedToId=1},
                new TaxReturn{Id=2, Description = "BBB", Client = "Marta Currado",TaxPayer="Quamar Mccullough", Year=2015, DueDate=new DateTime(2015,06,01), Status="Submitted", AssignedToId=1}
            }.AsQueryable() : tasksInRepository.AsQueryable();

            

            var uow = MockRepository.GenerateStub<ITaxRayUow>();
            

            uow.Stub(x => x.Taxes.GetAll())
                .IgnoreArguments()
                .Return(taxes);

            uow.Stub(x => x.Taxes.GetAssignedTasks(null))
                .IgnoreArguments()
                .Return(taxes);

            var controller = new TaxReturnController(uow);
            HttpTestHelper.SetupControllerForTests(controller, URL, "TaxPagination", WebApiConfig.ControllerAction, loggedInUser);
            return controller;
        }

        /*private IEnumerable<TaxReturn> CreateTaxes(int count)
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
        }*/


        [TestMethod]
        public void Get_Returns_Everything_In_Repository_1()
        {
            //Arrange
            var controller = CreateController();

            //Act
            var result = controller.GetAll();

            //Assert
            Assert.AreEqual(2, result.Count());
        }
    }
}
