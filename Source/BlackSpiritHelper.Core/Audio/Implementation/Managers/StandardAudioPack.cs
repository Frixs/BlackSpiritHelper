namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Standard audio pack
    /// </summary>
    public sealed class StandardAudioPack : BaseAudioPack
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public StandardAudioPack()
        {
            // Sample
            AddAudioSample(AudioSampleType.Alert3, o =>
            {
                o.AddAudioFile(new AudioFile("Alert3_Sound_0.mp3"));
                return o;
            });
            // Sample
            AddAudioSample(AudioSampleType.Alert2, o =>
            {
                o.AddAudioFile(new AudioFile("Alert2_Sound_0.mp3"));
                return o;
            });
            // Sample
            AddAudioSample(AudioSampleType.Alert1, o =>
            {
                o.AddAudioFile(new AudioFile("Alert1_Sound_0.mp3"));
                return o;
            });
            // Sample
            AddAudioSample(AudioSampleType.AlertCountdown, o =>
            {
                o.AddAudioFile(new AudioFile("AlertCounting_Sound_0.mp3"));
                return o;
            });
            // Sample
            AddAudioSample(AudioSampleType.AlertLongBefore, o =>
            {
                o.AddAudioFile(new AudioFile("AlertLongBefore_Sound_0.mp3"));
                return o;
            });
            // Sample
            AddAudioSample(AudioSampleType.AlertClockTicking, o =>
            {
                o.AddAudioFile(new AudioFile("AlertClockTicking_Sound_0.mp3"));
                return o;
            });
            // Sample
            AddAudioSample(AudioSampleType.Alert4, o =>
            {
                o.AddAudioFile(new AudioFile("Alert4_Sound_0.mp3"));
                return o;
            });
            // Sample
            AddAudioSample(AudioSampleType.Alert5, o =>
            {
                o.AddAudioFile(new AudioFile("Alert5_Sound_0.mp3"));
                return o;
            });
            // Sample
            AddAudioSample(AudioSampleType.StartNotification1, o =>
            {
                o.AddAudioFile(new AudioFile("StartNotification1_Sound_0.mp3"));
                return o;
            });
            // Sample
            AddAudioSample(AudioSampleType.StopNotification1, o =>
            {
                o.AddAudioFile(new AudioFile("StopNotification1_Sound_0.mp3"));
                return o;
            });
            // Sample
            AddAudioSample(AudioSampleType.PingNotification1, o =>
            {
                o.AddAudioFile(new AudioFile("PingNotification1_Sound_0.mp3"));
                return o;
            });
        }

        #endregion
    }
}
