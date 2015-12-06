namespace Funk.Data
{
    public class Match
    {
        private PlayerData[] _playerData;

        public Match(PlayerData[] playerData)
        {
            _playerData = playerData;
        }
    }

    public struct Action
    {
        public enum ActionType
        {
            None,
            MoveUp,
            MoveDown,
            MoveLeft,
            MoveRight
        }

        public string PlayerName;
        public ActionType Type;
    }

    public struct PlayerData
    {
        public string Name;
        public string Model;
        public int Life;
    }
}