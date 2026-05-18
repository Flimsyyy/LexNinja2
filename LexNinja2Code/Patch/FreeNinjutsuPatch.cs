using System.Reflection;
using HarmonyLib;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Cards;

namespace LexNinja2.LexNinja2Code.Patch
{
    [HarmonyPatch]
    public static class FreeNinjutsuPatch
    {
        // 返回所有名为 Ninjutsu 且无参数、返回 bool 的实例方法
        static IEnumerable<MethodBase> TargetMethods()
        {
            return Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .SelectMany(t =>
                    t.GetMethods(
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                    )
                )
                .Where(m =>
                    (m.Name == "Ninjutsu" || m.Name == "CanCastNinjutsu")
                    && m.ReturnType == typeof(bool)
                    && m.GetParameters().Length == 0
                );
        }

        static bool Prefix(ref bool __result, object __instance, MethodBase __originalMethod)
        {
            // 确保卡牌实例存在且是 Card 的派生类
            if (__instance is not LexNinja2Card card)
                return true;

            // 只对标记为忍术的卡牌生效（可选，防止干扰同名非忍术方法）
            if (card.Tags == null || !card.Tags.Contains(NinjaTags.Ninjutsu))
                return true;

            // 检查是否拥有“免费忍术”关键词
            if (card.Keywords != null && card.Keywords.Contains(NinjaKeyword.FreeNinjutsu))
            {
                __result = true; // 直接返回可以释放
                if (__originalMethod.Name == "Ninjutsu")
                {
                    card.RemoveKeyword(NinjaKeyword.FreeNinjutsu);
                }
                return false; // 跳过原方法
            }

            return true; // 不满足免费条件，正常执行原逻辑
        }
    }
}
