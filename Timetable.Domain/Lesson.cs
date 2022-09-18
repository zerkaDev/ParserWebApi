using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Domain
{
    public class Lesson
    {
        /// <summary>
        /// Уникальный ид пары
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Название предмета
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Какая пара (3 пара, 4)
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Время пары
        /// </summary>
        public string LessonDuration { get; set; }
        /// <summary>
        /// Тип пары (лекция, лаба, практика)
        /// </summary>
        public string TypeOfLesson { get; set; }
        /// <summary>
        /// Преподователь
        /// </summary>
        public string Teacher { get; set; }
        /// <summary>
        /// Аудитория
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// Период обучения по данному предмету (с 1 по 18 неделю)
        /// </summary>
        public string Period { get; set; }
        /// <summary>
        /// В лекционном потоке или нет
        /// </summary>
        public bool IsLectureStream { get; set; } = true;
        /// <summary>
        /// Примечание
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// Процент от группы
        /// </summary>
        public string PercentOfGroup { get; set; }
        public int OneDayTimetableId { get; set; }
        public OneDayTimetable OneDayTimetable { get; set; }
    }
}
