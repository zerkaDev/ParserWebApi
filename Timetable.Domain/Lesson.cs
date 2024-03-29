﻿using System;

namespace Timetable.Domain
{
    public class Lesson
    {
        /// <summary>
        /// Уникальный ид пары
        /// </summary>
        public Guid Id { get; set; }
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
        public Guid? TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        /// <summary>
        /// Аудитория
        /// </summary>
        public string Audience { get; set; }
        public Guid OneDayTimetableId { get; set; }
        public OneDayTimetable OneDayTimetable { get; set; }
    }
}
