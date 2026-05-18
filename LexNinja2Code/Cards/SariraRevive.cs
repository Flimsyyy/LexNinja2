using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using LexNinja2.LexNinja2Code.Relics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Cards;

public class SariraRevive() : LexNinja2Card(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new CardsVar(3), new LexKelaVar(2), new EnergyVar(1)];

    // protected override bool ShouldGlowGoldInternal => isHitPointLow();
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(NinjaKeyword.Sarira)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/SariraRevive.mp3");
        await PowerCmd.Apply<Lexkela>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            DynamicVars["Kela"].BaseValue,
            Owner.Creature,
            this
        );
        await CardPileCmd.Draw(choiceContext, Owner);
        // if (isHitPointLow())
        // {
        //     await CardPileCmd.Draw(choiceContext,DynamicVars.Cards.BaseValue,Owner);
        //     await PlayerCmd.GainEnergy(1,Owner);
        // }
    }

    protected override void OnUpgrade()
    {
        // DynamicVars.Cards.UpgradeValueBy(2);
        DynamicVars["Kela"].UpgradeValueBy(1);
    }

    public override async Task BeforeCardRemoved(CardModel card)
    {
        if ((object)card == this && card.Owner != null)
        {
            Player owner = card.Owner;
            if (!owner.Deck.Cards.Contains(card))
            {
                MainFile.Logger.Warn(
                    "[SariraRevive] Skip relic grant because the card is not present in the owner's deck.",
                    1
                );
            }
            else if (owner.GetRelicById(ModelDb.GetId<Sarira>()) == null)
            {
                NinjaAudio.Play("res://LexNinja2/audio/SariraRevive.mp3");
                await RelicCmd.Obtain(
                    ((RelicModel)ModelDb.Relic<Sarira>()).ToMutable(),
                    owner,
                    owner.Relics.Count
                );
                MainFile.Logger.Info("[SariraRevive] Granted Sarira relic after deck removal.", 1);
            }
        }
    }

    private bool isHitPointLow()
    {
        if (Owner.Creature.CurrentHp <= Owner.Creature.MaxHp / 2)
        {
            return true;
        }
        return false;
    }

    public override string CustomPortraitPath => $"SariraRevive_p.png".BigCardImagePath();
    public override string PortraitPath => $"SariraRevive.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/SariraRevive.png".CardImagePath();
}
