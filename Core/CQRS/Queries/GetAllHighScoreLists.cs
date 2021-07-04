using Core.Models;
using Core.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.CQRS.Queries
{
    public class GetAllHighScoreLists
    {
        public class Request: IRequest<IEnumerable<HighScoreList>>
        {
        }

        public class Handler : IRequestHandler<Request, IEnumerable<HighScoreList>>
        {
            private readonly IHighScoreRepository _repository;
            public Handler(IHighScoreRepository repository)
            {
                _repository = repository;
            }

            public async Task<IEnumerable<HighScoreList>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _repository.GetAllHighScoreLists();
            }
        }
    }
}
