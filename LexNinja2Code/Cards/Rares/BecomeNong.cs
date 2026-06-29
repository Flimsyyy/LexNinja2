using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Cards.Rares;

public class BecomeNong() : LexNinja2Card(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BecomeNongPower>(1)];
    public override bool CanBeGeneratedInCombat => false;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await NinjaAnim.TriggerCastAnim(this);
        NinjaAudio.Play("res://LexNinja2/audio/BecomeNong.mp3");
        CardModel? card;
        if (!NinjaConfig.IsChallengeMode() || IsUpgraded)
        {
            card = await CommonActions.SelectSingleCard(
                this,
                SelectionScreenPrompt,
                choiceContext,
                PileType.Hand
            );
        }
        else
        {
            var pile = PileType.Hand.GetPile(Owner);
            card = Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards);
        }

        if (card == null)
        {
            return;
        }
        var power = await CommonActions.ApplySelf<BecomeNongPower>(choiceContext, this);
        power?.SetSelectedCard(card);
    }

    protected override void OnUpgrade()
    {
        if (!NinjaConfig.IsChallengeMode())
        {
            AddKeyword(CardKeyword.Retain);
        }
    }

    public override string CustomPortraitPath => $"BecomeNong_p.png".BigCardImagePath();
    public override string PortraitPath => $"BecomeNong.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/BecomeNong.png".CardImagePath();
}
