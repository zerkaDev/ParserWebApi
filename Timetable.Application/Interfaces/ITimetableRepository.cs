using System.Collections.Generic;
using System.Threading.Tasks;
using Timetable.Domain;

namespace Timetable.Application.Interfaces
{
    public interface ITimetableRepository
    {
        Task<Group> GetGroup(string groupName);
        Task<IEnumerable<Week>> GetWeeksWithTimetable(string group);
        Task<IEnumerable<Institute>> GetAllInstitutes();
        Task<IEnumerable<Group>> GetAllGroups();
        public Task Save();
    }
}
