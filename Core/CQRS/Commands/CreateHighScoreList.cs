using Core.Models;
using Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.CQRS.Commands
{
    public class CreateHighScoreList
    {
        public class Request : IRequest<HighScoreList>
        {
            public Request(HighScoreListInput input)
            {
                Input = input;
            }
            public HighScoreListInput Input { get; init; }
        }

        public class Handler : IRequestHandler<Request,HighScoreList>
        {
            private readonly IHighScoreRepository _repository;
            public Handler(IHighScoreRepository repository)
            {
                _repository = repository;
            }

            Task<HighScoreList> IRequestHandler<Request, HighScoreList>.Handle(Request request, CancellationToken cancellationToken)
            {
                var result = _repository.CreateHighScoreList(request.Input);
                return result;
            }
        }
    }
}
