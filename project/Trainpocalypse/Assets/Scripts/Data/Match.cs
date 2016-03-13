namespace Funk.Data
{
    public class Match
    {
        public MapSettings MapSettings { get; private set; }
        public PlayerSettings[] PlayerSettings { get; private set; }
        public MatchSettings MatchSettings { get; private set; }
        public MatchState MatchState { get; private set; }

        public Match(
            MapSettings mapData,
            PlayerSettings[] playerSettings,
            MatchSettings matchSettings)
        {
            MapSettings = mapData;
            PlayerSettings = playerSettings;
            MatchSettings = matchSettings;
        }

        public void Start(MatchState startState)
        {
            MatchState = startState;
        }
    }

    public struct PlayerSettings
    {
        public short Index;
        public string Name;
        public string Train;
        public string Block;
        public InputMap InputMap;
    }

    public struct MapSettings
    {
        public string Name;
        public int Width;
        public int Height;
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