using HtmlAgilityPack;
using NUnit.Framework;
using System.Collections.Generic;
using Timetable.Application;
using Timetable.Domain;
using Timetable.Domain.Comparers;

namespace Timetable.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            //TimetableParser tp = new TimetableParser();
            //Assert.That(tp.GetAllCoursesOfInstitute(516).Result.Count, Is.EqualTo(6));
            //Assert.AreEqual(tp.GetAllInstitutes().Count, 17);
            //Assert.AreEqual(tp.GetAllGroupsOfCourse(516, 3).Count, 13);
            //Assert.AreEqual(tp.GetAllWeeksOfGroup(516, 3, "20-��-��2").Count, 2);
            //Assert.AreEqual(tp.GetAllWeeksOfGroup(516, 3, "20-��-��2")[0].Parity, "�������� ������");

            //Assert.AreEqual(tp.GetAllDaysOfWeek(516, 3, "20-��-��2", true).Count, 6);
            //Assert.AreEqual(tp.GetAllDaysOfWeek(516, 3, "20-��-��2", false).Count, 6);
            //Assert.AreEqual(tp.GetAllDaysOfWeek(516, 3, "20-��-��2", true)[5].Day, "�������");
            //Assert.AreEqual(tp.GetAllDaysOfWeek(495, 4, "19-��-��3", false).Count, 5);

            //Assert.AreEqual(tp.GetLessonsOfThisDay(516, 3, "20-��-��2", true, 3).Result.Count, 3);
            //Assert.AreEqual(tp.GetLessonsOfThisDay(516, 3, "20-��-��2", true, 2).Count, 4);

            var unsortList = new List<Lesson>()
            {
                new Lesson()
                {
                    Id = 1,
                    Number = "7 ����"
                },
                new Lesson()
                {
                    Id = 2,
                    Number = "6 ����"
                },new Lesson()
                {
                    Id=3,
                    Number = "5 ����"
                },new Lesson()
                {
                    Id=4,
                    Number = "4 ����"
                }
            };
            var unsortList2 = new List<Lesson>(unsortList);
            unsortList.Sort(new LessonComparer());
            Assert.That(unsortList, Is.Not.EqualTo(unsortList2));


        }
    }
}