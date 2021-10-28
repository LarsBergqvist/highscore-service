using Core.Models;
using Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.CQRS.Commands
{
    public class AddGameResult
    {
        public class Response
        {
            public bool NoMatchingListId { get; init; }
        }

        public class Request : IRequest<Response>
        {
            public Request(string highScoreListId, GameResult gameResult)
            {
                HighScoreListId = highScoreListId;
                GameResult = gameResult;
            }
            public string HighScoreListId { get; init; }
            public GameResult GameResult { get; init; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IHighScoreRepository _repository;
            public Handler(IHighScoreRepository repository)
            {
                _repository = repository;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var list = await _repository.GetHighScoreList(request.HighScoreListId);
                if (list == null)
                {
                    return new Response { NoMatchingListId = true };
                }
                await _repository.AddGameResultToHighScoreList(request.HighScoreListId, request.GameResult);
                return new Response { NoMatchingListId = false };
            }
        }
    }
}
