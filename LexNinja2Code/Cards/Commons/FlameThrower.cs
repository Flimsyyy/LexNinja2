using BaseLib.Extensions;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Commons;

public class FlameThrower()
    : LexNinja2Card(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(6, ValueProp.Move),
            new PowerVar<PoisonPower>(5),
            new PowerVar<WeakPower>(1),
            new NinjutsuVar(1),
        ];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<WeakPower>(),
            HoverTipFactory.FromPower<PoisonPower>(),
            HoverTipFactory.FromPower<Lexkela>(),
        ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/FlameThrower.mp3");
        var instance = NCombatRoom.Instance;
        instance?.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(play.Target!)!);
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        await CommonActions.Apply<WeakPower>(choiceContext, this, play);
        if (!await Ninjutsu(choiceContext, play))
        {
            return;
        }
        await CommonActions.Apply<PoisonPower>(choiceContext, this, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars.Power<WeakPower>().UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"FlameThrower_p.png".BigCardImagePath();
    public override string PortraitPath => $"FlameThrower.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/FlameThrower.png".CardImagePath();

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
