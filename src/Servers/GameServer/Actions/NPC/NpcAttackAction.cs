using GameServer.Managers;
using GameServer.Processors;
using RavenfallServer.Providers;
using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.Core;
using Shinobytes.Ravenfall.Data.Entities;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Runtime.CompilerServices;

public class NpcAttackAction : EntityActionInvoker
{
    private static readonly Random random = new Random();
    private const float MeleeRange = 3f;

    private const int PlayerMeleeAttackIntervalMs = 1500;
    private const int NpcMeleeAttackIntervalMs = 1500;
    private const int RetryTimeMs = 250;
    private const int TotalRetryTimeMs = 1500;

    private const string CombatAnimationName = "Attacking";
    //private const string AttackAnimationName = "Attack";

    private const string SkillHealth = "Health";
    private const string SkillAttack = "Attack";
    private const string SkillDefense = "Defense";
    private const string SkillStrength = "Strength";
    private readonly IGameData gameData;
    private readonly IWorldProcessor worldProcessor;
    private readonly IGameSessionManager sessionManager;
    private readonly IPlayerStatsProvider playerStatsProvider;
    private readonly IPlayerStateProvider playerState;

    public NpcAttackAction(
        IGameData gameData,
        IWorldProcessor worldProcessor,
        IGameSessionManager sessionManager,
        IPlayerStatsProvider playerStatsProvider,
        IPlayerStateProvider playerStateProvider)
        : base(10, "Npc Attack")
    {
        this.gameData = gameData;
        this.worldProcessor = worldProcessor;
        this.sessionManager = sessionManager;
        this.playerStatsProvider = playerStatsProvider;
        this.playerState = playerStateProvider;
    }

    public override bool Invoke(
        Player player,
        IEntity obj,
        int attackType)
    {
        if (!(obj is NpcInstance npc))
        {
            return false;
        }

        playerState.SetAttackType(player, attackType);

        return Invoke(player, npc, TimeSpan.Zero, TimeSpan.Zero);
    }

    private bool Invoke(
        Player player,
        NpcInstance npc,
        TimeSpan totalTime,
        TimeSpan deltaTime)
    {
        var attackType = playerState.GetAttackType(player);
        var session = sessionManager.Get(player);
        if (!session.Npcs.States.IsEnemy(player, npc))
        {
            return false;
        }

        if (npc.Health <= 0)
        {
            return false;
        }

        if (!WithinDistance(player, npc, attackType))
        {
            if (totalTime.TotalMilliseconds < TotalRetryTimeMs)
                worldProcessor.SetEntityTimeout(RetryTimeMs, player, npc, Invoke);
            return false;
        }

        if (!ReadyForAttack(player, npc, attackType))
        {
            return false;
        }


        PlayerAttack(player, npc, attackType);
        NpcAttack(player, npc);

        return false;
    }

    private void PlayerAttack(Player player, NpcInstance npc, int attackType)
    {
        var session = sessionManager.Get(player);

        if (playerState.EnterCombat(player, npc))
        {
            worldProcessor.SetCombatState(player, true);
            worldProcessor.SetTarget(player, npc);
            PlayEnterCombatAnimation(player, attackType);
        }

        if (session.Npcs.States.EnterCombat(npc, player))
        {
            worldProcessor.SetCombatState(npc, true);
            worldProcessor.SetTarget(npc, player);
            PlayEnterCombatAnimation(npc);
        }

        playerState.UpdateAttackTime(player, npc);
        worldProcessor.SetEntityTimeout(PlayerMeleeAttackIntervalMs, player, npc, OnPlayerAfflictDamage);
    }

    private void NpcAttack(Player player, NpcInstance npc)
    {
        var session = sessionManager.Get(player);

        var attackType = random.Next(0, 3);

        if (playerState.EnterCombat(player, npc))
        {
            worldProcessor.SetCombatState(player, true);
            PlayEnterCombatAnimation(player, attackType);
        }

        if (session.Npcs.States.EnterCombat(npc, player))
        {
            worldProcessor.SetCombatState(npc, true);
            PlayEnterCombatAnimation(npc);
        }

        PlayAttackAnimation(npc, attackType);

        worldProcessor.SetEntityTimeout(NpcMeleeAttackIntervalMs, player, npc, OnNpcAfflictDamage);
    }

    private bool OnNpcAfflictDamage(
        Player player,
        NpcInstance npc,
        TimeSpan totalTime,
        TimeSpan deltaTime)
    {
        //var session = sessionManager.Get(npc);
        //var attackType = session.Npcs.GetAttackType(npc);
        //if (!session.Npcs.States.InCombat(npc, player))
        //{
        //    ExitCombat(player, npc, attackType);
        //}

        return false;
    }


    private bool OnPlayerAfflictDamage(
        Player player,
        NpcInstance npc,
        TimeSpan totalTime,
        TimeSpan deltaTime)
    {
        var attackType = playerState.GetAttackType(player);

        if (!playerState.InCombat(player, npc))
        {
            ExitCombat(player, npc, attackType);
            return false;
        }

        worldProcessor.AttackTarget(player, attackType);
        PlayAttackAnimation(player, attackType);

        var npcRecord = gameData.GetNpc(npc.NpcId);
        var npcAttributes = gameData.GetAttributes(npcRecord.AttributesId);
        var playerAttributes = gameData.GetAttributes(player.AttributesId);

        var session = sessionManager.Get(player);
        var damage = CalculateDamage(playerAttributes, npcAttributes);

        var trainingSkill = SkillAttack;

        npc.Health -= damage;

        worldProcessor.DamageNpc(player, npc, damage, npc.Health, npcAttributes.Health);

        if (npc.Health <= 0)
        {
            worldProcessor.SetCombatState(npc, false);
            session.Npcs.States.ExitCombat(npc);

            // he ded
            worldProcessor.KillNpc(player, npc);

            // note(zerratar): action that kills the enemy shouldn't be the one responsible for respawning
            //                 this should be moved to a INpcProcessor or similar called from the WorldProcessor Update
            worldProcessor.SetEntityTimeout(gameData.GetNpc(npc.NpcId).RespawnTimeMs, player, npc, OnRespawn);

            var experience = GameMath.CombatExperience(npcRecord.Level);
            int levelsGaiend = playerStatsProvider.AddExperience(player.Id, trainingSkill, experience);
            var newExp = playerStatsProvider.GetExperience(player.Id, trainingSkill);
            var newLevel = playerStatsProvider.GetLevel(player.Id, trainingSkill);

            //var itemDrops = npcProvider.GetItemDrops(npc);
            //foreach (var itemDrop in itemDrops)
            //{
            //    if (random.NextDouble() > itemDrop.DropChance)
            //        continue;
            //    worldProcessor.AddPlayerItem(player, gameData.GetItem(itemDrop.ItemId));
            //}

            worldProcessor.UpdatePlayerStat(player, trainingSkill, newLevel, newExp);

            if (levelsGaiend > 0)
            {
                worldProcessor.PlayerStatLevelUp(player, trainingSkill, levelsGaiend);
            }

            ExitCombat(player, npc, attackType);
            return true;
        }

        PlayerAttack(player, npc, attackType);
        return false;
    }

    private int CalculateDamage(Attributes a, Attributes b)
    {
        var aAttack = a.Attack;
        var aStrength = a.Strength;
        var aWeaponPower = 0;
        var aWeaponAim = 0;
        var aCombatStyle = 0;

        var bStrength = b.Strength;
        var bDefense = b.Defense;
        var bArmorPower = 0;
        var bCombatStyle = 0;

        return (int)GameMath.CalculateDamage(
            aAttack, aStrength, aCombatStyle, aWeaponPower, aWeaponAim,
            bStrength, bDefense, bCombatStyle, bArmorPower, false, true);
    }

    private bool OnRespawn(Player player, NpcInstance npc, TimeSpan totalTime, TimeSpan deltaTime)
    {
        var n = gameData.GetNpc(npc.NpcId);
        var attributes = gameData.GetAttributes(n.AttributesId);
        npc.Health = attributes.Health;
        npc.Endurance = attributes.Endurance;
        worldProcessor.RespawnNpc(player, npc);
        return true;
    }
    private void ExitCombat(Player player, NpcInstance npc, int attackType)
    {
        worldProcessor.SetCombatState(player, false);

        playerState.ExitCombat(player);
        playerState.ClearAttackTime(player, npc);

        ExitCombatAnimation(player);
    }

    private void ExitCombat(NpcInstance npc, Player player, int attackType)
    {
        worldProcessor.SetCombatState(npc, false);

        playerState.ExitCombat(player);
        playerState.ClearAttackTime(player, npc);

        ExitCombatAnimation(player);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ExitCombatAnimation(NpcInstance npc)
    {
        worldProcessor.PlayAnimation(npc, CombatAnimationName, true);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ExitCombatAnimation(Player player)
    {
        worldProcessor.PlayAnimation(player, CombatAnimationName, false);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void PlayEnterCombatAnimation(NpcInstance npc)
    {
        worldProcessor.PlayAnimation(npc, CombatAnimationName, true);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void PlayEnterCombatAnimation(Player player, int attackType)
    {
        worldProcessor.PlayAnimation(player, CombatAnimationName, true, false, GetAttackAnimationNumber(attackType));
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void PlayAttackAnimation(NpcInstance player, int attackType)
    {
        worldProcessor.PlayAnimation(player, CombatAnimationName, true, true, GetAttackAnimationNumber(attackType));
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void PlayAttackAnimation(Player player, int attackType)
    {
        worldProcessor.PlayAnimation(player, CombatAnimationName, true, true, GetAttackAnimationNumber(attackType));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int GetAttackAnimationNumber(int attackType)
    {
        return 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool ReadyForAttack(Player player, NpcInstance npc, int attackType)
    {
        var lastAttack = playerState.GetAttackTime(player, npc);
        var timeDelta = DateTime.UtcNow - lastAttack;
        return timeDelta >= TimeSpan.FromMilliseconds(PlayerMeleeAttackIntervalMs);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool WithinDistance(Player player, NpcInstance npc, int attackType)
    {
        var playerTransform = gameData.GetTransform(player.TransformId);
        var npcTransform = gameData.GetTransform(npc.TransformId);
        var delta = playerTransform.GetPosition() - npcTransform.GetPosition();
        var distance = delta.SqrtMagnitude;
        return distance <= MeleeRange; // change depending how player attacks.
    }
}