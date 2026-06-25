using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace LexNinja2.LexNinja2Code.Cards.Events;

[RegisterCard(typeof(TokenCardPool))]
public class AlanWalker() : ModCardTemplate(3, CardType.Power, CardRarity.Event, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<AlanWalkerPower>(1)];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [HoverTipFactory.Static(StaticHoverTip.Block)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Stop("res://LexNinja2/audio/Faded.mp3", 5f);
        NinjaAudio.Play("res://LexNinja2/audio/AlanWalker.mp3");
        await CommonActions.ApplySelf<AlanWalkerPower>(choiceContext, this);
        await Cmd.Wait(0.5f);
        NinjaAudio.PlayLooped("res://LexNinja2/audio/Faded.mp3", 0.6f);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }

    public override string CustomPortraitPath => $"AlanWalker.png".BigCardImagePath();
    public override string PortraitPath => $"AlanWalker.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/AlanWalker.png".CardImagePath();
}
