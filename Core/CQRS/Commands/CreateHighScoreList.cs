using Core.Models;
using Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.CQRS.Commands
{
    public class CreateHighScoreList
    {
        public class Request : IRequest<HighScoreListReadModel>
        {
            public Request(HighScoreListWriteModel input)
            {
                Input = input;
            }
            public HighScoreListWriteModel Input { get; init; }
        }

        public class Handler : IRequestHandler<Request,HighScoreListReadModel>
        {
            private readonly IHighScoreRepository _repository;
            public Handler(IHighScoreRepository repository)
            {
                _repository = repository;
            }

            Task<HighScoreListReadModel> IRequestHandler<Request, HighScoreListReadModel>.Handle(Request request, CancellationToken cancellationToken)
            {
                var result = _repository.CreateHighScoreList(request.Input);
                return result;
            }
        }
    }
}
