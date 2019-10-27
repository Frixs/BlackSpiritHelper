namespace BlackSpiritHelper.Core
{
    public class Vector2Double
    {
        #region Public Properties

        public double X { get; set; } = 0;
        public double Y { get; set; } = 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Vector2Double()
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Vector2Double(double x, double y)
        {
            X = x;
            Y = y;
        }

        #endregion
    }
}
