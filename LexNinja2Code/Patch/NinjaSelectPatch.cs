using BaseLib.Abstracts;
using HarmonyLib;
using LexNinja2.LexNinja2Code.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;

namespace LexNinja2.LexNinja2Code.Patch;

[HarmonyPatch(typeof(NCharacterSelectScreen), "SelectCharacter")]
public static class NinjaSelectPatch
{
    static void Prefix(NCharacterSelectButton charSelectButton, CharacterModel characterModel)
    {
        // 在这里播放你的自定义音效
        // if (characterModel==ModelDb.Character<Character.LexNinja2>())
        // {
        //     MegaCrit.Sts2.Core.Commands.Cmd.Wait(0.3f);
        //     NinjaAudio.Play("res://LexNinja2/audio/pick.mp3");
        // }
    }
}
