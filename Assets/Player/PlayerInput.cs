using UnityEngine;

namespace Assets.Player
{
    public partial class PlayerInput : MonoBehaviour
    {
        // State.

        private const string MovementAxisX = "MoveX";
        private const string MovementAxisY = "MoveY";
        private const string AttackAxisX = "AttackX";
        private const string AttackAxisY = "AttackY";
        private Vector2 _movement_input;
        private Vector2 _attack_input;


        // Public interface.

        public void Start()
        {
            _movement_input = new Vector3(0, 0, 0);
        }

        public void Update()
        {
            _movement_input = CalculateVelocity(_movement_input, ReadInput(MovementAxisX, MovementAxisY));
            _attack_input = CalculateVelocity(_movement_input, ReadInput(AttackAxisX, AttackAxisY));
        }

        public Vector2 GetMovementInput()
        {
            return _movement_input;
        }

        public Vector2 GetAttackInput()
        {
            return _attack_input;
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
