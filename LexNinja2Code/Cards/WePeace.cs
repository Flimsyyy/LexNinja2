using BaseLib.Extensions;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Cards;

public class WePeace() : LexNinja2Card(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<IntangiblePower>(1), new NinjutsuVar(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<IntangiblePower>(), HoverTipFactory.FromPower<Lexkela>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Ninjutsu())
        {
            NinjaAudio.Play("res://LexNinja2/audio/WePeace.mp3");
            await PowerCmd.Apply<WePeacePower>(
                new ThrowingPlayerChoiceContext(),
                Owner.Creature,
                1,
                Owner.Creature,
                this
            );
            if (Owner.Creature.GetPower<IntangiblePower>() == null)
            {
                await PowerCmd.Apply<IntangiblePower>(
                    new ThrowingPlayerChoiceContext(),
                    Owner.Creature,
                    DynamicVars.Power<IntangiblePower>().BaseValue,
                    Owner.Creature,
                    this
                );
            }
            await CardCmd.Exhaust(choiceContext, this);
        }
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

    protected override void OnUpgrade()
    {
        CardCmd.RemoveKeyword(this, CardKeyword.Ethereal);
    }

    public override string CustomPortraitPath => $"WePeace_p.png".BigCardImagePath();
    public override string PortraitPath => $"WePeace.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/WePeace.png".CardImagePath();
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
}
