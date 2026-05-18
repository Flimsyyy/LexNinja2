using BaseLib.Abstracts;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Character;

namespace LexNinja2.LexNinja2Code.Potions;

[Pool(typeof(LexNinja2PotionPool))]
public abstract class LexNinja2Potion : CustomPotionModel;
