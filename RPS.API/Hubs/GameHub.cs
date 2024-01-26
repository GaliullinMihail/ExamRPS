using MassTransit;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using RPS.Application.Features.Room.DeleteRoom;
using RPS.Application.Features.Room.GetRoomById;
using RPS.Application.Features.User.GetUserById;
using RPS.Domain.Enums;
using RPS.Shared.GameResult;

namespace RPS.API.Hubs
{
    public class GameHub : Hub
    {
        private readonly IBus _bus;
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<string>> _connections;

        public GameHub(IMediator mediator, IBus bus)
        {
            _mediator = mediator;
            _bus = bus;
            _connections = new();
        }

        public async Task ConnectToRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            if (!_connections.ContainsKey(roomId))
                _connections[roomId] = new List<string>();
            _connections[roomId].Add(Context.ConnectionId);
            
            if (_connections[roomId].Count == 2)
                await Clients.Group(roomId).SendAsync("StartGame");
        }

        public async Task LeaveFromRoom(string userId, string roomId)
        {
            var roomResult = await _mediator.Send(new GetRoomByIdQuery(roomId));

            if (!roomResult.IsSuccess)
                return;
            var room = roomResult.Value;
            if (room!.FirstPlayerId != userId && room.SecondPlayerId != userId)
                return;

            foreach (var connectionId in _connections[roomId])
            {
                await Groups.RemoveFromGroupAsync(connectionId, roomId);
            }
            
            _connections.Remove(roomId);

            await _mediator.Send(new DeleteRoomCommand(roomId));
        }
        
        public async Task SendGameMessage(
            string senderUserId, 
            string sign,
            string roomId)
        {
            var room = await _mediator.Send(new GetRoomByIdQuery(roomId));
            
            var sender = await _mediator.Send(new GetUserByIdQuery(senderUserId));
            
            if (!sender.IsSuccess || !room.IsSuccess || !Enum.TryParse<GameSigns>(sign, out _))
                return;
            
            await Clients.Group(roomId).SendAsync("ReceiveGameMessage", senderUserId, 
                sign);
        }
        
        public async Task SendResultMessage(
            int result,
            string roomId)
        {
            var roomResult = await _mediator.Send(new GetRoomByIdQuery(roomId));

            var room = roomResult.Value;

            var firstPlayer =
                (await _mediator.Send(new GetUserByIdQuery(room!.FirstPlayerId))).Value;
            var secondPlayer =
                (await _mediator.Send(new GetUserByIdQuery(room.SecondPlayerId!))).Value;
            GameResultDto gameResult;
            switch (result)
            {
                case -1:
                    gameResult = new GameResultDto
                    {
                        Draw = false,
                        WinnerNickName = secondPlayer!.UserName!,
                        LooserNickName = firstPlayer!.UserName!
                    };
                    await _bus.Send(gameResult);
                    break;
                case 0:
                    gameResult = new GameResultDto
                    {
                        Draw = true,
                        WinnerNickName = secondPlayer!.UserName!,
                        LooserNickName = firstPlayer!.UserName!
                    };
                    await _bus.Send(gameResult);
                    break;
                default:
                    gameResult = new GameResultDto
                    {
                        Draw = false,
                        WinnerNickName = firstPlayer!.UserName!,
                        LooserNickName = secondPlayer!.UserName!
                    };
                    await _bus.Send(gameResult);
                    break;
            }
            await Clients.Group(roomId).SendAsync("ReceiveResultMessage", firstPlayer!.UserName,
                secondPlayer!.UserName,
                result);
        }
        
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}