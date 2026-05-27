using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Commons;

public class SaltStealth() : LexNinja2Card(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(StaticHoverTip.Transform)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/Salt.mp3");
        await CommonActions.CardBlock(this, play);
        var selectedCard = await CommonActions.SelectSingleCard(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Hand
        );
        if (selectedCard != null)
        {
            await CardCmd.TransformToRandom(selectedCard, Owner.RunState.Rng.CombatCardSelection);
        }
        // await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block,play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }

    public override string CustomPortraitPath => $"Salt.png".BigCardImagePath();
    public override string PortraitPath => $"Salt.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/Salt.png".CardImagePath();
}
