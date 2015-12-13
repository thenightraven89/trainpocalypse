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

    public struct PlayerData
    {
        public string Name;
        public string Train;
        public int Life;
        public InputMap SelectedInputMap;
    }

    public enum InputMap : byte
    {
        Controller0 = 0,
        Controller1 = 1,
        Controller2 = 2,
        Controller3 = 3,
        KeyboardWASD = 4,
        KeyboardArrows = 5
    }
}