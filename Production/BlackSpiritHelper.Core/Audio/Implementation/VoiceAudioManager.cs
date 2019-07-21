namespace BlackSpiritHelper.Core
{
    public class VoiceAudioManager : BaseAudioManager
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public VoiceAudioManager()
        {
            // Pack.
            AddPack(AudioType.Alert3, o =>
            {
                o.AddAudio(new AudioFile("Alert3_Sound_0.mp3"));
                return o;
            });
            // Pack.
            AddPack(AudioType.Alert2, o =>
            {
                o.AddAudio(new AudioFile("Alert2_Sound_0.mp3"));
                return o;
            });
            // Pack.
            AddPack(AudioType.Alert1, o =>
            {
                o.AddAudio(new AudioFile("Alert1_Sound_0.mp3"));
                return o;
            });
            // Pack.
            AddPack(AudioType.AlertCountdown, o =>
            {
                o.AddAudio(new AudioFile("AlertCounting_Sound_0.mp3"));
                return o;
            });
        }

        #endregion
    }
}
