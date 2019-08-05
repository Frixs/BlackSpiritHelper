using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{

    public class ScheduleViewModel : DataContentBaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Selected template ID.
        /// </summary>
        public int SelectedTemplateID { get; set; } = 0;

        /// <summary>
        /// Template list, predefined.
        /// </summary>
        [XmlIgnore]
        public List<ScheduleTemplateDataViewModel> TemplatePredefinedList { get; protected set; }

        /// <summary>
        /// Template list, custom.
        /// </summary>
        public ObservableCollection<ScheduleTemplateDataViewModel> TemplateCustomList { get; set; }

        /// <summary>
        /// Item list, predefined.
        /// </summary>
        [XmlIgnore]
        public List<ScheduleItemDataViewModel> ItemPredefinedList { get; protected set; }

        /// <summary>
        /// Item list, custom.
        /// </summary>
        public ObservableCollection<ScheduleItemDataViewModel> ItemCustomList { get; set; }

        /// <summary>
        /// Offset modifier for the local time.
        /// </summary>
        [XmlIgnore]
        public TimeSpan LocalTimeOffsetModifier { get; set; }

        /// <summary>
        /// <see cref="LocalTimeOffsetModifier"/> Ticks.
        /// It is used to store <see cref="LocalTimeOffsetModifier"/> in user settings.
        /// </summary>
        public long LocalTimeOffsetModifierTicks { get; set; }

        /// <summary>
        /// TODO IsRunning.
        /// </summary>
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

        #region Public Methods

        /// <summary>
        /// Add a new item.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="colorHex"></param>
        /// <param name="isPredefined"></param>
        /// <returns></returns>
        public ScheduleItemDataViewModel AddItem(string name, string colorHex, bool isPredefined = false)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(colorHex))
                return null;

            if (!colorHex.CheckColorHEX())
                return null;

            // Check duplicity.
            if (ItemPredefinedList.FirstOrDefault(o => o.Name.ToLower().Equals(name.ToLower().Trim())) != null)
                return null;

            if (!isPredefined)
            {
                if (ItemCustomList.FirstOrDefault(o => o.Name.ToLower().Equals(name.ToLower().Trim())) != null)
                    return null;
            }

            return new ScheduleItemDataViewModel
            {
                Name = name.Trim(),
                ColorHEX = colorHex,
            };
        }

        #endregion
    }
}
