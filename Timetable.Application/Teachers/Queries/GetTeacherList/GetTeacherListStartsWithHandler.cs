using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timetable.Application.Interfaces;

namespace Timetable.Application.Teachers.Queries.GetTeacherList
{
    public class GetTeacherListStartsWithHandler : IRequestHandler<GetTeacherListStartsWith, TeacherListStartsWithVm>
    {
        private readonly ITimetableDbContext _dbContext;
        private readonly IMapper _mapper;
        public GetTeacherListStartsWithHandler(ITimetableDbContext dbContext,
            IMapper mapper) => (_dbContext, _mapper) = (dbContext, mapper);
        public async Task<TeacherListStartsWithVm> Handle(GetTeacherListStartsWith request, CancellationToken cancellationToken)
        {
            var teachers = await _dbContext.Teachers
                .Where(e => e.FullName.ToLower().
                    StartsWith(request.Symbols.ToLower()))
                .ProjectTo<TeacherLookupDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new TeacherListStartsWithVm { Teachers = teachers };
        }
    }
}
