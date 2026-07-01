using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Extensions;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards.Rares;

public class StoneStrong()
    : LexNinja2NinjutsuCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<StoneStrongPower>(4), new NinjutsuVar(1)];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [
            HoverTipFactory.Static(StaticHoverTip.Block),
            LexKela.HoverTip(),
            HoverTipFactory.FromPower<SandWall>(),
        ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/StoneStrong.mp3");
        await CommonActions.ApplySelf<StoneStrongPower>(choiceContext, this);
        if (!Ninjutsu(play))
        {
            return;
        }
        await PowerCmd.Apply<SandWall>(
            choiceContext,
            Owner.Creature,
            DynamicVars.Power<StoneStrongPower>().BaseValue,
            Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<StoneStrongPower>().UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"StoneStrong_p.png".BigCardImagePath();
    public override string PortraitPath => $"StoneStrong.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/StoneStrong.png".CardImagePath();
}
