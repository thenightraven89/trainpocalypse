using Funk.Data;
using Funk.Powerup;

namespace Funk.Collision
{
    class PowerupCollisionHandler : ICollisionHandler
    {
        public string Tag
        {
            get
            {
                return "Powerup";
            }
        }

        private PowerupHandler _powerupHandler;

        public PowerupCollisionHandler(PowerupHandler powerupHandler)
        {
            _powerupHandler = powerupHandler;
        }

        public void HandleCollision(ICollisionTirgger t, CollisionEventArgs collisionArgs)
        {
            Train trainSender = (Train)t;
            if (t==null)
                return;

            PowerupBase pw = collisionArgs.Other.GetComponent<PowerupBase>();

            pw.Apply(new ApplyEffectContext(trainSender, collisionArgs.MatchState));

            _powerupHandler.PickUp(pw.GetType());
        }
    }
}
