using Core.Models;
using Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Helpers;

namespace Core.CQRS.Queries;

public static class GetCalculatedPositionInList
{
    public class Request : IRequest<ResultListPosition>
    {
        public Request(string id, int score)
        {
            Id = id;
            Score = score;
        }

        public string Id { get; init; }

        public int Score { get; }
    }


    public class Handler : IRequestHandler<Request, ResultListPosition>
    {
        private readonly IHighScoreRepository _repository;
        private readonly IGameResultHelper _helper;
        public Handler(IHighScoreRepository repository, IGameResultHelper helper)
        {
            _repository = repository;
            _helper = helper;
        }

        public async Task<ResultListPosition> Handle(Request request, CancellationToken cancellationToken)
        {
            var list = await _repository.GetHighScoreList(request.Id);
            if (list == null)
            {
                return new ResultListPosition(-1, ResultStatus.NotInList);
            }
            return _helper.GetProposedPositionInList(request.Score, list.Results, list.LowIsBest, list.MaxSize);
        }
    }
}