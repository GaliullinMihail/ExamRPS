using RPS.Domain.Entities;
using RPS.Infrastructure.Database;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RPS.Application.Dto.Chat;
using RPS.Application.Features.Room.SaveMessage;
using RPS.Domain.Enums;

namespace RPS.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IBus _bus;
        private readonly IMediator _mediator;

        public ChatHub(ApplicationDbContext dbContext, UserManager<User> userManager, IMediator mediator, IBus bus)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mediator = mediator;
            _bus = bus;
        }
        
        // public async Task GetGroupMessages(string roomName)
        // {
        //     await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        //
        //     var room = _dbContext.Rooms.FirstOrDefault(r => r.Id == roomName);
        //
        //     if (room is null)
        //         return;
        //
        //     var messages = await _dbContext.Messages.Where(m => m.RoomId == room.Id).OrderBy(m => m.Timestamp).ToListAsync();
        //     
        //     foreach (var message in messages)
        //     {
        //         await Clients.Client(Context.ConnectionId).SendAsync("ReceivePrivateMessage", 
        //             await GetUserName(message.SenderId), 
        //             message.Content);
        //     }
        // }

        // private async Task<string?> GetUserName(string id)
        // {
        //     return (await _userManager.FindByIdAsync(id))?.UserName;
        // }
        
        public async Task SendGameMessage(string senderUserName, 
            string sign,
            string groupName)
        {
            var room = _dbContext.Rooms.FirstOrDefault(r => r.Id == groupName);
            
            var sender = await _userManager.FindByNameAsync(senderUserName);

            ;
            
            if (sender is null || room is null || !Enum.TryParse<GameSigns>(sign, out _))
                return;
            
            await Clients.Group(groupName).SendAsync("ReceiveGameMessage", senderUserName, 
                sign);
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