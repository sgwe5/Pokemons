﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pokemons.Core.Enums.Battles;
using Pokemons.Core.Providers.TimeProvider;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.UnitOfWork;

namespace Pokemons.DataLayer.Database.Repositories.BattleRepos;

public class BattleDatabaseRepository : IBattleDatabaseRepository
{
    public BattleDatabaseRepository(AppDbContext context, IUnitOfWork unitOfWork, ITimeProvider timeProvider)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _timeProvider = timeProvider;
    }

    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITimeProvider _timeProvider;

    public async Task<Battle?> GetBattleByPlayerId(long playerId) =>
        await _context.Battles
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.PlayerId == playerId);

    public async Task<Battle> CreateBattleForPlayer(Battle battle)
    {
        await _unitOfWork.BeginTransaction();
        var createdBattle = await _context.AddAsync(battle);
        await _unitOfWork.CommitTransaction();
        
        return createdBattle.Entity;
    }

    public async Task UpdateBattle(Battle battle)
    {
        await _unitOfWork.BeginTransaction();
        _context.Update(battle);
        await _unitOfWork.CommitTransaction();
    }
}