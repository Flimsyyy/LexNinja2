using LexNinja2.LexNinja2Code.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace LexNinja2.LexNinja2Code.Api.Potions;

[RegisterPotion(typeof(LexNinja2PotionPool), Inherit = true)]
public abstract class LexNinja2Potion : ModPotionTemplate;
