using System;
using System.Linq;
using System.Net.Sockets;
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
        private Animator _animator;
        private bool _forehand = true;
        private BeatMatcher _beat_matcher;
        private int _lane_index;
        private float _movement_y_last_frame;
        private float _distance;
        private float _temp_boost_until;
        private bool _boosting;
        private Transform _shadow;


        // Public interface.

        public float GetHorizontalVelocity()
        {
            return _horizontal_velocity;
        }

        public void ResetHorizontalVelocity()
        {
            _horizontal_velocity = 0.5f;
        }

        public void BoostHorizontalVelocity()
        {
            _horizontal_velocity += 0.015f;
        }

        public void DoubleHorizontalVelocity()
        {
            _horizontal_velocity *= 2;
        }

        public void FlipHand()
        {
            _forehand = !_forehand;
        }

        public bool IsForehand()
        {
            return _forehand;
        }

        public void Start ()
        {
            _collider = GetComponent<CircleCollider2D>();
            _input = GetComponent<PlayerInput>();
            Facing = 1;
            _on_ground = false;
            ResetHorizontalVelocity();
            _animator = GetComponent<Animator>();
            _beat_matcher = GameObject.Find("Music").GetComponent<BeatMatcher>();
            _lane_index = 0;
            _movement_y_last_frame = 0;
            _distance = 0;
            _temp_boost_until = 0;
            _boosting = false;
            _shadow = transform.GetChild(0).transform;
        }

        public int DistanceTravelled()
        {
            return (int)_distance;
        }

        /*public void Update()
        {
            var movement_input = _input.GetMovementInput();
            Facing = Math.Abs(movement_input.x) < float.Epsilon
                ? Facing
                : movement_input.x > 0
                    ? 1
                    : -1;

            transform.localScale = new Vector3(Facing, 1, 1);
        }*/

        public void Update()
        {
           // _on_ground = IsOnGround(transform.position, _collider, GroundLayerMask);
           // var input = _input.GetMovementInput();

           // _horizontal_velocity = CalculateHorizontalVelocity(_horizontal_velocity, input.x, MaxHorizontalVelocity, HorizontalAcceleration);
            //rigidbody2D.velocity = new Vector2(_horizontal_velocity, CalculateVerticalVelocity(rigidbody2D.velocity.y, transform.position, _collider, GroundLayerMask));

            if (_boosting && Time.time > _temp_boost_until)
            {
                _boosting = false;
                _horizontal_velocity /= 4;
            }

            var target_lane_dist = (_beat_matcher.Lanes[_lane_index]*0.16f + 0.04f) - transform.position.y;

            transform.position += new Vector3(_horizontal_velocity, target_lane_dist * 5, 0) * Time.deltaTime;
            _distance += _horizontal_velocity * Time.deltaTime;

            var abs_lane_dist = Mathf.Abs(target_lane_dist);
            if (abs_lane_dist > 0.01f)
            {
                _animator.SetBaseAnimation(_animator.FlySprite, 1);
                _shadow.localScale = new Vector3(1 - abs_lane_dist, 1 - abs_lane_dist, 1 - abs_lane_dist);
            }
            else
            {
                _shadow.localScale = new Vector3(1,1,1);
                _animator.SetBaseAnimation(_forehand ? _animator.RunSpritesForehand : _animator.RunSpritesBackhand,
                    _horizontal_velocity);
            }

            var movement = _input.GetMovementInput();

            if (movement.y > 0.7f && _movement_y_last_frame <= 0.7f)
            {
                _lane_index++;

                if (_lane_index >= _beat_matcher.Lanes.Count())
                    _lane_index = _beat_matcher.Lanes.Count() - 1;
            }
            else if (movement.y < -0.7f && _movement_y_last_frame >= -0.7f)
            {
                _lane_index--;

                if (_lane_index < 0)
                    _lane_index = 0;
            }

            _movement_y_last_frame = movement.y;

            /*_on_ground = ApplyJump(_input.GetJump(), _on_ground, rigidbody2D, JumpForce, rigidbody2D.velocity.y);*/
        }


        // Implementation.

        private static bool ApplyJump(bool jump_key_pressed, bool on_ground, Rigidbody2D rigidbody, float jump_force, float vertical_speed)
        {
            if (jump_key_pressed && on_ground && vertical_speed < 0.1f)
            {
                rigidbody.AddForce(new Vector2(0, jump_force));
                return true;
            }

            return on_ground;
        }

        /*private static float CalculateHorizontalVelocity(float current_horizontal_velocity, float horizontal_movement_input, float max_horizontal_velocity, float horizontal_acceleration)
        {
            if (Mathf.Abs(horizontal_movement_input) < 0.3f)
                return 0;

            if ((horizontal_movement_input > 0 && current_horizontal_velocity < 0) ||
                (horizontal_movement_input < 0 && current_horizontal_velocity > 0))
                return horizontal_movement_input * horizontal_acceleration * Time.deltaTime;

            return current_horizontal_velocity + horizontal_movement_input * horizontal_acceleration * Time.deltaTime;
        }*/

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

        public void Boost()
        {
            _distance += 5.0f;
            _horizontal_velocity += 0.04f;
        }
    }
}
