using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Scaffolding.Content;

namespace LexNinja2.LexNinja2Code.Api.Cards;

public abstract class NinjutsuCard : ModCardTemplate
{
    protected virtual HashSet<CardTag> AdditionalCanonicalTags => [];
    protected sealed override HashSet<CardTag> CanonicalTags =>
        [NinjaTags.Ninjutsu, .. AdditionalCanonicalTags];

    private const string RenShu = "ren_shu";

    public bool HasLexKelaCostX { get; }

    protected NinjutsuCard(
        int cost,
        CardType type,
        CardRarity rarity,
        TargetType target,
        bool hasLexKelaCostX = false
    )
        : base(cost, type, rarity, target)
    {
        HasLexKelaCostX = hasLexKelaCostX;
        if (hasLexKelaCostX)
        {
            this.SecondaryResourceUses()
                .SpendIfAvailable(RenShu, LexKela.Id, SecondaryResourceCost.X());
            return;
        }
        this.SecondaryResourceUses()
            .SpendIfAvailable(RenShu, LexKela.Id, DynamicVars.Ninjutsu().IntValue);
    }

    //在OnPlay中使用
    protected static bool Ninjutsu(CardPlay cardPlay)
    {
        return cardPlay.SecondaryResources().Activated(RenShu);
    }

    public bool CanCastNinjutsu()
    {
        return SecondaryResourcePaymentResolver.CanPay(this);
    }

    protected void UpgradeNinjutsuValueBy(int addend)
    {
        DynamicVars.Ninjutsu().UpgradeValueBy(addend);
        this.SecondaryResourceUses()
            .SpendIfAvailable(RenShu, LexKela.Id, DynamicVars.Ninjutsu().IntValue);
    }

    protected int ResolveLexkelaXValue(CardPlay play)
    {
        if (!HasLexKelaCostX)
        {
            throw new InvalidOperationException("This card does not have an X-cost.");
        }
        return play.SecondaryResources().Value(LexKela.Id);
    }

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
