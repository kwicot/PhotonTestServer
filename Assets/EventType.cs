namespace DefaultNamespace
{
    public enum EventType
    {
        Change_MoveSpeed = 0,
        PlayersLoaded = 1
    }

    public enum ServerEventType
    {
        StartGame = 0,
        StartPhase_Prepare = 1,
        StartPhase_Move = 2,
        StopPhase_Prepare = 5,
        StopPhase_Move = 6,
        StartTimer = 252,
        TimerTick = 253,
        EndGame = 254
    }
}