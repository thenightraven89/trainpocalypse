namespace Funk.Data
{
    public struct PlayerAction
    {
        public string PlayerName;
        public ActionType Type;

        public override string ToString()
        {
            return string.Format("{0}: {1}", PlayerName, Type.ToString());
        }

        public override bool Equals(object obj)
        {
            var otherAction = (PlayerAction)obj;
            return (otherAction.PlayerName == PlayerName && otherAction.Type == Type);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    
    public enum ActionType
    {
        None,
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight
    }
}