using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Model;

namespace WebAPI.Tests
{
    public static class Doubles
    {
        public static T GetById<T>(List<T> list, int id) where T : Entity
        {
            return list.Where(l => l.Id == id).FirstOrDefault();
        }

        public static List<Gender> Genders()
        {
            return new List<Gender>
            {
                new Gender
                {
                    Id = 1,
                    Description = "Gender One",
                    DescriptionM = "GO",
                    DateRecStarted = new DateTime(1980, 1, 1),
                    LoginRecStarted = new DateTime(2001, 2, 16)
                },
                new Gender
                {
                    Id = 2,
                    Description = "Gender Two",
                    DescriptionM = "GT",
                    DateRecStarted = new DateTime(1986, 5, 7),
                    LoginRecStarted = new DateTime(1999, 2, 10)
                },
            };
        }

        public static List<Title> Titles()
        {
            return new List<Title> {
                new Title
                {
                    Id = 1,
                    Description = "Title One",
                    DescriptionM = "TO",
                    DateRecStarted = new DateTime(1986, 5, 7),
                    LoginRecStarted = new DateTime(1998, 10, 10)
                },
                new Title
                {
                    Id = 2,
                    Description = "Title Two",
                    DescriptionM = "TT",
                    DateRecStarted = new DateTime(1976, 7, 6),
                    LoginRecStarted = new DateTime(2000, 1, 10)
                }
            };
        }

        public static List<Lawyer> Lawyers(List<Gender> genders, List<Title> titles)
        {
            return new List<Lawyer>
            {
                new Lawyer {
                    Id = 1,
                    Name = "Elpidoforos",
                    Surname = "Prospathopoulos",
                    DateOfBirth = new DateTime(1981, 9, 16),
                    GenderRefId = genders.FirstOrDefault().Id,
                    TitleRefId = titles.FirstOrDefault().Id,
                    Initials = "EP",
                    Email = "e.prospathopoulos@gmail.com",
                    Active = false,
                },
                new Lawyer {
                    Id = 2,
                    Name = "Elpidoforos",
                    Surname = "Prospathopoulos",
                    DateOfBirth = new DateTime(1981, 9, 16),
                    GenderRefId = genders.FirstOrDefault().Id,
                    TitleRefId = titles.FirstOrDefault().Id,
                    Initials = "EP",
                    Email = "e.prospathopoulos@gmail.com",
                    Active = true,
                },
                new Lawyer {
                    Id = 3,
                    Name = "Persefoni",
                    Surname = "Trompeta",
                    DateOfBirth = new DateTime(1985, 5, 18),
                    GenderRefId = genders.FirstOrDefault().Id,
                    TitleRefId = titles.FirstOrDefault().Id,
                    Initials = "PT",
                    Email = "p.trompeta@gmail.com",
                    Active = false,
                },
                new Lawyer {
                    Id = 4,
                    Name = "Persefoni Updated",
                    Surname = "Trompeta Updated",
                    DateOfBirth = new DateTime(1985, 5, 18),
                    GenderRefId = genders.FirstOrDefault().Id,
                    TitleRefId = titles.FirstOrDefault().Id,
                    Initials = "PT2",
                    Email = "p.trompeta2@gmail.com",
                    Active = true,
                }
            };
        }

        public static List<Lawyer> Lawyers()
        {
            return Lawyers(Genders(), Titles());
        }

        public static List<Lawyer> Lawyers(List<Gender> genders)
        {
            return Lawyers(genders, Titles());
        }

        public static List<Lawyer> Lawyers(List<Title> titles)
        {
            return Lawyers(Genders(), titles);
        }

        public static LawyerSearchParameters GetParameters(bool IncludeGender = false, bool IncludeTitle = false, bool IncludeInactive = false, string Name = null, string Surname = null)
        {
            return new LawyerSearchParameters
            {
                IncludeGender = IncludeGender,
                IncludeTitle = IncludeTitle,
                IncludeInactive = IncludeInactive,
                Name = Name,
                Surname = Surname
            };
        }
    }
}