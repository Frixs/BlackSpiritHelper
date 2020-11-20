namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Wraps the data related to specific APM session
    /// </summary>
    public class ApmCalculatorSessionDataViewModel : BaseViewModel
    {
        int TotalActions { get; set; }

        int HighestApm { get; set; }

        int AverageApm { get; set; }
    }
}
