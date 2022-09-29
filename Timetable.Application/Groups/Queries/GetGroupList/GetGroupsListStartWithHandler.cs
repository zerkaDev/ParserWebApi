using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Timetable.Application.Interfaces;

namespace Timetable.Application.Groups.Queries.GetGroupList
{
    public class GetGroupsListStartWithHandler
        : IRequestHandler<GetGroupsListStartWith, GroupsListStartWithVm>
    {
    
        private readonly ITimetableDbContext _dbContext;
        private readonly IMapper _mapper;
        public GetGroupsListStartWithHandler(ITimetableDbContext dbContext,
            IMapper mapper) => (_dbContext, _mapper) = (dbContext, mapper);

        public async Task<GroupsListStartWithVm> Handle(GetGroupsListStartWith request, CancellationToken cancellationToken)
        {
            var groupsStartWithSymblog = await _dbContext.Groups
                .Where(g => g.Name.StartsWith(request.Symbols))
                .ProjectTo<GroupLookupDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new GroupsListStartWithVm { Groups = groupsStartWithSymblog };
        }
    }
}
