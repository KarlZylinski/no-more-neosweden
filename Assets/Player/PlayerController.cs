using System;
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
        private float _horizontal_velocity;
        public float Facing { get; private set; }


        // Public interface.

        public void Start ()
        {
            _collider = GetComponent<CircleCollider2D>();
            _input = GetComponent<PlayerInput>();
            Facing = 1;
            _on_ground = false;
            _horizontal_velocity = 0;
        }

        public void Update()
        {
            var movement_input = _input.GetMovementInput();
            Facing = Math.Abs(movement_input.x) < float.Epsilon
                ? Facing
                : movement_input.x > 0
                    ? 1
                    : -1;
        }

        public void FixedUpdate()
        {
            _on_ground = IsOnGround(transform.position, _collider, GroundLayerMask);
            var input = _input.GetMovementInput();
            _horizontal_velocity = CalculateHorizontalVelocity(_horizontal_velocity, input.x, MaxHorizontalVelocity, HorizontalAcceleration);
            rigidbody2D.velocity = new Vector2(_horizontal_velocity, CalculateVerticalVelocity(rigidbody2D.velocity.y, transform.position, _collider, GroundLayerMask));
            _on_ground = ApplyJump( _input.GetJump(), _on_ground, rigidbody2D, JumpForce);
        }


        // Implementation.

        private static bool ApplyJump(bool jump_key_pressed, bool on_ground, Rigidbody2D rigidbody, float jump_force)
        {
            if (jump_key_pressed && on_ground)
            {
                rigidbody.AddForce(new Vector2(0, jump_force));
                return true;
            }

            return on_ground;
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

        private static float CalculateVerticalVelocity(float current_vertical_velocity, Vector2 position, CircleCollider2D collider, LayerMask ground_layer_mask)
        {
            return HitHead(position, collider, ground_layer_mask) ? 0 : current_vertical_velocity;
        }

        private static bool HitHead(Vector2 position, CircleCollider2D collider, LayerMask ground_layer_mask)
        {
            var collider_center = new Vector2(position.x, position.y) + collider.center;
            var touched_ground = Physics2D.OverlapCircle(collider_center, collider.radius, ground_layer_mask);
            return touched_ground != null &&
                   (touched_ground.transform.position.y - touched_ground.bounds.extents.y/2) >=
                   (collider_center.y + collider.radius);
        }

        private static bool IsOnGround(Vector2 position, CircleCollider2D collider, LayerMask ground_layer_mask)
        {
            var collider_center = new Vector2(position.x, position.y) + collider.center;
            var touched_ground = Physics2D.OverlapCircle(collider_center, collider.radius, ground_layer_mask);
            var touched_below = touched_ground != null && (touched_ground.transform.position.y - touched_ground.bounds.extents.y / 2) >=
                                (collider_center.y + collider.radius);

            if (touched_below)
                return true;

            return Physics2D.Raycast(position, new Vector2(0, -1), collider.radius + collider.radius, ground_layer_mask).collider != null;
        }
    }
}
