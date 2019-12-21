namespace BlackSpiritHelper.Core
{
    public class SoundAudioManager : BaseAudioManager
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SoundAudioManager()
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
            // Pack.
            AddPack(AudioType.AlertLongBefore, o =>
            {
                o.AddAudio(new AudioFile("AlertLongBefore_Sound_0.mp3"));
                return o;
            });
            // Pack.
            AddPack(AudioType.AlertClockTicking, o =>
            {
                o.AddAudio(new AudioFile("AlertClockTicking_Sound_0.mp3"));
                return o;
            });
            // Pack.
            AddPack(AudioType.Alert4, o =>
            {
                o.AddAudio(new AudioFile("Alert4_Sound_0.mp3"));
                return o;
            });
            // Pack.
            AddPack(AudioType.Alert5, o =>
            {
                o.AddAudio(new AudioFile("Alert5_Sound_0.mp3"));
                return o;
            });
        }

        #endregion
    }
}
