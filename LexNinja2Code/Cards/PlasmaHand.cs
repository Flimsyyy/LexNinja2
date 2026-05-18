using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards;

public class PlasmaHand() : LexNinja2Card(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new CardsVar(1), new LexKelaVar(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Hand];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/PlasmaHand.mp3");
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        await PowerCmd.Apply<Lexkela>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            DynamicVars["Kela"].BaseValue,
            Owner.Creature,
            null
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"PlasmaHand_p.png".BigCardImagePath();
    public override string PortraitPath => $"PlasmaHand.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/PlasmaHand.png".CardImagePath();
}
