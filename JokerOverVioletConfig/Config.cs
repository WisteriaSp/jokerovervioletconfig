using JokerOverVioletConfig.Template.Configuration;
using Reloaded.Mod.Interfaces.Structs;
using System.ComponentModel;
using CriFs.V2.Hook;
using CriFs.V2.Hook.Interfaces;
using System.Reflection;

namespace JokerOverVioletConfig.Configuration
{
	public class Config : Configurable<Config>
	{
        /*
            User Properties:
                - Please put all of your configurable properties here.

            By default, configuration saves as "Config.json" in mod user config folder.    
            Need more config files/classes? See Configuration.cs

            Available Attributes:
            - Category
            - DisplayName
            - Description
            - DefaultValue

            // Technically Supported but not Useful
            - Browsable
            - Localizable

            The `DefaultValue` attribute is used as part of the `Reset` button in Reloaded-Launcher.
        */

        [Category("Gameplay")]
        [DisplayName("Full Skillset Edit")]
        [Description("Adds proper skillsets for Arsene/Raoul/Satanael.")]
        [DefaultValue(true)]
        public bool SkillsetJoker { get; set; } = true;

        [Category("Compatibility")]
        [DisplayName("Kasumi as Protag Compatibility")]
        [Description("Enables compatibility with Kasumi as Protag. This will be auto-enabled if Kasumi as Protag is enabled and if this mod is below it.")]
        [DefaultValue(false)]
        public bool ProtagSumiCompat { get; set; } = false;

        [Category("Compatibility")]
        [DisplayName("Auto Compatibility")]
        [Description("Automatically enables compatibility with Kasumi as Protag if the mod is detected.")]
        [DefaultValue(true)]
        public bool ProtagSumiCompatAuto { get; set; } = false;

        [Category("Bustup")]
        [DisplayName("placeholder")]
        [Description("placeholder")]
        [DefaultValue(false)]
        public bool Bustup1 { get; set; } = false;

        [Category("Model")]
        [DisplayName("Undarkened Face")]
        [Description("Removes the face darkening when summoning a persona. Meant to be used with the No Darkened Faces mod, disable if you'd like.")]
        [DefaultValue(true)]
        public bool DarkenedFaceJoker { get; set; } = true;

        [Category("Misc")]
        [DisplayName("Change name to Akira Kurusu")]
        [Description("Changes the name from Ren to Akira Kurusu.")]
        [DefaultValue(false)]
        public bool NameChange { get; set; } = false;
    }

    /// <summary>
    /// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
    /// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
    /// </summary>
	public class ConfiguratorMixin : ConfiguratorMixinBase
	{
		// 
	}
}