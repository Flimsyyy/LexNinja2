using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class DarknessShoryuKen()
    : LexNinja2Card(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new NinjutsuVar(1), new DamageVar(8, ValueProp.Move), new PowerVar<WeakPower>(2)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Hand];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/DarknessShoryuKen.mp3");
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        await CommonActionsExtensions.Apply<WeakPower>(choiceContext, this, play);
        if (!Ninjutsu(choiceContext))
        {
            return;
        }
        var pile = PileType.Hand.GetPile(Owner);
        var cardToExhaust = Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards);
        if (cardToExhaust != null)
        {
            await CardCmd.Exhaust(choiceContext, cardToExhaust);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }

    public override string CustomPortraitPath => $"DarknessShoryuKen_p.png".BigCardImagePath();
    public override string PortraitPath => $"DarknessShoryuKen.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/DarknessShoryuKen.png".CardImagePath();

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
