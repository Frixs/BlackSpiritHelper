namespace BlackSpiritHelper.Core
{
    public class TimerGroupMenuItemViewModel
    {
        /// <summary>
        /// ID of the group.
        /// </summary>
        public byte ID { get; set; }

        /// <summary>
        /// Group Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Button control - says if any of the child timers are active.
        /// </summary>
        public bool IsRunning { get; set; }
    }
}
