using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Cards.Commons;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class SnakeSwitch() : LexNinja2Card(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [HoverTipFactory.FromCard<AngrySnakeBite>(IsUpgraded)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var cards = await CommonActions.SelectCards(
            this,
            CardSelectorPrefs.TransformSelectionPrompt,
            choiceContext,
            PileType.Hand,
            count: DynamicVars.Cards.IntValue
        );
        foreach (var cardModel in cards)
        {
            NinjaAudio.Play("res://LexNinja2/audio/SnakeSwitch.mp3");
            var angrySnakeBite = CombatState?.CreateCard<AngrySnakeBite>(Owner);
            if (angrySnakeBite == null)
            {
                continue;
            }
            if (IsUpgraded)
            {
                CardCmd.Upgrade(angrySnakeBite);
            }
            await CardCmd.Transform(cardModel, angrySnakeBite);
        }
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }

    protected override void OnUpgrade() { }

    public override string CustomPortraitPath => $"SnakeSwitch_p.png".BigCardImagePath();
    public override string PortraitPath => $"SnakeSwitch.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/SnakeSwitch.png".CardImagePath();
}
