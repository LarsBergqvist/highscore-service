using Core.Models;
using Core.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.CQRS.Queries;

public static class GetAllHighScoreLists
{
    public class Request: IRequest<IEnumerable<HighScoreListReadModel>>
    {
    }

    public class Handler : IRequestHandler<Request, IEnumerable<HighScoreListReadModel>>
    {
        private readonly IHighScoreRepository _repository;
        public Handler(IHighScoreRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<HighScoreListReadModel>> Handle(Request request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllHighScoreLists();
        }
    }
}