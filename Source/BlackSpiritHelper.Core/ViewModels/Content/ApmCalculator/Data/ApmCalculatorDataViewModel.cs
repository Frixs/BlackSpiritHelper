using System;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// View model that represents the APM calculator base
    /// </summary>
    public class ApmCalculatorDataViewModel : ADataContentBaseViewModel<ApmCalculatorDataViewModel>
    {
        #region Public Properties

        /// <summary>
        /// Current session data
        /// </summary>
        [XmlIgnore]
        public ApmCalculatorSessionDataViewModel CurrentSession { get; set; }

        /// <inheritdoc/>
        [XmlIgnore]
        public override bool IsRunning 
        {
            get => false;
            protected set => throw new NotImplementedException();
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApmCalculatorDataViewModel()
        {

        }

        /// <inheritdoc/>
        protected override void DisposeRoutine()
        {
        }

        /// <inheritdoc/>
        protected override void InitRoutine(params object[] parameters)
        {
        }

        /// <inheritdoc/>
        protected override void SetDefaultsRoutine()
        {
        }

        #endregion
    }
}
