using BaseLib.Extensions;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Cards;

public class ReaperFlameBlade()
    : LexNinja2Card(7, CardType.Power, CardRarity.Rare, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<ReaperFlame>(3), new PowerVar<IntangiblePower>(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Blade];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/DeadBurningBladeSmog.mp3");
        await CommonActions.ApplySelf<IntangiblePower>(choiceContext, this);
        await CommonActions.ApplySelf<ReaperFlame>(choiceContext, this);
    }

    public override Task AfterCardExhausted(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool causedByEthereal
    )
    {
        if (card.Owner != Owner)
        {
            return Task.CompletedTask;
        }
        EnergyCost.AddThisCombat(-1);
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<IntangiblePower>().UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"DeathGodBlade_p.png".BigCardImagePath();
    public override string PortraitPath => $"DeathGodBlade.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/DeathGodBlade.png".CardImagePath();
}
