using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class GoBackHands() : LexNinja2Card(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new NinjutsuVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(NinjaKeyword.Hand)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/GoBackHands.mp3");
        var isNinjutsu = await Ninjutsu(choiceContext, play);
        foreach (var card in PileType.Discard.GetPile(Owner).Cards.Where(Filter).ToList())
        {
            if (isNinjutsu)
            {
                card.EnergyCost.SetUntilPlayed(0);
            }
            await CardPileCmd.Add(card, PileType.Hand);
        }
    }

    private static bool Filter(CardModel card)
    {
        var condition =
            card.Keywords.Contains(NinjaKeyword.Hand)
            || card.Tags.Contains(CardTag.OstyAttack)
            || card is HandOfGreed;
        if (!condition)
            return false;
        return card.Type switch
        {
            CardType.Attack or CardType.Skill or CardType.Power => true,
            _ => false,
        };
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override string CustomPortraitPath => $"GoBackHands_p.png".BigCardImagePath();
    public override string PortraitPath => $"GoBackHands.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/GoBackHands.png".CardImagePath();

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
