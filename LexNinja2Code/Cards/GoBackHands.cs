using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Cards;

public class GoBackHands() : LexNinja2Card(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new NinjutsuVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(NinjaKeyword.Hand)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/GoBackHands.mp3");
        if (Ninjutsu())
        {
            foreach (
                CardModel card2 in (IEnumerable<CardModel>)
                    PileType.Discard.GetPile(Owner).Cards.Where(Filter).ToList()
            )
            {
                card2.EnergyCost.SetUntilPlayed(0);
            }
            // foreach (CardModel card2 in (IEnumerable<CardModel>) PileType.Exhaust.GetPile(Owner).Cards.Where(Filter).ToList())
            // {
            //     card2.EnergyCost.SetUntilPlayed(0);
            // }
        }
        foreach (
            CardModel card2 in (IEnumerable<CardModel>)
                PileType.Discard.GetPile(Owner).Cards.Where(Filter).ToList()
        )
        {
            await CardPileCmd.Add(card2, PileType.Hand);
        }
        // foreach (CardModel card2 in (IEnumerable<CardModel>) PileType.Exhaust.GetPile(Owner).Cards.Where(Filter).ToList())
        // {
        //     await CardPileCmd.Add(card2, PileType.Hand);
        // }
    }

    private bool Filter(CardModel card)
    {
        bool flag1 =
            card.Keywords.Contains(NinjaKeyword.Hand) || card.Tags.Contains(CardTag.OstyAttack);
        bool exflag = card.Title == "HAND_OF_GREED";
        if (flag1 || exflag)
        {
            bool flag2;
            switch (card.Type)
            {
                case CardType.Attack:
                case CardType.Skill:
                case CardType.Power:
                    flag2 = true;
                    break;
                default:
                    flag2 = false;
                    break;
            }
            flag1 = flag2;
        }
        return flag1;
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override string CustomPortraitPath => $"GoBackHands_p.png".BigCardImagePath();
    public override string PortraitPath => $"GoBackHands.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/GoBackHands.png".CardImagePath();

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();

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
}
