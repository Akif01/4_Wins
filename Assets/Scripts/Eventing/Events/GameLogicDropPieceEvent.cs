namespace Assets.Scripts.Eventing.Events
{
    public class GameLogicDropPieceEvent : IEvent
    {
        public int Column { get; set; }

        public GameLogicDropPieceEvent(int column)
        {
            Column = column;
        }
    }
}
