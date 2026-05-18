using BaseLib.Patches.Features;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Api.Extensions;

// 从BaseLib临时复制的，更新后移除。
public static class CommonActionsExtensions
{
    /// <summary>
    /// Applies a power of type <typeparamref name="T"/> to the appropriate creature(s) based on
    /// the card's <see cref="TargetType"/>, handling both vanilla and custom target types.
    /// </summary>
    /// <typeparam name="T">The <see cref="PowerModel"/> type to apply.</typeparam>
    /// <param name="ctx">The player choice context for the current action.</param>
    /// <param name="card">The card being played, used to determine targeting behaviour.</param>
    /// <param name="cardPlay">
    /// The card play instance carrying the selected target for single-target cards.
    /// May be <see langword="null"/> for untargeted cards.
    /// </param>
    /// <returns>
    /// A list of all applied power instances, or an empty list if no valid targets were found.
    /// </returns>
    public static async Task<IReadOnlyList<T>> Apply<T>(
        PlayerChoiceContext ctx,
        CardModel card,
        CardPlay? cardPlay
    )
        where T : PowerModel
    {
        if (CustomTargetType.IsCustomSingleTargetType(card.TargetType))
        {
            if (cardPlay is null)
                return [];
            return await ApplyToTargetedCreature<T>(ctx, card, cardPlay) is { } a ? [a] : [];
        }
        if (CustomTargetType.IsCustomMultiTargetType(card.TargetType) && card.CombatState != null)
            return await ApplyToFilteredCreatures<T>(ctx, card);
        switch (card)
        {
            case { TargetType: TargetType.AnyEnemy or TargetType.AnyAlly or TargetType.AnyPlayer }:
                if (cardPlay is null)
                    return [];
                return await ApplyToTargetedCreature<T>(ctx, card, cardPlay) is { } a ? [a] : [];
            case { TargetType: TargetType.AllEnemies, CombatState: not null }:
                return await ApplyToAllEnemies<T>(ctx, card);
            case { TargetType: TargetType.RandomEnemy, CombatState: not null }:
                return await ApplyToRandomEnemy<T>(ctx, card) is { } b ? [b] : [];
        }
        return [];
    }

    /// <summary>
    /// Applies a power of type <typeparamref name="T"/> to all currently hittable enemies.
    /// </summary>
    /// <typeparam name="T">The <see cref="PowerModel"/> type to apply.</typeparam>
    /// <param name="ctx">The player choice context for the current action.</param>
    /// <param name="card">The card being played; its <see cref="CardModel.CombatState"/> is used to retrieve enemies.</param>
    /// <returns>
    /// A list of all applied power instances, or an empty list if there is no active combat state.
    /// </returns>
    public static async Task<IReadOnlyList<T>> ApplyToAllEnemies<T>(
        PlayerChoiceContext ctx,
        CardModel card
    )
        where T : PowerModel
    {
        if (card.CombatState == null)
            return [];
        return await CommonActions.Apply<T>(ctx, card.CombatState.HittableEnemies, card);
    }

    /// <summary>
    /// Applies a power of type <typeparamref name="T"/> to a single randomly selected hittable enemy.
    /// </summary>
    /// <typeparam name="T">The <see cref="PowerModel"/> type to apply.</typeparam>
    /// <param name="ctx">The player choice context for the current action.</param>
    /// <param name="card">The card being played; its <see cref="CardModel.CombatState"/> is used to retrieve enemies.</param>
    /// <returns>
    /// The applied power instance, or <see langword="null"/> if no hittable enemies are available.
    /// </returns>
    public static async Task<T?> ApplyToRandomEnemy<T>(PlayerChoiceContext ctx, CardModel card)
        where T : PowerModel
    {
        var enemy = card
            .CombatState?.HittableEnemies.TakeRandom(1, card.CombatState.RunState.Rng.CombatTargets)
            .FirstOrDefault();
        if (enemy == null)
            return null;
        return await CommonActions.Apply<T>(ctx, enemy, card);
    }

    private static async Task<IReadOnlyList<T>> ApplyToFilteredCreatures<T>(
        PlayerChoiceContext ctx,
        CardModel card
    )
        where T : PowerModel
    {
        if (card.CombatState == null)
            return [];
        var targets = card
            .CombatState.HittableEnemies.Concat(card.CombatState.PlayerCreatures)
            .Where(c => CustomTargetType.CanMultiTarget(card.TargetType, c))
            .ToList();
        return await CommonActions.Apply<T>(ctx, targets, card);
    }

    private static async Task<T?> ApplyToTargetedCreature<T>(
        PlayerChoiceContext ctx,
        CardModel card,
        CardPlay cardPlay
    )
        where T : PowerModel
    {
        if (cardPlay.Target is null)
            return null;
        return await CommonActions.Apply<T>(ctx, cardPlay.Target, card);
    }
}
