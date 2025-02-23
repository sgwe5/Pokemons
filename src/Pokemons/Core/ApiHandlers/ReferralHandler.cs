﻿using Pokemons.API.CallResult;
using Pokemons.API.Dto.Responses;
using Pokemons.API.Handlers;
using Pokemons.Core.Services.ReferralService;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.Core.ApiHandlers;

public class ReferralHandler : IReferralHandler
{
    public ReferralHandler(IReferralService referralService)
    {
        _referralService = referralService;
    }

    private readonly IReferralService _referralService;
    
    public async Task<CallResult<Friends>> GetReferralInfo(long playerId)
    {
        var refs = await _referralService.GetReferrals(playerId);
        var response = refs.Select(value => new FriendsItem
            {
                Id = value.Player.Id,
                Name = value.Player.Name,
                Surname = value.Player.Surname,
                DefeatedEntities = value.Player.DefeatedEntities,
                Level = value.Player.Level,
                PhotoUrl = value.Player.PhotoUrl,
                LeagueType = (int)value.Player.Rating.LeagueType,
                Inline = value.Inline,
                ProfitNumber = 0
            })
            .ToList();

        return CallResult<Friends>.Success(new Friends
        {
            List = response,
            Total = response.Count
        });
    }
}