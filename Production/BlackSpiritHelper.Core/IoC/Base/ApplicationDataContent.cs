namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Subclass for <see cref="IoC"/>. There you define all application data structures.
    /// F.e. Timer structure is a bit complex with its groups <see cref="TimerGroupListViewModel"/>. You have to bind the same view model to multiple views.
    /// FOr this reason, you define these data structures here to easily access it as a singleton.
    /// </summary>
    public class ApplicationDataContent
    {
        /// <summary>
        /// Data structure for timers with its groups.
        /// </summary>
        public TimerGroupListDesignModel TimerGroupListDesignModel { get; private set; } = TimerGroupListDesignModel.Instance;
    }
}
