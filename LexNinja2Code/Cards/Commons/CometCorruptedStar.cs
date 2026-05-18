using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards.Commons;

public class CometCorruptedStar()
    : LexNinja2Card(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new LexKelaVar(2), new EnergyVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(StaticHoverTip.Energy)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/CometCorruptedStar.mp3");
        var kela = Owner.Creature.GetPower<Lexkela>();
        if (kela is { Amount: > 2 })
        {
            await PowerCmd.Apply<Lexkela>(choiceContext, Owner.Creature, -1, Owner.Creature, this);
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        }
        else
        {
            await NinjaHelper.AddLexKela(choiceContext, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.LexKela().UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"CometCorruptedStar_p.png".BigCardImagePath();
    public override string PortraitPath => $"CometCorruptedStar.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/CometCorruptedStar.png".CardImagePath();
}
