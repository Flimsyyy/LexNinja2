using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class SandDefendHand() : LexNinja2Card(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new BlockVar(8, ValueProp.Move), new NinjutsuVar(1), new DynamicVar("Power", 6)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<SandWall>(), HoverTipFactory.FromPower<Lexkela>()];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Hand];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/SandDefendHand.mp3");
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        if (Ninjutsu())
        {
            await PowerCmd.Apply<SandWall>(
                new ThrowingPlayerChoiceContext(),
                Owner.Creature,
                DynamicVars["Power"].BaseValue,
                Owner.Creature,
                this
            );
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
        DynamicVars.Block.UpgradeValueBy(3);
        DynamicVars["Power"].UpgradeValueBy(3);
    }

    public override string CustomPortraitPath => $"SandDefendHand_p.png".BigCardImagePath();
    public override string PortraitPath => $"SandDefendHand.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/SandDefendHand.png".CardImagePath();

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
