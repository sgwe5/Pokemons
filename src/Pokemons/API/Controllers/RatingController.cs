﻿using Microsoft.AspNetCore.Mvc;
using Pokemons.API.Handlers;

namespace Pokemons.API.Controllers;

[ApiController]
[Route("rating/")]
public class RatingController : ControllerBase
{
    public RatingController(IRatingHandler handler)
    {
        _handler = handler;
    }

    private readonly IRatingHandler _handler;

    private async Task<IResult> GetPlayerUserInfo()
    {
        var id = (long)HttpContext.Items["UserId"]!;
        
        return Results.Ok();
    }

    [HttpGet("leagueRatingInfo")]
    public async Task<IResult> GetLeagueRatingInfo([FromQuery] int leagueType, [FromQuery] int offset)
    {
        var id = (long)HttpContext.Items["UserId"]!;

        var result = await _handler.GetLeagueRating(leagueType, offset);
        
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
}