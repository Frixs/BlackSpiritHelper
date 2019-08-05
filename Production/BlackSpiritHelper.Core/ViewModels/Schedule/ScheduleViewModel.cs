using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{

    public class ScheduleViewModel : DataContentBaseViewModel
    {
        #region Private Members

        /// <summary>
        /// Item list, custom.
        /// </summary>
        private ObservableCollection<ScheduleItemDataViewModel> mItemCustomList;

        #endregion

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
        public ObservableCollection<ScheduleItemDataViewModel> ItemCustomList
        {
            get => mItemCustomList;
            set
            {
                mItemCustomList = value;
                CheckDuplicityCustom();
            }
        }

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
            // Initialize templates.
            for (int i = 0; i < TemplatePredefinedList.Count; i++)
                TemplatePredefinedList[i].Init();
            for (int i = 0; i < TemplateCustomList.Count; i++)
                TemplateCustomList[i].Init();
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
                // Check duplicity.
                if (ItemCustomList.FirstOrDefault(o => o.Name.ToLower().Equals(name.ToLower().Trim())) != null)
                    return null;
            }

            // Create the item.
            var item = new ScheduleItemDataViewModel
            {
                Name = name.Trim(),
                ColorHEX = colorHex,
            };

            // Ad the item to the list.
            if (isPredefined)
                ItemPredefinedList.Add(item);
            else
                ItemCustomList.Add(item);

            return item;
        }

        /// <summary>
        /// Get item by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ScheduleItemDataViewModel GetItemByName(string name)
        {
            ScheduleItemDataViewModel ret = null;
            
            ret = ItemPredefinedList.FirstOrDefault(o => o.Name.ToLower().Equals(name.ToLower().Trim()));
            
            if (ret == null)
                ret = ItemCustomList.FirstOrDefault(o => o.Name.ToLower().Equals(name.ToLower().Trim()));

            return ret;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Check duplicity of custom list on application load and remove it.
        /// </summary>
        private void CheckDuplicityCustom()
        {

            for (int i = 0; i < ItemPredefinedList.Count; i++)
            {
                ItemCustomList.RemoveAll(
                    o => o.Name.ToLower().Trim().Equals(ItemPredefinedList[i].Name.ToLower())
                    );
            }
        }

        #endregion
    }
}
