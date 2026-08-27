namespace Bussigo.Core
{
    /// <summary>
    /// Discrete lifecycle phases for the BUSSIGO state machine.
    /// </summary>
    public enum GamePhase
    {
        MainMenu = 0,
        TerminalBoarding = 1,
        HighwayDriving = 2,
        DestinationArrival = 3,
        TripSummary = 4
    }
}
