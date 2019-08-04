using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class ScheduleDesignModel : ScheduleViewModel
    {
        #region New Instance Getter

        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
        [XmlIgnore]
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
            // Schedule Item list.
            PredefinedItemList = new List<ScheduleItemDataViewModel>();
            ScheduleItemDataViewModel kzarka = AddItem("Kzarka", "9c0017", true);
            ScheduleItemDataViewModel offin = AddItem("Offin", "40c3cf", true);

            // Schedule Template list.
            PredefinedTemplateList = new List<ScheduleTemplateDataViewModel>
            {
                // Template EU.
                new ScheduleTemplateDataViewModel
                {
                    LastUpdate = new DateTime(2019, 8, 3).Ticks,
                    IsCustom = false,
                    TimeZone = RegionTimeZone.NA,
                    Schedule = new ObservableCollection<ScheduleTemplateDayDataViewModel>
                    {
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Sunday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                            }
                        },
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Monday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(1, 30, 0),
                                    ItemList = new ObservableCollection<string>
                                    {
                                        SchedulePredefinedItem.Kzarka.ToString(),
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(22, 15, 0),
                                    ItemList = new ObservableCollection<string>
                                    {
                                        SchedulePredefinedItem.Kzarka.ToString(),
                                        SchedulePredefinedItem.Offin.ToString(),
                                    },
                                },
                            }
                        },
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Tuesday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(8, 15, 0),
                                    ItemList = new ObservableCollection<string>
                                    {
                                        SchedulePredefinedItem.Kzarka.ToString(),
                                        SchedulePredefinedItem.Offin.ToString(),
                                    },
                                },
                            }
                        },
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Wednesday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                            }
                        },
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Thursday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                            }
                        },
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Friday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                            }
                        },
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Saturday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                            }
                        },
                    },
                },
            };

        }

        #endregion
    }
}
