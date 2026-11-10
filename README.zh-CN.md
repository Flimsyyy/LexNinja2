# LexNinja2

[English](README.md) · **简体中文**

[![Release](https://img.shields.io/github/v/release/Flimsyyy/LexNinja2?include_prereleases&sort=semver&label=release)](https://github.com/Flimsyyy/LexNinja2/releases)
[![License](https://img.shields.io/badge/license-MIT-2e7d32)](LICENSE)
[![Game](https://img.shields.io/badge/Slay%20the%20Spire%202-0.111.0%2B-6a1b9a)](#开发)
[![Steam Workshop](https://img.shields.io/badge/Steam%20Workshop-subscribe-1b2838)](https://steamcommunity.com/sharedfiles/filedetails/?id=3747530622)

**蕾忍 Mod 的杀戮尖塔 2 版本。** 下载与玩法介绍见 [Steam 创意工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=3747530622)。

## 开发

**前置要求：**
- 安装 [.NET SDK 9](https://dotnet.microsoft.com/download/dotnet/9.0)
- 安装 [Godot Engine 4.5.1 Mono (.NET)](https://godotengine.org/download/archive/4.5.1-stable/)
- 在 Godot 中安装导出模板（编辑器 → 管理导出模板 → 下载并安装）

**开始使用：**

1. 克隆仓库：
   ```bash
   git clone https://github.com/Flimsyyy/LexNinja2.git
   cd LexNinja2
   ```

2. 还原 NuGet 包：
   ```bash
   dotnet restore
   ```

3. 安装 Spine-Godot 插件：
   - 从 [spine-godot-extension-4.2-4.5.1-stable.zip](https://spine-godot.s3.eu-central-1.amazonaws.com/4.2/4.5.1-stable/spine-godot-extension-4.2-4.5.1-stable.zip) 下载插件
   - 解压 ZIP 文件，找到其中的 `bin` 文件夹
   - 把整个 `bin` 文件夹直接复制到项目根目录（与 `project.godot` 同级）

4. 创建配置文件：
   - 在项目根目录创建名为 `Directory.Build.props` 的文件
   - 添加以下内容（替换为你自己的路径）：
     ```xml
     <Project>
         <PropertyGroup>
             <GodotPath>你的 Godot 安装目录</GodotPath>
             <SteamLibraryPath>包含《杀戮尖塔 2》的 Steam 库目录</SteamLibraryPath>
         </PropertyGroup>
     </Project>
     ```

5. 代码格式化：
   - 本项目使用 [CSharpier](https://csharpier.com/) 进行代码格式化
   - 安装 CSharpier（推荐作为 VS Code 或 Rider 插件）
   - 构建时会自动格式化，也可以手动运行格式化命令

6. 开始开发：
   - 使用 `dotnet build` 只编译 DLL 文件
   - 使用 `dotnet publish -c ExportRelease` 不仅编译 DLL，还会导出 PCK 文件，并把 JSON 文件（`ExportRelease` 配置不会复制 PDB 文件）复制到《杀戮尖塔 2》的 Mod 目录

7. 版本号：
   - 版本号由 Git 标签自动生成。
   - 如果当前提交正好位于某个标签上（例如 `v1.3.5`）且工作区干净（没有未提交的改动），版本号为短格式 `v1.3.5`。
   - 否则（有额外提交、工作区有改动，或没有标签），版本号为完整的 `git describe --long --dirty` 输出，例如 `v1.3.5-2-g123abcd` 或 `v1.3.5-0-g70ca280-dirty`。
   - 版本号会写入 `LexNinja2.json`，同时作为程序集的 `Version` 与 `FileVersion`。

8. 延伸阅读：
   - **如何启用调试器**：https://github.com/Alchyr/ModTemplate-StS2/wiki/Testing-and-Debugging#attaching-a-debugger
   - **杀戮尖塔 2 Mod 模板搭建指南**：https://github.com/Alchyr/ModTemplate-StS2/wiki
   - **BaseLib Wiki**：https://alchyr.github.io/BaseLib-Wiki/

## 许可

Mod 源码采用 [MIT 许可](LICENSE)。《杀戮尖塔 2》的游戏素材以及第三方库遵循各自的许可协议。

## 链接

- [Steam 创意工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=3747530622) · [Issues](https://github.com/Flimsyyy/LexNinja2/issues) · [Releases](https://github.com/Flimsyyy/LexNinja2/releases)
