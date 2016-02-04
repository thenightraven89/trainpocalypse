using Funk.Data;

namespace Funk.Player
{
    public interface IStateModifier
    {
        void Apply(ApplyEffectContext context);
        void Unapply();
    }
}
