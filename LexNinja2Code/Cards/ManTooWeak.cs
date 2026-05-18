using BaseLib.Extensions;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Cards;

public class ManTooWeak() : LexNinja2Card(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<StrengthPower>(5),
            new PowerVar<DexterityPower>(5),
            new PowerVar<ManTooWeakPower>(2),
        ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Lexkela>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Hand];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/ManTooWeak.mp3");
        await PowerCmd.Apply<ManTooWeakPower>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            DynamicVars.Power<ManTooWeakPower>().BaseValue,
            Owner.Creature,
            this
        );
        await PowerCmd.Apply<StrengthPower>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            DynamicVars.Strength.BaseValue,
            Owner.Creature,
            this
        );
        await PowerCmd.Apply<DexterityPower>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            DynamicVars.Strength.BaseValue,
            Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<ManTooWeakPower>().UpgradeValueBy(-1);
    }

    public override string CustomPortraitPath => $"ManTooWeak_p.png".BigCardImagePath();
    public override string PortraitPath => $"ManTooWeak.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/ManTooWeak.png".CardImagePath();
    //
    // private Boolean Ninjutsu()
    // {
    //     if (Owner.Creature.GetPower<FreeNinjutsuPower>() != null)
    //     {
    //         return true;
    //     }
    //     if (Owner.Creature.GetPower<Lexkela>() != null)
    //     {
    //         if (Owner.Creature.GetPower<Lexkela>().Amount >= DynamicVars["Renshu"].BaseValue)
    //         {
    //             PowerCmd.Apply<Lexkela>(new ThrowingPlayerChoiceContext(), Owner.Creature,-DynamicVars["Renshu"].BaseValue, Owner.Creature, this);
    //             return true;
    //         }
    //     }
    //     return false;
    // }
    //
    // private Boolean CanCastNinjutsu()
    // {
    //     if (Owner.Creature.GetPower<FreeNinjutsuPower>() != null)
    //     {
    //         return true;
    //     }
    //
    //     if (Owner.Creature.GetPower<Lexkela>() != null)
    //     {
    //         if (Owner.Creature.GetPower<Lexkela>().Amount >= DynamicVars["Renshu"].BaseValue)
    //         {
    //             return true;
    //         }
    //     }
    //
    //     return false;
    // }
    // protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
