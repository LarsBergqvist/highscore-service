using Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.CQRS.Commands
{
    public class AddGameResult
    {
        public class Response
        {
            public int? PositionInList { get; init; }
            public bool NoMatchingListId { get; init; }
        }

        public class Request : IRequest<Response>
        {
            public Request(Guid highScoreListId, string userName, int result)
            {
                HighScoreListId = highScoreListId;
                UserName = userName;
                Result = result;
            }
            public Guid HighScoreListId { get; init; }
            public string UserName { get; init; }
            public int Result { get; init; }
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
                await _repository.AddGameResultToHighScoreList(request.HighScoreListId, new Models.GameResult(request.UserName, request.Result));
                return new Response { NoMatchingListId = false };
            }
        }
    }
}
