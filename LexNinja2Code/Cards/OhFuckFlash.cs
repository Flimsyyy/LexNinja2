using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Cards;

public class OhFuckFlash() : LexNinja2Card(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    private bool _wasOwnerPartOfLastPlayerTurn = true;
    private bool _isNinjutsuCasted = false;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new NinjutsuVar(3)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/OhFuckFlash.mp3");
        await PowerCmd.Apply<BufferPower>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            1,
            Owner.Creature,
            this
        );
        if (Ninjutsu())
        {
            this._isNinjutsuCasted = true;
            NinjaAudio.Play("res://LexNinja2/audio/Flash.mp3");
            await PowerCmd.Apply<OhFuckFlashPower>(
                new ThrowingPlayerChoiceContext(),
                Owner.Creature,
                1,
                Owner.Creature,
                this
            );
            PlayerCmd.EndTurn(Owner, false);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Renshu"].UpgradeValueBy(-1);
    }

    public override string CustomPortraitPath => $"OhFuckFlash_p.png".BigCardImagePath();
    public override string PortraitPath => $"OhFuckFlash.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/OhFuckFlash.png".CardImagePath();

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

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
