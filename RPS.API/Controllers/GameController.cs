using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPS.Application.Dto.ResponsesAbstraction;
using RPS.Application.Dto.Room;
using RPS.Application.Features.MongoDb.GetPlayerRating;
using RPS.Application.Features.Room.AddRoom;
using RPS.Application.Features.Room.GetAllRooms;
using RPS.Application.Features.Room.GetRoomById;
using RPS.Application.Features.Room.JoinRoom;
using RPS.Application.Features.User.GetUserById;
using RPS.Shared.Mongo;

namespace RPS.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GameController : Controller
{
    private readonly IMediator _mediator;

    public GameController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("/games/getAllRooms/{page}")]
    public async Task<JsonResult> GetGamesByPage([FromRoute] int page)
    {
        return Json(await _mediator.Send(new GetAllRoomsQuery(page)));
    }
    
    [HttpGet]
    [Route("/games/{roomId}")]
    public async Task<JsonResult> GetGameById([FromRoute] string roomId)
    {
        return Json(await _mediator.Send(new GetRoomByIdQuery(roomId)));
    }

    [HttpPost]
    [Route("/games/createGame")]
    public async Task<JsonResult> CreateGame([FromBody] CreateRoomDto model, CancellationToken cancellationToken)
    {
        try
        {
            var curUserId = User.Claims.FirstOrDefault(c => c.Type == "Id")!.Value;
            var curUserResult = await _mediator.Send(
                new GetUserByIdQuery(curUserId),
                cancellationToken);
            if (!curUserResult.IsSuccess)
                return Json(new FailResponse(false, "User not found", 404));
            
            var res = await _mediator.Send(
                new AddRoomCommand(
                    curUserResult.Value!,
                    model.MaxRating), 
                cancellationToken);
            if (!res.IsSuccess)
                return Json(new FailResponse(false, res.Error!, 500));
            return Json(new RoomGetResponse
            {
                IsSuccess = true,
                FirstPlayerName = curUserResult.Value!.UserName!,
                RoomId = res.Value!.Id
            });
        }
        catch (Exception e)
        {
            return Json(new FailResponse(false, e.Message, 500));
        }
    }

    [HttpPost]
    [Route("/games/joinGame/{roomId}")]
    public async Task<JsonResult> JoinGame([FromRoute] string roomId)
    {
        try
        {
            var room = await _mediator.Send(new GetRoomByIdQuery(roomId));
        
            if (!room.IsSuccess)
                return Json(new FailResponse(false, "Room not found", 404));

            var user = await _mediator.Send(
                new GetUserByIdQuery(User.Claims.FirstOrDefault(c => c.Type == "Id")!.Value));
            
            if (!user.IsSuccess)
                return Json(new FailResponse(false, "You aren't player", 404));

            // var userRating = 
            //     (await _mediator.Send(new GetPlayerRatingMongoQuery(user.Value!.UserName!))).Value;

            var userRating = new PlayerRatingDto
            {
                Id = "",
                Key = "",
                Rating = 0
            };
            
            if (room.Value!.SecondPlayerId is null && room.Value!.MaxRating >= userRating!.Rating)
                await _mediator.Send(new JoinRoomCommand(roomId, user.Value!));

            return Json(room.Value);
        }
        catch (Exception e)
        {
            return Json(new FailResponse(false, e.Message, 500));
        }
    }
}