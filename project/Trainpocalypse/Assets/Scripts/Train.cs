using Funk.Data;
using UnityEngine;

namespace Funk
{
    public class Train : MonoBehaviour
    {
        private Vector3 _direction;

        public void Play(ActionType action)
        {
            switch (action)
            {
                case ActionType.MoveUp:
                    _direction = Vector3.forward;
                    break;

                case ActionType.MoveDown:
                    _direction = Vector3.back;
                    break;

                case ActionType.MoveLeft:
                    _direction = Vector3.left;
                    break;

                case ActionType.MoveRight:
                    _direction = Vector3.right;
                    break;
            }
        }
    }
}