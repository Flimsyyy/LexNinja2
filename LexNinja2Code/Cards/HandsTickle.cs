using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class HandsTickle()
    : LexNinja2Card(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(4, ValueProp.Move),
            (DynamicVar)new CalculationBaseVar(0M),
            (DynamicVar)new CalculationExtraVar(1M),
            (DynamicVar)
                new CalculatedVar("CalculatedHits").WithMultiplier(
                    (Func<CardModel, Creature, Decimal>)(
                        (card, _) =>
                            (Decimal)
                                card.Owner.PlayerCombatState.AllCards.Count<CardModel>(
                                    (Func<CardModel, bool>)(
                                        c => c.Keywords.Contains(NinjaKeyword.Hand)
                                    )
                                )
                    )
                ),
        ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Hand];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/HandsTickle.mp3", 1);
        await DamageCmd
            .Attack(this.DynamicVars.Damage.BaseValue)
            .WithHitCount(
                (int)((CalculatedVar)this.DynamicVars["CalculatedHits"]).Calculate(play.Target)
            )
            .FromCard((CardModel)this)
            .Targeting(play.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }

    public override string CustomPortraitPath => "HandsTickle.png".BigCardImagePath();
    public override string PortraitPath => "HandsTickle.png".CardImagePath();
    public override string BetaPortraitPath => "beta/HandsTickle.png".CardImagePath();
}
