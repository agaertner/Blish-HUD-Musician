using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nekres.Musician.Controls;
using Nekres.Musician.Core.Instrument;
using Nekres.Musician.Core.Player;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD.Graphics.UI;
using Nekres.Musician.UI;
using Nekres.Musician.UI.Models;
using Nekres.Musician.UI.Views;
using static Blish_HUD.GameService;

namespace Nekres.Musician
{

    [Export(typeof(Module))]
    public class MusicianModule : Module
    {
        internal static readonly Logger Logger = Logger.GetLogger(typeof(MusicianModule));

        internal static MusicianModule ModuleInstance;

        #region Service Managers
        internal SettingsManager SettingsManager => this.ModuleParameters.SettingsManager;
        internal ContentsManager ContentsManager => this.ModuleParameters.ContentsManager;
        internal DirectoriesManager DirectoriesManager => this.ModuleParameters.DirectoriesManager;
        internal Gw2ApiManager Gw2ApiManager => this.ModuleParameters.Gw2ApiManager;
        #endregion


        #region Settings

        internal SettingEntry<float> audioVolume;
        internal SettingEntry<KeyBinding> keySwapWeapons;
        internal SettingEntry<KeyBinding> keyWeaponSkill1;
        internal SettingEntry<KeyBinding> keyWeaponSkill2;
        internal SettingEntry<KeyBinding> keyWeaponSkill3;
        internal SettingEntry<KeyBinding> keyWeaponSkill4;
        internal SettingEntry<KeyBinding> keyWeaponSkill5;
        internal SettingEntry<KeyBinding> keyHealingSkill;
        internal SettingEntry<KeyBinding> keyUtilitySkill1;
        internal SettingEntry<KeyBinding> keyUtilitySkill2;
        internal SettingEntry<KeyBinding> keyUtilitySkill3;
        internal SettingEntry<KeyBinding> keyEliteSkill;

        #endregion

        private CornerIcon _moduleIcon;

        private WindowTab _moduleTab;

        internal MusicPlayer MusicPlayer { get; private set; }
        internal MusicSheetFactory MusicSheetFactory { get; private set; }

        [ImportingConstructor]
        public MusicianModule([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters) { ModuleInstance = this; }

        protected override void DefineSettings(SettingCollection settingsManager)
        {
            audioVolume = settingsManager.DefineSetting("audioVolume", 80f, () => "Audio Volume");

            var skillKeyBindingsCollection = settingsManager.AddSubCollection("Skills", true, false);
            keySwapWeapons = skillKeyBindingsCollection.DefineSetting("keySwapWeapons", new KeyBinding(Keys.OemPipe), () => "Swap Weapons");
            keyWeaponSkill1 = skillKeyBindingsCollection.DefineSetting("keyWeaponSkill1", new KeyBinding(Keys.D1), () => "Weapon Skill 1");
            keyWeaponSkill2 = skillKeyBindingsCollection.DefineSetting("keyWeaponSkill2", new KeyBinding(Keys.D2), () => "Weapon Skill 2");
            keyWeaponSkill3 = skillKeyBindingsCollection.DefineSetting("keyWeaponSkill3", new KeyBinding(Keys.D3), () => "Weapon Skill 3");
            keyWeaponSkill4 = skillKeyBindingsCollection.DefineSetting("keyWeaponSkill4", new KeyBinding(Keys.D4), () => "Weapon Skill 4");
            keyWeaponSkill5 = skillKeyBindingsCollection.DefineSetting("keyWeaponSkill5", new KeyBinding(Keys.D5), () => "Weapon Skill 5");
            keyHealingSkill = skillKeyBindingsCollection.DefineSetting("keyHealingSkill", new KeyBinding(Keys.D6), () => "Healing Skill");
            keyUtilitySkill1 = skillKeyBindingsCollection.DefineSetting("keyUtilitySkill1", new KeyBinding(Keys.D7), () => "Utility Skill 1");
            keyUtilitySkill2 = skillKeyBindingsCollection.DefineSetting("keyUtilitySkill2", new KeyBinding(Keys.D8), () => "Utility Skill 2");
            keyUtilitySkill3 = skillKeyBindingsCollection.DefineSetting("keyUtilitySkill3", new KeyBinding(Keys.D9), () => "Utility Skill 3");
            keyEliteSkill = skillKeyBindingsCollection.DefineSetting("keyEliteSkill", new KeyBinding(Keys.D0), () => "Elite Skill");
        }

        protected override void Initialize()
        {
            MusicSheetFactory = new MusicSheetFactory(DirectoriesManager.GetFullDirectoryPath("musician"));
            MusicPlayer = new MusicPlayer();
        }

        public override IView GetSettingsView() => new CustomSettingsView(new CustomSettingsModel(this.SettingsManager.ModuleSettings));

        protected override async Task LoadAsync()
        {
            await MusicSheetFactory.LoadAsync();
            await MusicPlayer.LoadAsync();
        }

        protected override void OnModuleLoaded(EventArgs e)
        {
            var icon = ContentsManager.GetTexture("musician_icon.png");
            _moduleIcon = new CornerIcon(ContentsManager.GetTexture("musician_icon.png"), this.Name);

            _moduleTab = Overlay.BlishHudWindow.AddTab(this.Name, icon, () => new LibraryView(new LibraryModel(MusicSheetFactory)));

            base.OnModuleLoaded(e);
        }

        protected override void Unload()
        {
            Overlay.BlishHudWindow.RemoveTab(_moduleTab);
            _moduleIcon?.Dispose();
            MusicPlayer?.Dispose();
            ModuleInstance = null;
        }
    }
}
