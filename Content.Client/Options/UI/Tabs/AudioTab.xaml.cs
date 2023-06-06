using Content.Shared.CCVar;
using Content.Shared.Corvax.CCCVars;
using Robust.Client.AutoGenerated;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared;
using Robust.Shared.Configuration;
using Range = Robust.Client.UserInterface.Controls.Range;

namespace Content.Client.Options.UI.Tabs
{
    [GenerateTypedNameReferences]
    public sealed partial class AudioTab : Control
    {
        [Dependency] private readonly IConfigurationManager _cfg = default!;
        [Dependency] private readonly IClydeAudio _clydeAudio = default!;

        public AudioTab()
        {
            RobustXamlLoader.Load(this);
            IoCManager.InjectDependencies(this);

            LobbyMusicCheckBox.Pressed = _cfg.GetCVar(CCVars.LobbyMusicEnabled);
            RestartSoundsCheckBox.Pressed = _cfg.GetCVar(CCVars.RestartSoundsEnabled);
            EventMusicCheckBox.Pressed = _cfg.GetCVar(CCVars.EventMusicEnabled);
            AdminSoundsCheckBox.Pressed = _cfg.GetCVar(CCVars.AdminSoundsEnabled);

            ApplyButton.OnPressed += OnApplyButtonPressed;
            ResetButton.OnPressed += OnResetButtonPressed;
            MasterVolumeSlider.OnValueChanged += OnMasterVolumeSliderChanged;
            MidiVolumeSlider.OnValueChanged += OnMidiVolumeSliderChanged;
            AmbientMusicVolumeSlider.OnValueChanged += OnAmbientMusicVolumeSliderChanged;
            AmbienceVolumeSlider.OnValueChanged += OnAmbienceVolumeSliderChanged;
            AmbienceSoundsSlider.OnValueChanged += OnAmbienceSoundsSliderChanged;
            LobbyVolumeSlider.OnValueChanged += OnLobbyVolumeSliderChanged;
            TtsVolumeSlider.OnValueChanged += OnTtsVolumeSliderChanged; // Corvax-TTS
            TtsAnnounceVolumeSlider.OnValueChanged += OnTtsAnnounceVolumeSliderChanged;
            LobbyMusicCheckBox.OnToggled += OnLobbyMusicCheckToggled;
            RestartSoundsCheckBox.OnToggled += OnRestartSoundsCheckToggled;
            EventMusicCheckBox.OnToggled += OnEventMusicCheckToggled;
            AdminSoundsCheckBox.OnToggled += OnAdminSoundsCheckToggled;

            AmbienceSoundsSlider.MinValue = _cfg.GetCVar(CCVars.MinMaxAmbientSourcesConfigured);
            AmbienceSoundsSlider.MaxValue = _cfg.GetCVar(CCVars.MaxMaxAmbientSourcesConfigured);

            Reset();
        }

        protected override void Dispose(bool disposing)
        {
            ApplyButton.OnPressed -= OnApplyButtonPressed;
            ResetButton.OnPressed -= OnResetButtonPressed;
            MasterVolumeSlider.OnValueChanged -= OnMasterVolumeSliderChanged;
            MidiVolumeSlider.OnValueChanged -= OnMidiVolumeSliderChanged;
            AmbientMusicVolumeSlider.OnValueChanged -= OnAmbientMusicVolumeSliderChanged;
            AmbienceVolumeSlider.OnValueChanged -= OnAmbienceVolumeSliderChanged;
            LobbyVolumeSlider.OnValueChanged -= OnLobbyVolumeSliderChanged;
            TtsVolumeSlider.OnValueChanged -= OnTtsVolumeSliderChanged; // Corvax-TTS
            TtsAnnounceVolumeSlider.OnValueChanged -= OnTtsAnnounceVolumeSliderChanged;
            base.Dispose(disposing);
        }

        private void OnLobbyVolumeSliderChanged(Range obj)
        {
            UpdateChanges();
        }

        private void OnAmbientMusicVolumeSliderChanged(Range obj)
        {
            UpdateChanges();
        }

        private void OnAmbienceVolumeSliderChanged(Range obj)
        {
            UpdateChanges();
        }

        private void OnAmbienceSoundsSliderChanged(Range obj)
        {
            UpdateChanges();
        }

        private void OnMasterVolumeSliderChanged(Range range)
        {
            _clydeAudio.SetMasterVolume(MasterVolumeSlider.Value / 100);
            UpdateChanges();
        }

        private void OnMidiVolumeSliderChanged(Range range)
        {
            UpdateChanges();
        }

        // Corvax-TTS-Start
        private void OnTtsVolumeSliderChanged(Range obj)
        {
            UpdateChanges();
        }
        // Corvax-TTS-End

        private void OnTtsAnnounceVolumeSliderChanged(Range obj)
        {
            UpdateChanges();
        }

        private void OnTtsRadioVolumeSliderChanged(Range obj)
        {
            UpdateChanges();
        }

        private void OnLobbyMusicCheckToggled(BaseButton.ButtonEventArgs args)
        {
            UpdateChanges();
        }
        private void OnRestartSoundsCheckToggled(BaseButton.ButtonEventArgs args)
        {
            UpdateChanges();
        }
        private void OnEventMusicCheckToggled(BaseButton.ButtonEventArgs args)
        {
            UpdateChanges();
        }

        private void OnAdminSoundsCheckToggled(BaseButton.ButtonEventArgs args)
        {
            UpdateChanges();
        }

        private void OnApplyButtonPressed(BaseButton.ButtonEventArgs args)
        {
            _cfg.SetCVar(CVars.AudioMasterVolume, MasterVolumeSlider.Value / 100);
            // Want the CVar updated values to have the multiplier applied
            // For the UI we just display 0-100 still elsewhere
            _cfg.SetCVar(CVars.MidiVolume, LV100ToDB(MidiVolumeSlider.Value, CCVars.MidiMultiplier));
            _cfg.SetCVar(CCVars.AmbienceVolume, LV100ToDB(AmbienceVolumeSlider.Value, CCVars.AmbienceMultiplier));
             _cfg.SetCVar(CCVars.LobbyMusicVolume, LV100ToDB(LobbyVolumeSlider.Value));
            _cfg.SetCVar(CCCVars.TTSVolume, LV100ToDB(TtsVolumeSlider.Value)); // Corvax-TTS
            _cfg.SetCVar(CCCVars.TTSRadioVolume, LV100ToDB(TtsRadioVolumeSlider.Value));
            _cfg.SetCVar(CCCVars.TTSAnnounceVolume, LV100ToDB(TtsAnnounceVolumeSlider.Value));
            _cfg.SetCVar(CCVars.MaxAmbientSources, (int)AmbienceSoundsSlider.Value);
            _cfg.SetCVar(CCVars.LobbyMusicEnabled, LobbyMusicCheckBox.Pressed);
            _cfg.SetCVar(CCVars.RestartSoundsEnabled, RestartSoundsCheckBox.Pressed);
            _cfg.SetCVar(CCVars.EventMusicEnabled, EventMusicCheckBox.Pressed);
            _cfg.SetCVar(CCVars.AdminSoundsEnabled, AdminSoundsCheckBox.Pressed);
            _cfg.SaveToFile();
            UpdateChanges();
        }

        private void OnResetButtonPressed(BaseButton.ButtonEventArgs args)
        {
            Reset();
        }

        private void Reset()
        {
            MasterVolumeSlider.Value = _cfg.GetCVar(CVars.AudioMasterVolume) * 100;
            MidiVolumeSlider.Value = DBToLV100(_cfg.GetCVar(CVars.MidiVolume), CCVars.MidiMultiplier);
            AmbienceVolumeSlider.Value = DBToLV100(_cfg.GetCVar(CCVars.AmbienceVolume), CCVars.AmbienceMultiplier);
            LobbyVolumeSlider.Value = DBToLV100(_cfg.GetCVar(CCVars.LobbyMusicVolume));
            TtsVolumeSlider.Value = DBToLV100(_cfg.GetCVar(CCCVars.TTSVolume)); // Corvax-TTS
            TtsRadioVolumeSlider.Value = DBToLV100(_cfg.GetCVar(CCCVars.TTSRadioVolume));
            TtsAnnounceVolumeSlider.Value = DBToLV100(_cfg.GetCVar(CCCVars.TTSAnnounceVolume));
            AmbienceSoundsSlider.Value = _cfg.GetCVar(CCVars.MaxAmbientSources);
            LobbyMusicCheckBox.Pressed = _cfg.GetCVar(CCVars.LobbyMusicEnabled);
            RestartSoundsCheckBox.Pressed = _cfg.GetCVar(CCVars.RestartSoundsEnabled);
            EventMusicCheckBox.Pressed = _cfg.GetCVar(CCVars.EventMusicEnabled);
            AdminSoundsCheckBox.Pressed = _cfg.GetCVar(CCVars.AdminSoundsEnabled);
            UpdateChanges();
        }

        // Note: Rather than moving these functions somewhere, instead switch MidiManager to using linear units rather than dB
        // Do be sure to rename the setting though
        private float DBToLV100(float db, float multiplier = 1f)
        {
            var weh = (float) (Math.Pow(10, db / 10) * 100 / multiplier);
            return weh;
        }

        private float LV100ToDB(float lv100, float multiplier = 1f)
        {
            // Saving negative infinity doesn't work, so use -10000000 instead (MidiManager does it)
            var weh = MathF.Max(-10000000, (float) (Math.Log(lv100 * multiplier / 100, 10) * 10));
            return weh;
        }

        private void UpdateChanges()
        {
            var isMasterVolumeSame =
                Math.Abs(MasterVolumeSlider.Value - _cfg.GetCVar(CVars.AudioMasterVolume) * 100) < 0.01f;
            var isMidiVolumeSame =
                Math.Abs(MidiVolumeSlider.Value - DBToLV100(_cfg.GetCVar(CVars.MidiVolume), CCVars.MidiMultiplier)) < 0.01f;
            var isAmbientVolumeSame =
                Math.Abs(AmbienceVolumeSlider.Value - DBToLV100(_cfg.GetCVar(CCVars.AmbienceVolume), CCVars.AmbienceMultiplier)) < 0.01f;
            var isAmbientMusicVolumeSame =
                Math.Abs(AmbientMusicVolumeSlider.Value - DBToLV100(_cfg.GetCVar(CCVars.AmbientMusicVolume), CCVars.AmbientMusicMultiplier)) < 0.01f;
            var isLobbyVolumeSame =
                Math.Abs(LobbyVolumeSlider.Value - DBToLV100(_cfg.GetCVar(CCVars.LobbyMusicVolume))) < 0.01f;
            var isTtsVolumeSame =
                Math.Abs(TtsVolumeSlider.Value - DBToLV100(_cfg.GetCVar(CCCVars.TTSVolume))) < 0.01f; // Corvax-TTS
            var isTtsRadioVolumeSame =
                Math.Abs(TtsRadioVolumeSlider.Value - DBToLV100(_cfg.GetCVar(CCCVars.TTSRadioVolume))) < 0.01f;
            var isTtsAnnounceVolumeSame = Math.Abs(TtsAnnounceVolumeSlider.Value - DBToLV100(_cfg.GetCVar(CCCVars.TTSAnnounceVolume))) < 0.01f;
            var isAmbientSoundsSame = (int)AmbienceSoundsSlider.Value == _cfg.GetCVar(CCVars.MaxAmbientSources);
            var isLobbySame = LobbyMusicCheckBox.Pressed == _cfg.GetCVar(CCVars.LobbyMusicEnabled);
            var isRestartSoundsSame = RestartSoundsCheckBox.Pressed == _cfg.GetCVar(CCVars.RestartSoundsEnabled);
            var isEventSame = EventMusicCheckBox.Pressed == _cfg.GetCVar(CCVars.EventMusicEnabled);
            var isAdminSoundsSame = AdminSoundsCheckBox.Pressed == _cfg.GetCVar(CCVars.AdminSoundsEnabled);
            var isEverythingSame = isMasterVolumeSame && isMidiVolumeSame && isAmbientVolumeSame && isAmbientMusicVolumeSame && isAmbientSoundsSame && isLobbySame && isRestartSoundsSame && isEventSame && isAdminSoundsSame && isLobbyVolumeSame && isTtsRadioVolumeSame;
            isEverythingSame = isEverythingSame && isTtsVolumeSame && isTtsAnnounceVolumeSame; // Corvax-TTS
            ApplyButton.Disabled = isEverythingSame;
            ResetButton.Disabled = isEverythingSame;
            MasterVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", MasterVolumeSlider.Value / 100));
            MidiVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", MidiVolumeSlider.Value / 100));
           AmbientMusicVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", AmbientMusicVolumeSlider.Value / 100));
            AmbienceVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", AmbienceVolumeSlider.Value / 100));
            LobbyVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", LobbyVolumeSlider.Value / 100));
            TtsVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", TtsVolumeSlider.Value / 100)); // Corvax-TTS
            TtsRadioVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", TtsRadioVolumeSlider.Value / 100)); // Corvax-TTS
            TtsAnnounceVolumeLabel.Text =
                Loc.GetString("ui-options-volume-percent", ("volume", TtsAnnounceVolumeSlider.Value / 100));
            AmbienceSoundsLabel.Text = ((int)AmbienceSoundsSlider.Value).ToString();
        }
    }
}
