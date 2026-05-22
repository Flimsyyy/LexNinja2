using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Hooks;
using LexNinja2.LexNinja2Code.Character;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;

namespace LexNinja2.LexNinja2Code.Api.Cards;

[Pool(typeof(LexNinja2CardPool))]
public abstract class LexNinja2Card(int cost, CardType type, CardRarity rarity, TargetType target)
    : CustomCardModel(cost, type, rarity, target)
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    public override string BetaPortraitPath =>
        $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    private List<TemporaryCardCost> _temporaryLexKelaCosts = [];
    public event Action? LexKelaCostChanged;

    public virtual bool HasLexKelaCostX => false;

    public TemporaryCardCost? TemporaryLexKelaCost => _temporaryLexKelaCosts.LastOrDefault();

    protected async Task<bool> Ninjutsu(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.IsAutoPlay)
        {
            return true;
        }
        var lexKeLa = GetLexKelaAmount();
        var renShuAmount = GetLexKelaCostWithModifiers();
        if (lexKeLa < renShuAmount)
        {
            return false;
        }
        await SpendLexKela(renShuAmount, choiceContext);
        return true;
    }

    public bool CanCastNinjutsu()
    {
        return GetLexKelaAmount() >= GetLexKelaCostWithModifiers();
    }

    protected int ResolveLexkelaXValue()
    {
        if (!HasLexKelaCostX)
        {
            throw new InvalidOperationException("This card does not have an X-cost.");
        }

        var value = Hook.ModifyXValue(CombatState!, this, Owner.Creature.GetPowerAmount<Lexkela>());
        return value;
    }

    public void SetLexKelaToFreeUntilPlayed()
    {
        SetLexKelaCostUntilPlayed(0);
    }

    public void SetLexKelaToFreeThisTurn()
    {
        SetLexKelaCostThisTurn(0);
    }

    public void SetLexKelaToFreeThisCombat()
    {
        SetLexKelaCostThisCombat(0);
    }

    public void SetLexKelaCostUntilPlayed(int cost)
    {
        AddTemporaryLexKelaCost(TemporaryCardCost.UntilPlayed(cost));
    }

    public void SetLexKelaCostThisTurn(int cost)
    {
        AddTemporaryLexKelaCost(TemporaryCardCost.ThisTurn(cost));
    }

    public void SetLexKelaCostThisCombat(int cost)
    {
        AddTemporaryLexKelaCost(TemporaryCardCost.ThisCombat(cost));
    }

    private void AddTemporaryLexKelaCost(TemporaryCardCost cost)
    {
        AssertMutable();
        if (HasLexKelaCostX || !DynamicVars.ContainsKey(NinjutsuVar.Key))
        {
            return;
        }
        _temporaryLexKelaCosts.Add(cost);
        LexKelaCostChanged?.Invoke();
    }

    public int GetBaseLexKelaCost()
    {
        return DynamicVars.Ninjutsu().IntValue;
    }

    public int GetCurrentLexKelaCost()
    {
        var cost = _temporaryLexKelaCosts.LastOrDefault()?.Cost;
        var baseCost = GetBaseLexKelaCost();
        return cost ?? baseCost;
    }

    public int GetLexKelaAmount()
    {
        return Owner.Creature.GetPowerAmount<Lexkela>();
    }

    public int GetLexKelaCostWithModifiers()
    {
        if (HasLexKelaCostX)
        {
            return GetLexKelaAmount();
        }

        if (Owner == null)
        {
            return GetCurrentLexKelaCost();
        }
        var runState = Owner.RunState;
        if (runState != null && CombatState != null)
            return (int)
                NinjaHooks.ModifyLexKelaCost(
                    Owner.RunState,
                    CombatState,
                    this,
                    GetCurrentLexKelaCost()
                );
        return GetCurrentLexKelaCost();
    }

    public async Task SpendLexKela(int amount, PlayerChoiceContext choiceContext)
    {
        if (amount <= 0)
        {
            await NinjaHooks.AfterLexKelaSpent(Owner.RunState, CombatState!, amount, Owner);
            return;
        }
        await CommonActions.ApplySelf<Lexkela>(choiceContext, this, -amount);
        if (TemporaryLexKelaCost != null)
        {
            ClearsLexKelaWhenCardIsPlayed();
        }
        await NinjaHooks.AfterLexKelaSpent(Owner.RunState, CombatState!, amount, Owner);
    }

    public void ClearsLexKelaWhenCardIsPlayed()
    {
        var count = _temporaryLexKelaCosts.RemoveAll(c => c.ClearsWhenCardIsPlayed);
        if (count <= 0)
        {
            return;
        }
        LexKelaCostChanged?.Invoke();
    }

    public void ClearsLexKelaWhenTurnEnds()
    {
        var count = _temporaryLexKelaCosts.RemoveAll(c => c.ClearsWhenTurnEnds);
        if (count <= 0)
        {
            return;
        }
        LexKelaCostChanged?.Invoke();
    }

    protected override void DeepCloneFields()
    {
        base.DeepCloneFields();
        _temporaryLexKelaCosts = _temporaryLexKelaCosts.ToList();
    }
}
