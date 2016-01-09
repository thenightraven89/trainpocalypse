namespace Funk.Data
{
    public class Match
    {
        public MapData MapData { get; private set; }
        public PlayerData[] PlayerData { get; private set; }
        public MatchSettings MatchSettings { get; private set; }

        public Match(MapData mapData, PlayerData[] playerData, MatchSettings matchSettings)
        {
            MapData = mapData;
            PlayerData = playerData;
            MatchSettings = matchSettings;
        }
    }

    public struct PlayerData
    {
        public byte Index;
        public string Name;
        public string Train;
        public int Life;
        public InputMap InputMap;
    }

    public struct MapData
    {
        public string Name;
        public int Rezolution;
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