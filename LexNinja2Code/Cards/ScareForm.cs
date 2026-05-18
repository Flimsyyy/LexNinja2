using BaseLib.Extensions;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards;

public class ScareForm() : LexNinja2Card(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<ScarePower>(1), new NinjutsuVar(0)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/pick.mp3");
        await PowerCmd.Apply<ScarePower>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            DynamicVars.Power<ScarePower>().BaseValue,
            Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<ScarePower>().UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"ScareForm_p.png".BigCardImagePath();
    public override string PortraitPath => $"ScareForm.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/ScareForm.png".CardImagePath();
}
