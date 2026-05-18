using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace LexNinja2.LexNinja2Code.Cards;

public class GenshinStorm()
    : LexNinja2Card(2, CardType.Skill, CardRarity.Rare, TargetType.AllAllies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<HolyLittleStorm>(true), HoverTipFactory.FromPower<Lexkela>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [NinjaKeyword.Hand, NinjaKeyword.Blade, CardKeyword.Exhaust];
    protected override bool ShouldGlowGoldInternal => IsGlowed();
    public override CardMultiplayerConstraint MultiplayerConstraint =>
        CardMultiplayerConstraint.MultiplayerOnly;
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/GenshinStorm.mp3");
        IEnumerable<Creature> enumerable =
            from c in base.CombatState.GetTeammatesOf(base.Owner.Creature)
            where c != null && c.IsAlive && c.IsPlayer
            select c;
        foreach (Creature item in enumerable)
        {
            if (item != Owner.Creature)
            {
                CardModel card = CombatState.CreateCard<HolyLittleStorm>(item.Player);
                CardCmd.Upgrade(card);
                card.AddKeyword(CardKeyword.Exhaust);
                card.AddKeyword(CardKeyword.Ethereal);
                await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
            }
        }
        int amount = 0;
        if (Owner.GetRelic<ChemicalX>() != null)
        {
            amount += 2;
            Owner.GetRelic<ChemicalX>().Flash();
        }
        if (Owner.Creature.GetPower<Lexkela>() != null)
        {
            amount += Owner.Creature.GetPower<Lexkela>().Amount;
            foreach (Creature item in enumerable)
            {
                if (item != Owner.Creature)
                {
                    await PowerCmd.Apply<Lexkela>(
                        new ThrowingPlayerChoiceContext(),
                        item.Player.Creature,
                        amount,
                        Owner.Creature,
                        this
                    );
                }
            }
            if (Keywords.Contains(NinjaKeyword.FreeNinjutsu))
            {
                RemoveKeyword(NinjaKeyword.FreeNinjutsu);
                return;
            }
            if (Owner.Creature.GetPower<FreeNinjutsuPower>() != null)
            {
                return;
            }
            await PowerCmd.Apply<Lexkela>(
                new ThrowingPlayerChoiceContext(),
                Owner.Creature,
                -Owner.Creature.GetPower<Lexkela>().Amount,
                Owner.Creature,
                this
            );
        }
    }

    private Boolean IsGlowed()
    {
        if (this.Keywords.Contains(NinjaKeyword.FreeNinjutsu))
        {
            return true;
        }
        if (Owner.Creature.GetPower<FreeNinjutsuPower>() != null)
        {
            return true;
        }
        if (Owner.Creature.GetPower<Lexkela>() != null)
        {
            return true;
        }
        return false;
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override string CustomPortraitPath => $"GenshinStorm.png".BigCardImagePath();
    public override string PortraitPath => $"GenshinStorm.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/GenshinStorm.png".CardImagePath();
}
