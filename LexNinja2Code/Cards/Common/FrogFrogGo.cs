using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Cards;

public class FrogFrogGo() : LexNinja2Card(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new NinjutsuVar(1), new EnergyVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromKeyword(CardKeyword.Exhaust),
            HoverTipFactory.FromPower<Lexkela>(),
            HoverTipFactory.Static(StaticHoverTip.Energy),
        ];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/FrogFrogGo.mp3");
        if (Ninjutsu(choiceContext))
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        }
        var card = await CommonActions.SelectSingleCard(this, CardSelectorPrefs.ExhaustSelectionPrompt, choiceContext,
            PileType.Hand);
        if (card != null)
        {
            await CardCmd.Exhaust(choiceContext, card);
        }
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => "FrogFrogGo_p.png".BigCardImagePath();
    public override string PortraitPath => "FrogFrogGo.png".CardImagePath();
    public override string BetaPortraitPath => "beta/FrogFrogGo.png".CardImagePath();

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
