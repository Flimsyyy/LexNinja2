using System;
using System.Collections.Generic;
using System.Linq;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Combat.SecondaryResources;

namespace LexNinja2.LexNinja2Code.Api.Cards;

public abstract class NinjutsuCard : LexNinja2BaseCard
{
    protected virtual HashSet<CardTag> AdditionalCanonicalTags => [];
    protected sealed override HashSet<CardTag> CanonicalTags =>
        [NinjaTags.Ninjutsu, .. AdditionalCanonicalTags];

    private const string RenShu = "renShu";

    public bool HasLexKelaCostX => DynamicVars.Ninjutsu().HasLexKelaCostX;

    protected NinjutsuCard(int cost, CardType type, CardRarity rarity, TargetType target)
        : base(cost, type, rarity, target)
    {
        var ninjutsuVar = DynamicVars.Ninjutsu();
        if (ninjutsuVar.HasLexKelaCostX)
        {
            this.SecondaryResourceUses()
                .SpendIfAvailable(RenShu, LexKela.Id, SecondaryResourceCost.X());
            return;
        }
        this.SecondaryResourceUses().SpendIfAvailable(RenShu, LexKela.Id, ninjutsuVar.IntValue);
    }

    //在OnPlay中使用
    protected static bool Ninjutsu(CardPlay cardPlay)
    {
        return cardPlay.SecondaryResources().Activated(RenShu);
    }

    public bool CanCastNinjutsu()
    {
        var line = SecondaryResourcePaymentResolver.Plan(this).Lines.ToList().FirstOrDefault();
        return line != null && line.IsAffordable;
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
