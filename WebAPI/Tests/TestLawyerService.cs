using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebAPI.Data;
using WebAPI.Model;
using WebAPI.Services;

namespace WebAPI.Tests
{
    [TestClass]
    public class TestLawyerService : TestService
    {

        [TestMethod]
        public void GetOneLawyer()
        {
            // setting up the expectations
            var existing = Lawyers.ElementAt(1);
            var notExisting = new Lawyer { Id = 404 };

            // test
            using (LawyerService lawyerService = new LawyerService(MockContext.Object))
            {
                var actual = lawyerService.Get(existing.Id);
                MockLawyersSet.Verify(m => m.Find(It.IsAny<int>()), Times.Once);
                Assert.IsNotNull(actual);
                Assert.AreEqual(existing, actual);

                actual = lawyerService.Get(notExisting.Id);
                MockLawyersSet.Verify(m => m.Find(It.IsAny<int>()), Times.Exactly(2));
                Assert.IsNull(actual);
            }
        }

        [TestMethod]
        public void GetAllDefault()
        {
            // setting up the expectation
            var activeLawyerRecords = Lawyers.Where(l => l.Active == true).ToList();

            // test that GetAll() fetches the Lawyers list
            using (LawyerService lawyerService = new LawyerService(MockContext.Object))
            {
                var actual = lawyerService.GetAll();
                CollectionAssert.AreEqual(activeLawyerRecords, actual.ToList());
            }
        }

        [TestMethod]
        public void GetAllWithParameters()
        {
            // setting up the expectation
            var defaultParams = Doubles.GetParameters(); // nothing included and no search by name or surname
            var defaultParamsExpectation = Lawyers.Where(l => l.Active == true).ToList();

            var includeGender = Doubles.GetParameters(IncludeGender: true);
            var includeGenderExpectation = Lawyers.Where(l => l.Active == true).Select(l =>
            {
                l.Gender = Doubles.GetById(Genders, l.GenderRefId);
                return l;
            }).ToList();

            var includeTitle = Doubles.GetParameters(IncludeTitle: true);
            var includeTitleExpectation = Lawyers.Where(l => l.Active == true).Select(l =>
            {
                if (l.TitleRefId.HasValue)
                {
                    l.Title = Doubles.GetById(Titles, l.TitleRefId.Value);
                }
                return l;
            }).ToList();

            var includeInactive = Doubles.GetParameters(IncludeInactive: true);
            var includeInactiveExpectation = Lawyers.ToList();

            var searchNameInDefaults = Doubles.GetParameters(Name: "Elp");
            var searchNameInDefaultsExpectation = Lawyers.Where(l => l.Active == true && l.Name.ToLower().Contains("Elp".ToLower())).ToList();

            var searchSurnameInDefaults = Doubles.GetParameters(Surname: "Tromp");
            var searchSurnameInDefaultsExpectation = Lawyers.Where(l => l.Active == true && l.Surname.ToLower().Contains("Tromp".ToLower())).ToList();

            var searchNameInAll = Doubles.GetParameters(Name: "Elp", IncludeInactive: true);
            var searchNameInAllExpectation = Lawyers.Where(l => l.Name.ToLower().Contains("Elp".ToLower())).ToList();

            var searchSurnameInAll = Doubles.GetParameters(Surname: "Tromp", IncludeInactive: true);
            var searchSurnameInAllExpectation = Lawyers.Where(l => l.Surname.ToLower().Contains("Tromp".ToLower())).ToList();

            using (LawyerService lawyerService = new LawyerService(MockContext.Object))
            {
                var actual = lawyerService.GetAll(defaultParams);
                CollectionAssert.AreEqual(defaultParamsExpectation, actual.ToList());

                actual = lawyerService.GetAll(includeGender);
                CollectionAssert.AreEqual(includeGenderExpectation, actual.ToList());

                actual = lawyerService.GetAll(includeTitle);
                CollectionAssert.AreEqual(includeTitleExpectation, actual.ToList());

                actual = lawyerService.GetAll(includeInactive);
                CollectionAssert.AreEqual(includeInactiveExpectation, actual.ToList());

                actual = lawyerService.GetAll(searchNameInDefaults);
                CollectionAssert.AreEqual(searchNameInDefaultsExpectation, actual.ToList());

                actual = lawyerService.GetAll(searchSurnameInDefaults);
                CollectionAssert.AreEqual(searchSurnameInDefaultsExpectation, actual.ToList());

                actual = lawyerService.GetAll(searchNameInAll);
                CollectionAssert.AreEqual(searchNameInAllExpectation, actual.ToList());

                actual = lawyerService.GetAll(searchSurnameInAll);
                CollectionAssert.AreEqual(searchSurnameInAllExpectation, actual.ToList());
            }
        }

        [TestMethod]
        public void AddALawyer()
        {
            // setting up the expectations
            Lawyer newLawyer = new Lawyer
            {
                Name = "Neos",
                Surname = "Surneos",
                Initials = "NS",
                DateOfBirth = new DateTime(2000, 1, 1),
                Email = "test@test.com",
                GenderRefId = 1,
                TitleRefId = 1
            };
            CollectionAssert.DoesNotContain(Lawyers, newLawyer);

            using (LawyerService lawyerService = new LawyerService(MockContext.Object))
            {
                // test add new lawyer
                lawyerService.Add(newLawyer);
                MockLawyersSet.Verify(m => m.Add(It.IsAny<Lawyer>()), Times.Once);
                CollectionAssert.Contains(Lawyers, newLawyer);
                Assert.AreEqual(true, newLawyer.Active);

                // also test that findByName fetches the new lawyer
                Assert.AreEqual(newLawyer, lawyerService.GetAll(Doubles.GetParameters(Name: newLawyer.Name)).FirstOrDefault());

                // same goes with the surname
                Assert.AreEqual(newLawyer, lawyerService.GetAll(Doubles.GetParameters(Surname: newLawyer.Surname)).FirstOrDefault());
            }
        }

        [TestMethod]
        public void UpdateALawyer()
        {
            // setting up the expectations

            // current lowyer is the one to be updated.
            Lawyer currentLawyer = Lawyers.ElementAt(1);

            // create the updated lawyer. This will become the new "Active" instace.
            Lawyer updatedLawyer = new Lawyer
            {
                Name = "Updated",
                Surname = "Surneos",
                Initials = "US",
                DateOfBirth = new DateTime(2000, 1, 1),
                Email = "test@test.com",
                GenderRefId = 1,
                TitleRefId = 1
            };

            // before any change we need to make sure the DB contains the current instance
            // and does not contain the updated (new) one.
            CollectionAssert.DoesNotContain(Lawyers, updatedLawyer);
            CollectionAssert.Contains(Lawyers, currentLawyer);

            using (LawyerService lawyerService = new LawyerService(MockContext.Object))
            {
                // update the lawyer
                lawyerService.Update(currentLawyer.Id, updatedLawyer);

                // validate .Add is called only once
                MockLawyersSet.Verify(m => m.Add(It.IsAny<Lawyer>()), Times.Once);

                // make sure both instances are now in DB
                CollectionAssert.Contains(Lawyers, updatedLawyer);
                CollectionAssert.Contains(Lawyers, currentLawyer);

                // validate old instance is not Active
                Assert.AreEqual(false, currentLawyer.Active);

                // verify the new instance is Active
                Assert.AreEqual(true, updatedLawyer.Active);

                // we recall the new lawyer instace with his new surname
                Lawyer fetched2 = lawyerService.GetAll(Doubles.GetParameters(Surname: updatedLawyer.Surname)).FirstOrDefault();
                Assert.AreEqual(updatedLawyer, fetched2);

                // verify that we cannot find the lawyer with his old surname
                Lawyer oldLawyer = lawyerService.GetAll(Doubles.GetParameters(Surname: currentLawyer.Surname)).FirstOrDefault();
                Assert.IsNull(oldLawyer);

                // verify that we cannot update the inactive instace
                try
                {
                    lawyerService.Update(currentLawyer.Id, new Lawyer());
                }
                catch (Exception ex)
                {
                    Assert.IsInstanceOfType(ex, typeof(HttpRequestValidationException));
                    Assert.AreEqual(string.Format("the lawyer record with id: '{0}' is archived.", currentLawyer.Id), ex.Message);
                }

                // verify we cannot update a non existing lawyer
                try
                {
                    lawyerService.Update(404, new Lawyer());
                }
                catch (Exception ex)
                {
                    Assert.IsInstanceOfType(ex, typeof(NullReferenceException));
                    Assert.AreEqual("Lawyer not found.", ex.Message);
                }
            }
        }
    }
}