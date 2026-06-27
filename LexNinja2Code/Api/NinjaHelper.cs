using System.Reflection;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace LexNinja2.LexNinja2Code.Api;

public static class NinjaHelper
{
    public static async Task DrawExtra(CardModel card, PlayerChoiceContext ctx)
    {
        await CardPileCmd.Draw(ctx, card.DynamicVars.ExtraCard().IntValue, card.Owner);
    }

    public static CardModel? LastCard(Player player)
    {
        var cardLast = CombatManager
            .Instance.History.CardPlaysFinished.LastOrDefault(
                delegate(CardPlayFinishedEntry e)
                {
                    var flag =
                        e.CardPlay.Card.Owner == player
                        && !e.CardPlay.Card.Tags.Contains(NinjaTags.Snake);
                    // if (flag2)
                    // {
                    //     CardType type = e.CardPlay.Card.Type;
                    //     bool flag3 = (uint)(type - 1) <= 1u;
                    //     flag2 = flag3;
                    // }
                    return flag && !e.CardPlay.Card.IsDupe;
                }
            )
            ?.CardPlay.Card;
        return cardLast;
    }

    public static CardModel? LastCard(CardModel card)
    {
        return LastCard(card.Owner);
    }

    public static object? CloneData(object source)
    {
        var type = source.GetType();
        var clone = Activator.CreateInstance(type);
        var fields = type.GetFields(
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        );
        foreach (var field in fields)
        {
            var value = field.GetValue(source);
            field.SetValue(clone, value);
        }
        return clone;
    }

    public static bool IsHandRenShu(CardModel card)
    {
        return card.Keywords.Contains(NinjaKeyword.Hand)
            || card.Tags.Contains(CardTag.OstyAttack)
            || card is HandOfGreed;
    }

    public static bool IsBladeRenShu(CardModel card)
    {
        return card.Keywords.Contains(NinjaKeyword.Blade) || card.Tags.Contains(CardTag.Shiv);
    }

    public static void UpgradeDynamicVarValue(DynamicVar dynamicVar, decimal addend)
    {
        dynamicVar.UpgradeValueBy(addend);
        if (dynamicVar.BaseValue >= 0)
            return;
        dynamicVar.BaseValue = 0;
        dynamicVar.FinalizeUpgrade();
    }

    public static T GetValueByChallengeMode<T>(T challengeValue, T normalValue)
    {
        return NinjaConfigsPage.IsChallengeMode() ? challengeValue : normalValue;
    }
}
