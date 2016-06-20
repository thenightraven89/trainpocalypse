using Funk.Data;
using System.Collections.Generic;

namespace Funk
{
    public interface IActionProvider
    {
        ICollection<IPlaybackAction> GetActions();
        void ClearActions();
    }
}