using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class BeastShout()
    : LexNinja2Card(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
{
    private const string StrengthLoss = "StrengthLoss";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(11, ValueProp.Move), new(StrengthLoss, 1), new NinjutsuVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<StrengthPower>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await NinjaAnim.TriggerCastAnim(this);
        NinjaAudio.Play("res://LexNinja2/audio/BeastShout.mp3");
        await Cmd.Wait(0.5f);
        await CommonActions
            .CardAttack(
                this,
                play,
                vfx: "vfx/vfx_attack_lightning",
                tmpSfx: "lightning_orb_evoke.mp3"
            )
            .Execute(choiceContext);
        if (!Ninjutsu(choiceContext))
        {
            return;
        }
        NinjaAudio.Play("res://LexNinja2/audio/BeastVoice.mp3");
        await PowerCmd.Apply<StrengthPower>(
            choiceContext,
            CombatState!.HittableEnemies,
            -DynamicVars[StrengthLoss].BaseValue,
            Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
        DynamicVars[StrengthLoss].UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => "BeastShout_p.png".BigCardImagePath();
    public override string PortraitPath => "BeastShout.png".CardImagePath();
    public override string BetaPortraitPath => "beta/BeastShout.png".CardImagePath();

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
