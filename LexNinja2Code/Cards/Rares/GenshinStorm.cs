using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Cards.Commons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards.Rares;

public class GenshinStorm()
    : LexNinja2NinjutsuCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies, true)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new NinjutsuVar(0)];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [HoverTipFactory.FromCard<HolyLittleStorm>(true), LexKela.HoverTip()];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [NinjaKeyword.Hand, NinjaKeyword.Blade, CardKeyword.Exhaust];
    public override CardMultiplayerConstraint MultiplayerConstraint =>
        CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/GenshinStorm.mp3");
        IEnumerable<Creature> players =
            from c in CombatState?.GetTeammatesOf(Owner.Creature)
            where c is { IsAlive: true, IsPlayer: true }
            select c;
        var amount = ResolveLexkelaXValue(play);
        Ninjutsu(play);
        foreach (var player in players)
        {
            if (player == Owner.Creature)
                continue;
            var card = CombatState?.CreateCard<HolyLittleStorm>(player.Player!);
            if (card != null)
            {
                CardCmd.Upgrade(card);
                card.AddKeyword(CardKeyword.Exhaust);
                card.AddKeyword(CardKeyword.Ethereal);
                await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
            }
            if (player != Owner.Creature)
            {
                await LexKela.Gain(player.Player!, amount, this);
            }
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override string CustomPortraitPath => $"GenshinStorm.png".BigCardImagePath();
    public override string PortraitPath => $"GenshinStorm.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/GenshinStorm.png".CardImagePath();
}
