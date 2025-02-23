﻿using Pokemons.API.Dto.Requests;
using Pokemons.Core.Providers.TimeProvider;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.MasterRepositories.BattleRepository;
using Pokemons.DataLayer.MasterRepositories.PlayerRepository;
using PokemonsDomain.MessageBroker.Models;

namespace Pokemons.Core.Services.PlayerService;

public class PlayerService : IPlayerService
{
    public PlayerService(IPlayerRepository playerRepository, ITimeProvider timeProvider, 
        IBattleRepository battleRepository)
    {
        _playerRepository = playerRepository;
        _timeProvider = timeProvider;
        _battleRepository = battleRepository;
    }

    private readonly IPlayerRepository _playerRepository;
    private readonly IBattleRepository _battleRepository;
    private readonly ITimeProvider _timeProvider;

    public async Task<(int, int)> CommitDamage(long playerId, int taps)
    {
        var battle = await _battleRepository.GetPlayerBattle(playerId);
        var player = await GetPlayer(playerId);
        if (player is null) return (0, 0);

        var energy = GetEnergy(player);
        player.CurrentEnergy += energy;
        player.CurrentEnergy = player.CurrentEnergy > player.Energy ? player.Energy : player.CurrentEnergy;
        
        var damage = player.DamagePerClick * taps;
        damage = damage > player.CurrentEnergy ? player.CurrentEnergy : damage;
        player.CurrentEnergy -= damage;
        player.LastCommitDamageTime = _timeProvider.Now();
        
        player.Taps += taps;
        player.TotalDamage += damage;

        if (battle.IsGold)
            player.GoldBalance += damage;
        else
            player.Balance += damage;

        await _playerRepository.FastUpdate(player);
        
        return (damage, player.DefeatedEntities);
    }

    public async Task<(int, int)> UseSuperCharge(long playerId)
    {
        var player = await _playerRepository.GetPlayerById(playerId);
        if (player is null) return (0, 0);

        if (!CanUseSuperCharge(player)) return (0, 0);
        
        player.LastSuperChargeActivatedTime = _timeProvider.Now();
        await _playerRepository.FastUpdate(player);
        return (player.SuperCharge, player.DefeatedEntities);
    }

    public async Task<Player?> GetPlayer(long userId)
    {
        var player = await _playerRepository.GetPlayerById(userId);
        if (player is null) return null;

        player.CurrentEnergy += GetEnergy(player);
        return player;
    }

    public async Task<Player> CreatePlayer(long userId, CreateUserModel dto) =>
        await _playerRepository.CreatePlayer(userId, dto);

    public async Task<bool> IsPlayerExist(long playerId) =>
        await _playerRepository.GetPlayerById(playerId) is not null;

    public async Task Save(long playerId) =>
        await _playerRepository.Save(playerId);

    public async Task<int> EntityDefeated(long playerId)
    {
        var player = await _playerRepository.GetPlayerById(playerId);
        if (player is null) return 0;
        player.DefeatedEntities++;
        LevelUpdate(player);
        await _playerRepository.FastUpdate(player);
        return player.DefeatedEntities;
    }

    public async Task UpdatePlayerData(StartSessionDto dto, Player player)
    {
        player.Name = dto.Name;
        player.Surname = dto.Surname;
        player.Username = dto.Username;
        
        await _playerRepository.FastUpdate(player);
    }

    public async Task Update(Player player) =>
        await _playerRepository.Update(player);

    public async Task ConfirmMissionReward(long playerId, int activeMissionReward)
    {
        var player = await _playerRepository.GetPlayerById(playerId);
        if (player is null) return;
        
        player.GoldBalance += activeMissionReward;
        await Update(player);
    }

    private void LevelUpdate(Player player)
    {
        player.Exp += 5000;
        if (player.Exp != 50000) return;
        
        player.Exp = 0;
        player.Level++;
    }

    private int GetEnergy(Player player)
    {
        var energy = player.LastCommitDamageTime != default
            ? (int)(_timeProvider.GetSecondsBetweenDateAndNow(player.LastCommitDamageTime) / 10 * player.EnergyCharge)
            : player.Energy;
        
        return energy > player.Energy ? player.Energy : energy;
    }

    private bool CanUseSuperCharge(Player player)
    {
        var cooldown = (int)(_timeProvider.Now() - player.LastSuperChargeActivatedTime).TotalHours;
        return cooldown * player.SuperChargeCooldown >= 8;
    }
}