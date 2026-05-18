using BaseLib.Extensions;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Cards.Tokens;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class LanBladeCutting()
    : LexNinja2Card(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new NinjutsuVar(1), new PowerVar<LanBladePower>(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<LanBlade>(IsUpgraded)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Blade];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/LanBladeCutting.mp3");
        if (IsUpgraded)
        {
            await PowerCmd.Apply<LanBladePowerUpgraded>(
                choiceContext,
                Owner.Creature,
                DynamicVars.Power<LanBladePower>().IntValue,
                Owner.Creature,
                this
            );
        }
        else
        {
            await CommonActions.ApplySelf<LanBladePower>(choiceContext, this);
        }

        if (!Ninjutsu(choiceContext))
        {
            return;
        }
        {
            CardModel card = CombatState!.CreateCard<LanBlade>(Owner);
            if (IsUpgraded)
            {
                CardCmd.Upgrade(card);
            }
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
        }
    }

    protected override void OnUpgrade() { }

    public override string CustomPortraitPath => $"LanBladeCutting_p.png".BigCardImagePath();
    public override string PortraitPath => $"LanBladeCutting.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/LanBladeCutting.png".CardImagePath();

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
