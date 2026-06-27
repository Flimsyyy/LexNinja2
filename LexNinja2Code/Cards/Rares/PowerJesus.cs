using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Cards.Rares;

public class PowerJesus() : LexNinja2Card(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (NinjaConfigsPage.IsChallengeMode())
        {
            var list = PileType.Hand.GetPile(Owner).Cards.ToList();
            foreach (var item in list)
            {
                await CardCmd.Exhaust(choiceContext, item);
            }
        }

        NinjaAudio.Play("res://LexNinja2/audio/PowerJesus.mp3");
        await NinjaAnim.TriggerCastAnim(this);

        var buffs = Owner
            .Creature.Powers.Where(p => p.TypeForCurrentAmount == PowerType.Buff)
            .Where(p => p.StackType == PowerStackType.Counter)
            .ToList();
        foreach (var buff in buffs)
        {
            if (buff.InstanceType == PowerInstanceType.None)
            {
                await PowerCmd.Apply(
                    choiceContext,
                    buff,
                    Owner.Creature,
                    buff.Amount,
                    Owner.Creature,
                    this
                );
                continue;
            }
            if (buff.ClonePreservingMutability() is not PowerModel buffClone)
            {
                continue;
            }
            var data = buff.GetInternalData();
            if (data != null)
            {
                var dataClone = NinjaHelper.CloneData(data);
                if (dataClone == null)
                {
                    continue;
                }
                buffClone.SetInternalData(dataClone);
            }
            await PowerCmd.Apply(
                choiceContext,
                buffClone,
                Owner.Creature,
                buff.Amount,
                Owner.Creature,
                this
            );
        }
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }

    public override string CustomPortraitPath => $"PowerJesus_p.png".BigCardImagePath();
    public override string PortraitPath => $"PowerJesus.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/PowerJesus.png".CardImagePath();
}
