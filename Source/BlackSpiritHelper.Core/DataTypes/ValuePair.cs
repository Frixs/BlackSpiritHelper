namespace BlackSpiritHelper.Core
{
    public class ValuePair<ValA, ValB>
    {
        #region Public Properties

        public ValA ValueA { get; set; }
        public ValB ValueB { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ValuePair() { }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ValuePair(ValA valueA, ValB valueB)
        {
            ValueA = valueA;
            ValueB = valueB;
        }

        #endregion
    }
}
