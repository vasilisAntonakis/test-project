using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebAPI.Model;

namespace WebAPI.Tests
{
    public class TestService
    {
        protected Mock<DbContext> MockContext;

        protected List<Lawyer> Lawyers;
        protected List<Gender> Genders;
        protected List<Title> Titles;

        protected Mock<DbSet<Lawyer>> MockLawyersSet;
        protected Mock<DbSet<Gender>> MockGendersSet;
        protected Mock<DbSet<Title>> MockTitlesSet;

        protected Mock<DbSet<T>> MockDbSet<T>(List<T> list) where T : Entity
        {
            // convert the list to queryable in order to wire with the mock
            // in order to make it consider the list as a datasource
            var queryable = list.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            // mock needs to be explicitly told to fetch x with each .Find(x)
            // and to add a new instance y with .Add(y)
            foreach (var element in list)
            {
                mockSet.Setup(g => g.Find(element.Id)).Returns(element);
            }
            mockSet.Setup(g => g.Add(It.IsAny<T>())).Callback<T>(newT =>
            {
                list.Add(newT);
                mockSet.Setup(g => g.Find(newT.Id)).Returns(newT);
            });

            // return the newly created mock
            return mockSet;
        }

        protected void AddMockDbSetToMockContext<T>(Mock<DbSet<T>> mock) where T : Entity
        {
            if (MockContext == null)
            {
                MockContext = new Mock<DbContext>();
            }
            MockContext.Setup(c => c.Set<T>()).Returns(mock.Object);
        }

        /// <summary>
        /// This function is executed before each [TestMethod].
        /// It Prepares the in-memory context with the default doubles.
        /// It can be overridden if needed.
        /// </summary>
        [TestInitialize]
        public virtual void Setup()
        {
            // prepare lists
            Lawyers = Doubles.Lawyers();
            Genders = Doubles.Genders();
            Titles = Doubles.Titles();

            // prepare mockSets
            MockLawyersSet = MockDbSet(Lawyers);
            MockGendersSet = MockDbSet(Genders);
            MockTitlesSet = MockDbSet(Titles);

            MockContext = new Mock<DbContext>();

            // register mocks to MockContext
            AddMockDbSetToMockContext(MockLawyersSet);
            AddMockDbSetToMockContext(MockGendersSet);
            AddMockDbSetToMockContext(MockTitlesSet);
        }

        /// <summary>
        /// This function is executed after each [TestMethod].
        /// It cleans up everymock instatiated before or in the [TestMethod].
        /// It can be overridden if needed.
        /// </summary>
        [TestCleanup]
        public virtual void Clean()
        {
            Lawyers = null;
            Genders = null;
            Titles = null;

            MockLawyersSet = null;
            MockGendersSet = null;
            MockTitlesSet = null;

            MockContext = null;
        }
    }
}