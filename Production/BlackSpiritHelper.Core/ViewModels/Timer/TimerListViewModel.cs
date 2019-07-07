namespace BlackSpiritHelper.Core
{
    public class TimerListViewModel : BaseViewModel
    {
        /// <summary>
        /// Group List data structure.
        /// </summary>
        public TimerGroupListDesignModel TimerGroupListDesignModel { get; private set; } = IoC.DataContent.TimerGroupListDesignModel;
    }
}
