using System.Security.AccessControl;
using BaseLib.Extensions;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Api;

public static class NinjutsuCmd
{
    public static bool Ninjutsu(CardModel card, PlayerChoiceContext choiceContext)
    {
        var player = card.Owner;
        if (player.HasPower<FreeNinjutsuPower>())
        {
            return true;
        }
        if (card.Keywords.Contains(NinjaKeyword.FreeNinjutsu))
        {
            card.RemoveKeyword(NinjaKeyword.FreeNinjutsu);
            return true;
        }
        var lexKeLa = player.Creature.GetPower<Lexkela>();
        if (lexKeLa == null)
        {
            return false;
        }
        var renShuAmount = card.DynamicVars.Ninjutsu().BaseValue;
        if (lexKeLa.Amount < renShuAmount)
        {
            return false;
        }
        CommonActions.ApplySelf<Lexkela>(choiceContext, card, -renShuAmount);
        return true;
    }

    public static bool CanCastNinjutsu(CardModel card)
    {
        var player = card.Owner;
        if (
            player.HasPower<FreeNinjutsuPower>()
            || card.Keywords.Contains(NinjaKeyword.FreeNinjutsu)
        )
        {
            return true;
        }
        var lexKeLa = player.Creature.GetPower<Lexkela>();
        if (lexKeLa == null)
        {
            return false;
        }
        return lexKeLa.Amount >= card.DynamicVars.Ninjutsu().BaseValue;
    }
}
