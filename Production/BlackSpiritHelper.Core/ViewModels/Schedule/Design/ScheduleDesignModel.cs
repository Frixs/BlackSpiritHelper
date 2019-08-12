namespace BlackSpiritHelper.Core
{

    public class ScheduleDesignModel : ScheduleViewModel
    {
        #region New Instance Getter

        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
        public static ScheduleDesignModel Instance
        {
            get
            {
                ScheduleDesignModel o = new ScheduleDesignModel();
                return o;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScheduleDesignModel()
        {
            Init();
        }

        #endregion

        #region Initialize Predefined Values

        private void Init()
        {
            // Schedule predefined Item list.
            AddItem("Kzarka", "fc2121", true);
            AddItem("Karanda", "004fcf", true);
            AddItem("Offin", "0087e0", true);
            AddItem("Nouver", "e68507", true);
            AddItem("Kutum", "7f07b8", true);
            AddItem("Vell", "009677", true);
            AddItem("Garmoth", "d60229", true);
            AddItem("Quint", "752300", true);
            AddItem("Muraka", "752300", true);

            // Schedule predefined Template list.
            AddPredefinedTemplate("BDO-EU");
            AddPredefinedTemplate("BDO-JP");
            AddPredefinedTemplate("BDO-KR");
            AddPredefinedTemplate("BDO-MENA");
            AddPredefinedTemplate("BDO-NA");
            AddPredefinedTemplate("BDO-RU");
            AddPredefinedTemplate("BDO-SA");
            AddPredefinedTemplate("BDO-SEA");
            AddPredefinedTemplate("BDO-TH");
            AddPredefinedTemplate("BDO-TW");
        }

        #endregion
    }
}
