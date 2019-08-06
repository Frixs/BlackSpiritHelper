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
        /// Selected template.
        /// </summary>
        private ScheduleTemplateDataViewModel mSelectedTemplate;

        /// <summary>
        /// Template list, custom.
        /// </summary>
        private ObservableCollection<ScheduleTemplateDataViewModel> mTemplateCustomList = new ObservableCollection<ScheduleTemplateDataViewModel>();

        /// <summary>
        /// Item list, custom.
        /// </summary>
        private ObservableCollection<ScheduleItemDataViewModel> mItemCustomList = new ObservableCollection<ScheduleItemDataViewModel>();

        #endregion

        #region Public Properties

        /// <summary>
        /// Selected template Title.
        /// </summary>
        public string SelectedTemplateTitle { get; set; } = string.Empty;

        /// <summary>
        /// Selected template.
        /// </summary>
        [XmlIgnore]
        public ScheduleTemplateDataViewModel SelectedTemplate
        {
            get => mSelectedTemplate;
            set
            {
                mSelectedTemplate = value;
                SelectedTemplateTitle = mSelectedTemplate.Title;
            }
        }

        /// <summary>
        /// Template list, predefined.
        /// </summary>
        [XmlIgnore]
        public List<ScheduleTemplateDataViewModel> TemplatePredefinedList { get; protected set; } = new List<ScheduleTemplateDataViewModel>();

        /// <summary>
        /// Template list, custom.
        /// </summary>
        public ObservableCollection<ScheduleTemplateDataViewModel> TemplateCustomList
        {
            get => mTemplateCustomList;
            set
            {
                mTemplateCustomList = value;
                CheckTemplateDuplicityCustom();
            }
        }

        /// <summary>
        /// Template list, presenter.
        /// </summary>
        [XmlIgnore]
        public List<ScheduleTemplateDataViewModel> TemplateListPresenter
        {
            get
            {
                var l = new List<ScheduleTemplateDataViewModel>();
                l.AddRange(TemplatePredefinedList);
                l.AddRange(TemplateCustomList);
                return l;
            }
        }

        /// <summary>
        /// Item list, predefined.
        /// </summary>
        [XmlIgnore]
        public List<ScheduleItemDataViewModel> ItemPredefinedList { get; protected set; } = new List<ScheduleItemDataViewModel>();

        /// <summary>
        /// Item list, custom.
        /// </summary>
        public ObservableCollection<ScheduleItemDataViewModel> ItemCustomList
        {
            get => mItemCustomList;
            set
            {
                mItemCustomList = value;
                CheckItemDuplicityCustom();
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
        public override bool IsRunning => false;

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
            // Initialize items.
            for (int i = 0; i < ItemPredefinedList.Count; i++)
                ItemPredefinedList[i].Init(true);
            for (int i = 0; i < ItemCustomList.Count; i++)
                ItemCustomList[i].Init();

            // Initialize templates.
            for (int i = 0; i < TemplatePredefinedList.Count; i++)
                TemplatePredefinedList[i].Init(true);
            for (int i = 0; i < TemplateCustomList.Count; i++)
                TemplateCustomList[i].Init();

            // Set selected item.
            //SelectedTemplate = GetTemplateByName(SelectedTemplateTitle);
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
            if (IsItemAlreadyDefined(name))
                return null;

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
        /// Check if the item is already defined.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsItemAlreadyDefined(string name)
        {
            if (ItemPredefinedList.FirstOrDefault(o => o.Name.ToLower().Equals(name.ToLower().Trim())) != null)
                return true;

            if (ItemCustomList != null && ItemCustomList.FirstOrDefault(o => o.Name.ToLower().Equals(name.ToLower().Trim())) != null)
                return true;

            return false;
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

        /// <summary>
        /// Add a new template.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="isPredefined"></param>
        /// <returns></returns>
        public ScheduleTemplateDataViewModel AddTemplate(ScheduleTemplateDataViewModel template, bool isPredefined = false)
        {
            if (IsTemplateAlreadyDefined(template.Title))
                return null;

            if (isPredefined)
                TemplatePredefinedList.Add(template);
            else
                TemplateCustomList.Add(template);

            return template;
        }

        /// <summary>
        /// Check if the template is already defined.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool IsTemplateAlreadyDefined(string title)
        {
            if (TemplatePredefinedList.FirstOrDefault(o => o.Title.ToLower().Equals(title.ToLower().Trim())) != null)
                return true;

            if (TemplateCustomList != null && TemplateCustomList.FirstOrDefault(o => o.Title.ToLower().Equals(title.ToLower().Trim())) != null)
                return true;

            return false;
        }

        /// <summary>
        /// Get template by title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public ScheduleTemplateDataViewModel GetTemplateByName(string title)
        {
            ScheduleTemplateDataViewModel ret = null;

            ret = TemplatePredefinedList.FirstOrDefault(o => o.Title.ToLower().Equals(title.ToLower().Trim()));

            if (ret == null)
                ret = TemplateCustomList.FirstOrDefault(o => o.Title.ToLower().Equals(title.ToLower().Trim()));

            return ret;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Check duplicity of item custom list on application load and remove it.
        /// </summary>
        private void CheckItemDuplicityCustom()
        {

            for (int i = 0; i < ItemPredefinedList.Count; i++)
            {
                ItemCustomList.RemoveAll(
                    o => o.Name.ToLower().Trim().Equals(ItemPredefinedList[i].Name.ToLower())
                    );
            }
        }

        /// <summary>
        /// Check duplicity of template custom list on application load and remove it.
        /// </summary>
        private void CheckTemplateDuplicityCustom()
        {

            for (int i = 0; i < TemplatePredefinedList.Count; i++)
            {
                TemplateCustomList.RemoveAll(
                    o => o.Title.ToLower().Trim().Equals(TemplatePredefinedList[i].Title.ToLower())
                    );
            }
        }

        #endregion
    }
}
