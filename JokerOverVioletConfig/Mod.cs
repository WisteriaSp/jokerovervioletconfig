using JokerOverVioletConfig.Configuration;
using JokerOverVioletConfig.Template;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using CriFs.V2.Hook.Interfaces;
using BF.File.Emulator.Interfaces;
using BMD.File.Emulator.Interfaces;
using PAK.Stream.Emulator.Interfaces;
using SPD.File.Emulator.Interfaces;
using P5R.CostumeFramework.Interfaces;
using CriExtensions;

namespace JokerOverVioletConfig
{
    public class Mod : ModBase
    {
        private readonly IModLoader _modLoader;
        private readonly IReloadedHooks? _hooks;
        private readonly ILogger _logger;
        private readonly IMod _owner;
        private Config _configuration;
        private readonly IModConfig _modConfig;

        public Mod(ModContext context)
        {
            _modLoader = context.ModLoader;
            _hooks = context.Hooks;
            _logger = context.Logger;
            _owner = context.Owner;
            _configuration = context.Configuration;
            _modConfig = context.ModConfig;

            string modDir = _modLoader.GetDirectoryForModId(_modConfig.ModId);
            string modId = _modConfig.ModId;

            // Initialize file emulator controllers
            var criFsCtl = _modLoader.GetController<ICriFsRedirectorApi>();
            var bfEmuCtl = _modLoader.GetController<IBfEmulator>();
            var bmdEmuCtl = _modLoader.GetController<IBmdEmulator>();
            var pakEmuCtl = _modLoader.GetController<IPakEmulator>();
            var spdEmuCtl = _modLoader.GetController<ISpdEmulator>();
            var costumeCtl = _modLoader.GetController<ICostumeApi>();

            if (criFsCtl == null || !criFsCtl.TryGetTarget(out var criFsApi)) { _logger.WriteLine("CRI FS missing → cpk and binds broken.", System.Drawing.Color.Red); return; }
            if (bfEmuCtl == null || !bfEmuCtl.TryGetTarget(out var bfEmu)) { _logger.WriteLine("BF Emu missing → BF merges broken.", System.Drawing.Color.Red); return; }
            if (bmdEmuCtl == null || !bmdEmuCtl.TryGetTarget(out var bmdEmu)) { _logger.WriteLine("BMD Emu missing → BMD merges broken.", System.Drawing.Color.Red); return; }
            if (pakEmuCtl == null || !pakEmuCtl.TryGetTarget(out var pakEmu)) { _logger.WriteLine("PAK Emu missing → PAK merges broken.", System.Drawing.Color.Red); return; }
            if (spdEmuCtl == null || !spdEmuCtl.TryGetTarget(out var spdEmu)) { _logger.WriteLine("SPD Emu missing → SPD merges broken.", System.Drawing.Color.Red); return; }
            if (costumeCtl == null || !costumeCtl.TryGetTarget(out var costumeApi)) { _logger.WriteLine("Costume API missing → Costumes broken.", System.Drawing.Color.Red); return; }

            var mods = _modLoader.GetActiveMods();
            var isKasumiProtagActive = mods.Any(x => x.Generic.ModId == "p5rpc.kasumiasprotag");
            _logger.WriteLine($"Is Kasumi as Protagonist active? {isKasumiProtagActive}", System.Drawing.Color.Magenta);

            // Darkened Face
            if (_configuration.DarkenedFaceJoker)
                BindAllFilesIn(Path.Combine("OptionalModFiles", "Model", "DarkenedFace"), modDir, criFsApi, modId);

            // Kasumi as Protag Compatibility
            if (_configuration.ProtagSumiCompatAuto || isKasumiProtagActive) // Automatically enables the config if the mod is found
            {
                if (!_configuration.ProtagSumiCompat && isKasumiProtagActive && _configuration.ProtagSumiCompatAuto)
                {
                    _logger.WriteLine($"Kasumi as Protag detected, auto-enabling ProtagSumiCompat.", System.Drawing.Color.Green);
                    _configuration.ProtagSumiCompat = true;
                }
            }
            if (_configuration.ProtagSumiCompat)
            {
                BindAllFilesIn(Path.Combine("OptionalModFiles", "Compat", "ProtagSumi", "Bind"), modDir, criFsApi, modId);
                bmdEmu.AddDirectory(Path.Combine(modDir, "OptionalModFiles", "Compat", "ProtagSumi", "BMD"));
            }

            // Skillset
            if (_configuration.SkillsetJoker)
            {
                criFsApi.AddProbingPath("OptionalModFiles\\Skillset");
            }

            // NameChange
            if (_configuration.NameChange)
            {
                criFsApi.AddProbingPath("OptionalModFiles\\Akira");
                pakEmu.AddDirectory(Path.Combine(modDir, "OptionalModFiles", "Akira", "PAK"));
                bmdEmu.AddDirectory(Path.Combine(modDir, "OptionalModFiles", "Akira", "BMD"));
            }
        }

        private static void BindAllFilesIn(string subPathRelativeToModDir, string modDir, ICriFsRedirectorApi criFsApi, string modId)
        {
            var absoluteFolder = Path.Combine(modDir, subPathRelativeToModDir);
            if (!Directory.Exists(absoluteFolder)) return;
            foreach (var file in Directory.EnumerateFiles(absoluteFolder, "*", SearchOption.AllDirectories))
                criFsApi.AddBind(file, Path.GetRelativePath(absoluteFolder, file).Replace(Path.DirectorySeparatorChar, '/'), modId);
        }

        public override void ConfigurationUpdated(Config configuration)
        {
            _configuration = configuration;
            _logger.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");
        }

#pragma warning disable CS8618
        public Mod() { }
#pragma warning restore CS8618
    }
}