using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards;

public class StoneStrong() : LexNinja2Card(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("Power", 4), new NinjutsuVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.Static(StaticHoverTip.Block),
            HoverTipFactory.FromPower<Lexkela>(),
            HoverTipFactory.FromPower<SandWall>(),
        ];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/StoneStrong.mp3");
        await PowerCmd.Apply<StoneStrongPower>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            DynamicVars["Power"].BaseValue,
            Owner.Creature,
            this
        );
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

    protected override void OnUpgrade()
    {
        DynamicVars["Power"].UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"StoneStrong_p.png".BigCardImagePath();
    public override string PortraitPath => $"StoneStrong.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/StoneStrong.png".CardImagePath();

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
