using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Cards;

public class SnakeSwitch() : LexNinja2Card(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<AngrySnakeBite>(IsUpgraded)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        // foreach (CardModel original in (await CardSelectCmd.FromSimpleGrid(choiceContext, (IReadOnlyList<CardModel>) PileType.Draw.GetPile(Owner).Cards.OrderBy<CardModel, CardRarity>((Func<CardModel, CardRarity>) (c => c.Rarity)).ThenBy<CardModel, ModelId>((Func<CardModel, ModelId>) (c => c.Id)).ToList<CardModel>(), Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, DynamicVars.Cards.IntValue))).ToList<CardModel>())
        // {
        //     await CardPileCmd.AddGeneratedCardToCombat(original.CreateClone(),PileType.Hand,true);
        //     CardPileAddResult? nullable = await CardCmd.TransformTo<AngrySnakeBite>(original);
        // }

        foreach (
            CardModel cardModel in (
                await CardSelectCmd.FromHand(
                    choiceContext,
                    Owner,
                    new CardSelectorPrefs(
                        CardSelectorPrefs.TransformSelectionPrompt,
                        DynamicVars.Cards.IntValue
                    ),
                    (Func<CardModel, bool>)null,
                    (AbstractModel)this
                )
            ).ToList<CardModel>()
        )
        {
            NinjaAudio.Play("res://LexNinja2/audio/SnakeSwitch.mp3");
            if (cardModel != null)
            {
                CardModel cardModel2 = base.CombatState.CreateCard<AngrySnakeBite>(base.Owner);
                if (base.IsUpgraded)
                {
                    CardCmd.Upgrade(cardModel2);
                }
                await CardCmd.Transform(cardModel, cardModel2);
            }
        }
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }

    protected override void OnUpgrade() { }

    public override string CustomPortraitPath => $"SnakeSwitch_p.png".BigCardImagePath();
    public override string PortraitPath => $"SnakeSwitch.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/SnakeSwitch.png".CardImagePath();

    private IEnumerable<CardModel> GetCards()
    {
        CardModel card = CombatState.CreateCard<AngrySnakeBite>(Owner);
        return PileType
            .Hand.GetPile(this.Owner)
            .Cards.Where<CardModel>((Func<CardModel, bool>)(c => c == card));
    }

    private Boolean Ninjutsu()
    {
        if (Owner.Creature.GetPower<FreeNinjutsuPower>() != null)
        {
            return true;
        }
        if (Owner.Creature.GetPower<Lexkela>() != null)
        {
            if (Owner.Creature.GetPower<Lexkela>().Amount >= DynamicVars["Renshu"].BaseValue)
            {
                PowerCmd.Apply<Lexkela>(
                    new ThrowingPlayerChoiceContext(),
                    Owner.Creature,
                    -DynamicVars["Renshu"].BaseValue,
                    Owner.Creature,
                    this
                );
                return true;
            }
        }
        return false;
    }

    private Boolean CanCastNinjutsu()
    {
        if (Owner.Creature.GetPower<FreeNinjutsuPower>() != null)
        {
            return true;
        }

        if (Owner.Creature.GetPower<Lexkela>() != null)
        {
            if (Owner.Creature.GetPower<Lexkela>().Amount >= DynamicVars["Renshu"].BaseValue)
            {
                return true;
            }
        }

        return false;
    }
}
