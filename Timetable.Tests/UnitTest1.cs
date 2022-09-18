using HtmlAgilityPack;
using NUnit.Framework;
using Timetable.Application;

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
            TimetableParser tp = new TimetableParser();
            //Assert.That(tp.GetAllCoursesOfInstitute(516).Result.Count, Is.EqualTo(6));
            //Assert.AreEqual(tp.GetAllInstitutes().Count, 17);
            //Assert.AreEqual(tp.GetAllGroupsOfCourse(516, 3).Count, 13);
            //Assert.AreEqual(tp.GetAllWeeksOfGroup(516, 3, "20-КБ-ПИ2").Count, 2);
            //Assert.AreEqual(tp.GetAllWeeksOfGroup(516, 3, "20-КБ-пи2")[0].Parity, "Нечетная неделя");

            //Assert.AreEqual(tp.GetAllDaysOfWeek(516, 3, "20-КБ-ПИ2", true).Count, 6);
            //Assert.AreEqual(tp.GetAllDaysOfWeek(516, 3, "20-КБ-ПИ2", false).Count, 6);
            //Assert.AreEqual(tp.GetAllDaysOfWeek(516, 3, "20-КБ-ПИ2", true)[5].Day, "Суббота");
            //Assert.AreEqual(tp.GetAllDaysOfWeek(495, 4, "19-НБ-НД3", false).Count, 5);

            Assert.AreEqual(tp.GetLessonsOfThisDay(516, 3, "20-КБ-ПИ2", true, 3).Result.Count, 3);
            //Assert.AreEqual(tp.GetLessonsOfThisDay(516, 3, "20-КБ-ПИ2", true, 2).Count, 4);

        }
    }
}