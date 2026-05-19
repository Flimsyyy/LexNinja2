using System.Collections;
using BaseLib.Utils;
using Godot;
using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace LexNinja2.LexNinja2Code;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "LexNinja2"; //Used for resource filepath

    // private static bool _audioInitTriggered = false;

    public static Logger Logger { get; } = new(ModId, LogType.Generic);

    public static void Initialize()
    {
        var harmony = new Harmony(ModId);

        harmony.PatchAll();

        ScriptManagerBridge.LookupScriptsInAssembly(typeof(MainFile).Assembly);

        try
        {
            var animHandlerField = AccessTools.Field(typeof(CustomAnimation), "animHandler");
            if (animHandlerField == null)
            {
                // 类或字段根本不存在，可能 BaseLib 没加载或版本不对
                Logger.Info("CustomAnimation.animHandler not found, skipping disable.");
                return;
            }

            var dict = animHandlerField.GetValue(null) as IDictionary;
            if (dict != null)
            {
                dict.Clear(); // 清空所有已注册的动画处理器
                Logger.Info("Successfully cleared CustomAnimation handlers.");
            }
            else
            {
                // 字段可能为空，尝试设置为空字典或 null（根据实际类型）
                animHandlerField.SetValue(null, null);
            }
        }
        catch (Exception ex)
        {
            Logger.Warn($"Failed to disable CustomAnimation: {ex.Message}");
        }
        // // ✅ 订阅第一帧事件，提前创建 NinjaAudio 单例
        // if (!_audioInitTriggered)
        // {
        //     _audioInitTriggered = true;
        //     var mainLoop = Engine.GetMainLoop();
        //     if (mainLoop is SceneTree sceneTree)
        //     {
        //         // 使用 ProcessFrame 确保在主循环稳定后执行一次
        //         sceneTree.ProcessFrame += OnFirstProcessFrame;
        //     }
        // }
    }
    // private static void OnFirstProcessFrame()
    // {
    //     var mainLoop = Engine.GetMainLoop();
    //     if (mainLoop is SceneTree sceneTree)
    //     {
    //         sceneTree.ProcessFrame -= OnFirstProcessFrame;
    //         // 再延迟一帧，让引擎彻底空闲
    //         sceneTree.ProcessFrame += OnSecondProcessFrame;
    //     }
    // }
    //
    // private static void OnSecondProcessFrame()
    // {
    //     var mainLoop = Engine.GetMainLoop();
    //     if (mainLoop is SceneTree sceneTree)
    //     {
    //         sceneTree.ProcessFrame -= OnSecondProcessFrame;
    //     }
    //
    //     // 现在安全创建单例
    //     // 注意： NinjaAudio.Instance 内部也会进一步延迟操作
    //     var instance = NinjaAudio.Instance;
    //     GD.Print($"[MainFile] NinjaAudio 单例创建结果：{(instance != null ? "成功" : "失败")}");
    // }
}
