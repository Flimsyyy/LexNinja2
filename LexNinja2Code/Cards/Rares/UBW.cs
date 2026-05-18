using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Rares;

public class UBW() : LexNinja2Card(2, CardType.Attack, CardRarity.Rare, TargetType.RandomEnemy)
{
    private const string Hitcounts = "HitCounts";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new NinjutsuVar(1),
            new DamageVar(8, ValueProp.Move),
            new CalculationBaseVar(0),
            new CalculationExtraVar(1),
            new CalculatedVar(Hitcounts).WithMultiplier(
                (card, _) =>
                    CombatManager.Instance.History.CardPlaysFinished.Count(e =>
                        e.CardPlay.Card.Owner == card.Owner
                        && (
                            e.CardPlay.Card.Keywords.Contains(NinjaKeyword.Blade)
                            || e.CardPlay.Card.Tags.Contains(CardTag.Shiv)
                        )
                    )
            ),
        ];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Blade];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (!Ninjutsu(choiceContext))
        {
            return;
        }
        NinjaAudio.Play("res://LexNinja2/audio/UBW.mp3");
        await NinjaAnim.TriggerCastAnim(this);
        var instance = NCombatRoom.Instance;
        instance?.CombatVfxContainer.AddChildSafely(NHellraiserVfx.Create(Owner.Creature)!);
        await Cmd.Wait(1);
        var hitCount = (int)((CalculatedVar)this.DynamicVars[Hitcounts]).Calculate(play.Target);
        await CommonActions
            .CardAttack(
                this,
                play,
                hitCount: hitCount,
                vfx: "vfx/hellraiser_attack_vfx",
                tmpSfx: "heavy_attack.mp3"
            )
            .SpawningHitVfxOnEachCreature()
            .WithHitVfxSpawnedAtBase()
            .Execute(choiceContext);
        ;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }

    public override string CustomPortraitPath => $"UBW_p.png".BigCardImagePath();
    public override string PortraitPath => $"UBW.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/UBW.png".CardImagePath();

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
