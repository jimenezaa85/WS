using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Moq;
using TaxRay.Model;

namespace TaxRay.Data.Tests.Unit
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TaxReturnRepositoryTests
    {
        private IEnumerable<TaxReturn> CreateFakeTasks()
        {
            return new List<TaxReturn>
            {
                new TaxReturn{Description = "Task1"},
                new TaxReturn{Description = "Task2", AssignedTo = new User{Username = "User1"}},
                new TaxReturn{Description = "Task3", AssignedTo = new User{Username = "User2"}}

            };
        }

        private TaxReturnRepository CreateRepository()
        {
            var data = CreateFakeTasks().AsQueryable();

            var mockSet = new Mock<DbSet<TaxReturn>>();
            mockSet.As<IQueryable<TaxReturn>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<TaxReturn>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<TaxReturn>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<TaxReturn>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

            var mockContext = new Mock<DbContext>();
            mockContext.Setup(c => c.Set<TaxReturn>()).Returns(mockSet.Object);

            return new TaxReturnRepository(mockContext.Object);            
        }

        [TestMethod]
        public void GetAssigned_NullUsername_EmptyCollection()
        {
            var repository = CreateRepository();

            var tasks = repository.GetAssignedTasks(null);

            Assert.AreEqual(0, tasks.Count());
        }


        [TestMethod]
        public void GetAssigned_EmptyUsername_EmptyCollection()
        {
            var repository = CreateRepository();

            var tasks = repository.GetAssignedTasks("");

            Assert.AreEqual(0, tasks.Count());
        }

        [TestMethod]
        public void GetAssigned_ExistingUsername_AllCurrentUserAssignedTasks()
        {
            var repository = CreateRepository();

            var tasks = repository.GetAssignedTasks("User1");

            Assert.AreEqual(1, tasks.Count());
        }

    }
}