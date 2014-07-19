using System.Linq;
using UnityEngine;

namespace Assets.Player
{
    public class PlayerController : MonoBehaviour
    {
        // State.
        public float JumpForce = 100.0f;
        public float MaxHorizontalVelocity = 500.0f;
        public float HorizontalAcceleration = 5.0f;

        public LayerMask GroundLayerMask;
        private CircleCollider2D _collider;
        private PlayerInput _input;
        private bool _on_ground;


        // Public interface.

        public void Start ()
        {
            _collider = GetComponent<CircleCollider2D>();
            _input = GetComponent<PlayerInput>();
            _on_ground = false;
        }

        public void FixedUpdate()
        {
            _on_ground = IsOnGround(transform.position, _collider, GroundLayerMask);
            rigidbody2D.velocity = CalculateVelocity(rigidbody2D.velocity, _input.GetMovementInput(), MaxHorizontalVelocity, HorizontalAcceleration, _on_ground);
            ApplyJump( _input.GetJump(), _on_ground, rigidbody2D, JumpForce);
        }


        // Implementation.

        private static void ApplyJump(bool jump_key_pressed, bool on_ground, Rigidbody2D rigidbody, float jump_force)
        {
            if (jump_key_pressed && on_ground)
                rigidbody.AddForce(new Vector2(0, jump_force));
        }

        private static Vector2 CalculateVelocity(Vector2 current_velocity, Vector2 movement_input, float max_horizontal_velocity, float horizontal_acceleration, bool on_ground)
        {
            return new Vector2(
               CalculateHorizontalVelocity(current_velocity.x, movement_input.x, max_horizontal_velocity, horizontal_acceleration),
               current_velocity.y);
        }

        private static float CalculateHorizontalVelocity(float current_horizontal_velocity, float horizontal_movement_input, float max_horizontal_velocity, float horizontal_acceleration)
        {
            if (Mathf.Abs(horizontal_movement_input) < float.Epsilon)
                return 0;

            if ((horizontal_movement_input > 0 && current_horizontal_velocity < 0) ||
                (horizontal_movement_input < 0 && current_horizontal_velocity > 0))
                return horizontal_movement_input * horizontal_acceleration * Time.deltaTime;

            return current_horizontal_velocity + horizontal_movement_input * horizontal_acceleration * Time.deltaTime;
        }

        private static bool IsOnGround(Vector2 position, CircleCollider2D collider, LayerMask ground_layer_mask)
        {
            var collider_center = new Vector2(position.x, position.y) + collider.center;
            var touched_ground = Physics2D.OverlapCircle(collider_center, collider.radius, ground_layer_mask);
            var touched_below = touched_ground != null && (touched_ground.transform.position.y - touched_ground.bounds.extents.y / 2) >=
                                (collider_center.y + collider.radius);

            if (touched_below)
                return true;

            return Physics2D.Raycast(position, new Vector2(0, -1), collider.radius * 2, ground_layer_mask).collider != null;
        }
    }
}
