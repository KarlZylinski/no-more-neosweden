using System.Linq;
using UnityEngine;

namespace Assets.Player
{
    public class PlayerController : MonoBehaviour
    {
        private const float JumpForce = 100.0f;

        // State.

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
            rigidbody2D.velocity = CalculateVelocity(rigidbody2D.velocity, _input.GetMovementInput());
            ApplyJump( _input.GetJump(), _on_ground, rigidbody2D);
        }


        // Implementation.

        private static void ApplyJump(bool jump_key_pressed, bool on_ground, Rigidbody2D rigidbody)
        {
            if (jump_key_pressed && on_ground)
                rigidbody.AddForce(new Vector2(0, JumpForce));
        }

        private static Vector2 CalculateVelocity(Vector2 current_velocity, Vector2 movement_input)
        {
            return new Vector2(movement_input.x, current_velocity.y);
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
