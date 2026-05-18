using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards.Commons;

public class SaltStealth() : LexNinja2Card(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new LexKelaVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(StaticHoverTip.Transform)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/Salt.mp3");
        await NinjaHelper.AddLexKela(choiceContext, this);
        var selectedCard = CommonActions
            .SelectSingleCard(this, SelectionScreenPrompt, choiceContext, PileType.Hand)
            .Result;
        if (selectedCard != null)
        {
            await CardCmd.TransformToRandom(selectedCard, RunState!.Rng.CombatCardSelection);
        }
        // await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block,play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.LexKela().UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"Salt.png".BigCardImagePath();
    public override string PortraitPath => $"Salt.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/Salt.png".CardImagePath();
}
