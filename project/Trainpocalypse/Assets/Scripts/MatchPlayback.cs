using Funk.Data;
using System.Collections.Generic;

namespace Funk
{
    public class MatchPlayback
    {
        public MatchPlayback()
        {

        }

        public void Play(List<Action> actions)
        {
            for (int i = 0; i < actions.Count; i++)
            {
                // get train using actions[i].PlayerName from a dictionary
                // set train to play actionis[i].ActionType
            }
        }
    }
}