using System;
using UnityEngine;

namespace Assets.Player
{
    public partial class PlayerInput : MonoBehaviour
    {
        // State.

        private const string MovementAxisX = "MoveX";
        private const string MovementAxisY = "MoveY";
        private const string JumpAxis = "Jump";
        private Vector2 _movement_input;
        private bool _jump;


        // Public interface.

        public void Start()
        {
            _movement_input = new Vector2(0, 0);
        }

        public void Update()
        {
            _movement_input = CalculateVelocity(_movement_input, ReadInput(MovementAxisX, MovementAxisY));
        }

        public Vector2 GetMovementInput()
        {
            return _movement_input;
        }

        // Implementation.

        private static Vector2 ReadInput(string x_axis, string y_axis)
        {
            return new Vector2(Input.GetAxis(x_axis), -Input.GetAxis(y_axis));
        }

        private static Vector2 CalculateVelocity(Vector2 current_velocity, Vector2 input)
        {
            return input;
        }
    }
}
