using System.Linq;
using UnityEngine;

namespace Assets.Player
{
    public class PlayerController : MonoBehaviour
    {
        // State.

        public LayerMask GroundLayerMask;
        private CircleCollider2D _collider;
        private bool _on_ground;
        private PlayerInput _input;

        // Public interface.

        public void Start ()
        {
            _collider = GetComponent<CircleCollider2D>();
            _on_ground = false;
            _input = GetComponent<PlayerInput>();
            
        }

        public void FixedUpdate()
        {
            _on_ground = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y) + _collider.center,
                    _collider.radius, GroundLayerMask);
            rigidbody2D.velocity = CalculateVelocity(rigidbody2D.velocity, _input.GetMovementInput(), _input.GetJump(), _on_ground);
        }


        // Implementation.

        private static Vector2 CalculateVelocity(Vector2 current_velocity, Vector2 movement_input, bool jump, bool on_ground)
        {
            return new Vector2(movement_input.x,
                CalculateVerticalSpeed(current_velocity.y, jump, on_ground));
        }

        // ADD FORCE INSTEAD ON JUMP. FIXES STUCK PROBLEM. THEN DO ATTACKING.
        private static float CalculateVerticalSpeed(float current_vertical_speed, bool jump, bool on_ground)
        {
            var jump_speed = jump && on_ground ? 4.0f : 0.0f;
            var vertical_speed = on_ground ? 0 : current_vertical_speed;
            return vertical_speed + jump_speed;
        }
    }
}
