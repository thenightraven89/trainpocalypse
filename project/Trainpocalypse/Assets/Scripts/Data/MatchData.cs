using System.Collections.Generic;

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

        public override string ToString()
        {
            return string.Format("{0}: {1}", PlayerName, Type.ToString());
        }

        public override bool Equals(object obj)
        {
            var otherAction = (Action)obj;
            return (otherAction.PlayerName == PlayerName && otherAction.Type == Type);
        }
    }

    public struct PlayerData
    {
        public string Name;
        public string Model;
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