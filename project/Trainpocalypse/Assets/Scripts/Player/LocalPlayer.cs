using Funk.Data;

namespace Funk.Player
{
    public class LocalPlayer : IPlayer
    {
        private string _name;
        private Action _noAction;

        public LocalPlayer(PlayerData data)
        {
            _name = data.Name;
            _noAction = new Action { PlayerName = _name, Type = Action.ActionType.None };
        }

        public Action GetAction()
        {
            return _noAction;
        }
    }
}