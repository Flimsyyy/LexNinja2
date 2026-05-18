using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Character;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace LexNinja2.LexNinja2Code.Cards;

[Pool(typeof(LexNinja2CardPool))]
public abstract class LexNinja2Card(int cost, CardType type, CardRarity rarity, TargetType target)
    : CustomCardModel(cost, type, rarity, target)
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath =>
        $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    protected bool Ninjutsu(PlayerChoiceContext choiceContext)
    {
        var player = Owner;
        if (player.HasPower<FreeNinjutsuPower>())
        {
            return true;
        }
        if (Keywords.Contains(NinjaKeyword.FreeNinjutsu))
        {
            RemoveKeyword(NinjaKeyword.FreeNinjutsu);
            return true;
        }
        var lexKeLa = player.Creature.GetPower<Lexkela>();
        if (lexKeLa == null)
        {
            return false;
        }
        var renShuAmount = DynamicVars.Ninjutsu().BaseValue;
        if (lexKeLa.Amount < renShuAmount)
        {
            return false;
        }
        CommonActions.ApplySelf<Lexkela>(choiceContext, this, -renShuAmount);
        return true;
    }

    protected bool CanCastNinjutsu()
    {
        var player = Owner;
        if (player.HasPower<FreeNinjutsuPower>() || Keywords.Contains(NinjaKeyword.FreeNinjutsu))
        {
            return true;
        }
        var lexKeLa = player.Creature.GetPower<Lexkela>();
        if (lexKeLa == null)
        {
            return false;
        }
        return lexKeLa.Amount >= DynamicVars.Ninjutsu().BaseValue;
    }
}
