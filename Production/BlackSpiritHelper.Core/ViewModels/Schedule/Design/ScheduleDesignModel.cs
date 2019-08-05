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
            ItemPredefinedList = new List<ScheduleItemDataViewModel>();
            ScheduleItemDataViewModel itemKzarka = AddItem("Kzarka", "9c0017", true);
            ScheduleItemDataViewModel itemKaranda = AddItem("Karanda", "000000", true);
            ScheduleItemDataViewModel itemOffin = AddItem("Offin", "40c3cf", true);
            ScheduleItemDataViewModel itemNouver = AddItem("Nouver", "000000", true);
            ScheduleItemDataViewModel itemKutum = AddItem("Kutum", "000000", true);
            ScheduleItemDataViewModel itemVell = AddItem("Vell", "000000", true);
            ScheduleItemDataViewModel itemGarmoth = AddItem("Garmoth", "000000", true);
            ScheduleItemDataViewModel itemQuint = AddItem("Quint", "000000", true);
            ScheduleItemDataViewModel itemMuraka = AddItem("Muraka", "000000", true);

            // Schedule Template list.
            TemplatePredefinedList = new List<ScheduleTemplateDataViewModel>
            {
                // Template EU.
                new ScheduleTemplateDataViewModel
                {
                    LastUpdate = new DateTime(2019, 8, 3).Ticks,
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
                                    Time = new TimeSpan(0, 15, 0),
                                    ItemList = new ObservableCollection<string>
                                    {
                                        itemKutum.Name,
                                        itemKaranda.Name,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(2, 0, 0),
                                    ItemList = new ObservableCollection<string>
                                    {
                                        itemKaranda.Name,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(5, 0, 0),
                                    ItemList = new ObservableCollection<string>
                                    {
                                        itemKzarka.Name,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(9, 0, 0),
                                    ItemList = new ObservableCollection<string>
                                    {
                                        itemKzarka.Name,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(12, 0, 0),
                                    ItemList = new ObservableCollection<string>
                                    {
                                        itemOffin.Name,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(16, 0, 0),
                                    ItemList = new ObservableCollection<string>
                                    {
                                        itemKutum.Name,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(18, 0, 0),
                                    ItemList = new ObservableCollection<string>
                                    {
                                        itemNouver.Name,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(22, 15, 0),
                                    ItemList = new ObservableCollection<string>
                                    {
                                        itemKzarka.Name,
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
                                        itemKzarka.Name,
                                        itemOffin.Name,
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
