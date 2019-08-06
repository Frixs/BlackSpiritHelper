using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;

namespace BlackSpiritHelper.Core
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
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
            // Schedule Item list.
            string itemKzarka = AddItem("Kzarka", "fc2121", true).Name;
            string itemKaranda = AddItem("Karanda", "004fcf", true).Name;
            string itemOffin = AddItem("Offin", "0087e0", true).Name;
            string itemNouver = AddItem("Nouver", "e68507", true).Name;
            string itemKutum = AddItem("Kutum", "b108ff", true).Name;
            string itemVell = AddItem("Vell", "009677", true).Name;
            string itemGarmoth = AddItem("Garmoth", "d60229", true).Name;
            string itemQuint = AddItem("Quint", "752300", true).Name;
            string itemMuraka = AddItem("Muraka", "752300", true).Name;

            // Schedule templates.
            #region Template: BDO-EU
            AddTemplate(new ScheduleTemplateDataViewModel
            {
                LastUpdate = new DateTime(2019, 8, 5).Ticks,
                Title = "BDO-EU",
                TimeZone = RegionTimeZone.EU,
                Schedule = new ObservableCollection<ScheduleTemplateDayDataViewModel>
                    {
                        #region Monday
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Monday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(0, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                        itemKaranda,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(2, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKaranda,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(5, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(9, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(12, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemOffin,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(16, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(19, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(22, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                    },
                                },
                            }
                        },
	                    #endregion

                        #region Tuesday
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Tuesday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(0, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemKaranda,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(2, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(5, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(9, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(12, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(16, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(19, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKaranda,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(22, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemGarmoth,
                                    },
                                },
                            }
                        },
	                    #endregion

                        #region Wednesday
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Wednesday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(0, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(2, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKaranda,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(5, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(9, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKaranda,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(16, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(19, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemOffin,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(22, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemKaranda,
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(23, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemQuint,
                                        itemMuraka,
                                    },
                                },
                            }
                        },
	                    #endregion

                        #region Thursday
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Thursday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(0, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(2, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(5, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(9, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(12, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(16, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(19, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(22, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemGarmoth,
                                    },
                                },
                            }
                        },
	                    #endregion

                        #region Friday
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Friday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(0, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                        itemKaranda,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(2, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(5, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKaranda,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(9, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(12, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKaranda,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(16, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(19, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(22, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                        itemKzarka,
                                    },
                                },
                            }
                        },
	                    #endregion

                        #region Saturday
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Saturday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(0, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(2, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemOffin,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(5, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(9, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(12, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(16, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemQuint,
                                        itemMuraka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(19, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKaranda,
                                        itemKzarka,
                                    },
                                },
                            }
                        },
	                    #endregion

                        #region Sunday
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Sunday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(0, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(2, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(5, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(9, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(12, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(16, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemVell,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(19, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemGarmoth,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(22, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                        itemNouver,
                                    },
                                },
                            }
                        },
	                    #endregion
                    },
            }, true);
            #endregion

            #region Template: BDO-NA
            AddTemplate(new ScheduleTemplateDataViewModel
            {
                LastUpdate = new DateTime(2019, 8, 5).Ticks,
                Title = "BDO-NA",
                TimeZone = RegionTimeZone.NA,
                Schedule = new ObservableCollection<ScheduleTemplateDayDataViewModel>
                    {
                        #region Monday
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Monday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(0, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKaranda,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(3, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(7, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(10, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemOffin,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(14, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(17, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(20, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(22, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemKaranda,
                                    },
                                },
                            }
                        },
	                    #endregion

                        #region Tuesday
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Tuesday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(0, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(3, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(7, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(10, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(14, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(17, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKaranda,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(20, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemGarmoth,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(22, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                        itemKzarka,
                                    },
                                },
                            }
                        },
	                    #endregion

                        #region Wednesday
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Wednesday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(0, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKaranda,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(7, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKaranda,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(10, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(14, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(17, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemOffin,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(20, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemKaranda,
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(21, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemQuint,
                                        itemMuraka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(22, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                            }
                        },
	                    #endregion

                        #region Thursday
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Thursday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(0, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(3, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(7, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(10, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(14, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(17, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(20, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemGarmoth,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(22, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                        itemKaranda,
                                    },
                                },
                            }
                        },
	                    #endregion

                        #region Friday
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Friday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(0, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(3, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKaranda,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(7, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(10, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKaranda,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(14, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(17, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(20, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(22, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemKaranda,
                                    },
                                },
                            }
                        },
	                    #endregion

                        #region Saturday
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Saturday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(0, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemOffin,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(3, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(7, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(10, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(14, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemQuint,
                                        itemMuraka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(17, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                        itemKaranda,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(22, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                        itemKutum,
                                    },
                                },
                            }
                        },
	                    #endregion

                        #region Sunday
                        new ScheduleTemplateDayDataViewModel
                        {
                            DayOfWeek = DayOfWeek.Sunday,
                            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
                            {
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(0, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(3, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(7, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(10, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(14, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemVell,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(17, 0, 0), ItemList = new ObservableCollection<string> {
                                        itemGarmoth,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(20, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemKzarka,
                                        itemNouver,
                                    },
                                },
                                new ScheduleTemplateDayTimeDataViewModel
                                {
                                    Time = new TimeSpan(22, 15, 0), ItemList = new ObservableCollection<string> {
                                        itemKutum,
                                        itemKaranda,
                                    },
                                },
                            }
                        },
	                    #endregion
                    },
            }, true);
            #endregion

            #region Template: BDO-RU
            //AddTemplate(new ScheduleTemplateDataViewModel
            //{
            //    LastUpdate = new DateTime(2019, 8, 5).Ticks,
            //    Title = "BDO-RU",
            //    TimeZone = RegionTimeZone.RU,
            //    Schedule = new ObservableCollection<ScheduleTemplateDayDataViewModel>
            //    {
            //        #region Monday
            //        new ScheduleTemplateDayDataViewModel
            //        {
            //            DayOfWeek = DayOfWeek.Monday,
            //            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
            //            {
            //            }
            //        },
            //        #endregion

            //        #region Tuesday
            //        new ScheduleTemplateDayDataViewModel
            //        {
            //            DayOfWeek = DayOfWeek.Tuesday,
            //            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
            //            {
            //            }
            //        },
            //        #endregion

            //        #region Wednesday
            //        new ScheduleTemplateDayDataViewModel
            //        {
            //            DayOfWeek = DayOfWeek.Wednesday,
            //            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
            //            {
            //            }
            //        },
            //        #endregion

            //        #region Thursday
            //        new ScheduleTemplateDayDataViewModel
            //        {
            //            DayOfWeek = DayOfWeek.Thursday,
            //            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
            //            {
            //            }
            //        },
            //        #endregion

            //        #region Friday
            //        new ScheduleTemplateDayDataViewModel
            //        {
            //            DayOfWeek = DayOfWeek.Friday,
            //            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
            //            {
            //            }
            //        },
            //        #endregion

            //        #region Saturday
            //        new ScheduleTemplateDayDataViewModel
            //        {
            //            DayOfWeek = DayOfWeek.Saturday,
            //            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
            //            {
            //            }
            //        },
            //        #endregion

            //        #region Sunday
            //        new ScheduleTemplateDayDataViewModel
            //        {
            //            DayOfWeek = DayOfWeek.Sunday,
            //            TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>
            //            {
            //            }
            //        },
            //        #endregion
            //    },
            //}, true);
            #endregion
        }

        #endregion
    }
}
