using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funk.Data
{
    public class ApplyEffectContext
    {
        public Train Target { get; private set; }
        public MatchState Context { get; private set; }

        public ApplyEffectContext(Train target, MatchState context)
        {
            Target = target;
            Context = context;
        }
    }
}
