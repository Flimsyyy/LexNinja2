using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Commons;

public class MonkHand()
    : LexNinja2NinjutsuCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new NinjutsuVar(1),
            new CalculationBaseVar(0),
            new BlockVar(6, ValueProp.Move),
            new CalculatedDamageVar(ValueProp.Move).WithMultiplier(
                (card, _) =>
                {
                    var block = card.Owner.Creature.Block;
                    if (card is not NinjutsuCard ninjaCard || !ninjaCard.CanCastNinjutsu())
                        return block;
                    block += (int)
                        Hook.ModifyBlock(
                            card.Owner.Creature.CombatState!,
                            card.Owner.Creature,
                            card.DynamicVars.Block.BaseValue,
                            ValueProp.Move,
                            null,
                            null,
                            out var _
                        );
                    return block;
                }
            ),
            new ExtraDamageVar(1M),
        ];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [LexKela.HoverTip()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Hand];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/MonkHand.mp3");
        if (Ninjutsu(play))
        {
            await CommonActions.CardBlock(this, play);
        }
        await CommonActions
            .CardAttack(
                this,
                play.Target,
                damage: Owner.Creature.Block,
                vfx: "vfx/vfx_attack_blunt",
                tmpSfx: "blunt_attack.mp3"
            )
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }

    public override string CustomPortraitPath => $"MonkHand_p.png".BigCardImagePath();
    public override string PortraitPath => $"MonkHand.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/MonkHand.png".CardImagePath();
}
