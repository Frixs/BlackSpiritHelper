namespace BlackSpiritHelper.Core
{
    public class ScheduleViewModel : DataContentBaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Selected region.
        /// </summary>
        public Region SelectedRegion { get; set; } = Region.EU;

        public override bool IsRunning => throw new System.NotImplementedException();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScheduleViewModel()
        {
        }

        protected override void SetDefaultsMethod()
        {
        }

        protected override void SetupMethod()
        {
        }

        #endregion


    }
}
