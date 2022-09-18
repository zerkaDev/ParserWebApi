using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Domain
{
    //У института есть всех n-курсники данного института, а у всех n-курсников есть группы
   public class Course
    {
        /// <summary>
        /// Идентификатор. [лучше через гуиды]
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Номер курса (1-6)
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Для EF Core [должен совпадать с айди находящимися на сайте университета]
        /// </summary>
        public int InstituteId { get; set; } 
        /// <summary>
        /// Ссылка на институт
        /// </summary>
        public Institute Institute { get; set; }
        /// <summary>
        /// Список груп принадлежащих данному курсу
        /// </summary>
        public List<Group> Groups { get; set; }
    }
}
