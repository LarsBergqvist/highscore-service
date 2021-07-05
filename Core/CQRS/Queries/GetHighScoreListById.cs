using Core.Models;
using Core.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.CQRS.Queries
{
    public class GetHighScoreListById
    {
        public class Request: IRequest<HighScoreListReadModel>
        {
            public Request(string id) => Id = id;
            public string Id { get; init; }
        }

        public class Handler : IRequestHandler<Request, HighScoreListReadModel>
        {
            private readonly IHighScoreRepository _repository;
            public Handler(IHighScoreRepository repository)
            {
                _repository = repository;
            }

            public async Task<HighScoreListReadModel> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _repository.GetHighScoreList(request.Id);
            }
        }
    }
}
