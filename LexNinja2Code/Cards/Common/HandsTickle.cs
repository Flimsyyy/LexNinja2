using BaseLib.Utils;
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
    private const string CalculatedHits = "CalculatedHits";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(4, ValueProp.Move),
            new CalculationBaseVar(0M),
            new CalculationExtraVar(1M),
            new CalculatedVar(CalculatedHits).WithMultiplier(
                (card, _) =>
                    card.Owner.PlayerCombatState!.AllCards.Count(c =>
                        c.Keywords.Contains(NinjaKeyword.Hand)
                    )
            ),
        ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Hand];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/HandsTickle.mp3");
        var hitCount = (int)((CalculatedVar)DynamicVars[CalculatedHits]).Calculate(play.Target);
        //因为没有CalculatedDamageVar，所以可以这样使用
        await CommonActions.CardAttack(this, play, hitCount: hitCount).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }

    public override string CustomPortraitPath => "HandsTickle.png".BigCardImagePath();
    public override string PortraitPath => "HandsTickle.png".CardImagePath();
    public override string BetaPortraitPath => "beta/HandsTickle.png".CardImagePath();
}
