﻿using System.Linq;
using UnityEngine;

namespace Assets.Player
{
    public class Animator : MonoBehaviour
    {
        public Sprite[] IdleSprites;
        public Sprite[] RunSpritesBackhand;
        public Sprite[] HitSpritesBackhand;
        public Sprite[] RunSpritesForehand;
        public Sprite[] HitSpritesForehand;
        
        public float AnimationSpeed = 1/10.0f;
        private float _current_time;
        private int _frame_index;
        private SpriteRenderer _sprite_renderer;
        private Sprite[] _current_base_animation;
        private Sprite[] _current_temp_animation;
        private float _temp_anim_speed;
        private float _base_anim_speed;

        public void Start()
        {
            _current_time = 0;
            _frame_index = 0;
            _sprite_renderer = GetComponent<SpriteRenderer>();
            _current_base_animation = IdleSprites;
            _current_temp_animation = null;
            _temp_anim_speed = AnimationSpeed;
            _base_anim_speed = AnimationSpeed;
        }

        public void Update()
        {
            PlayAnimation(_current_temp_animation != null, _current_temp_animation ?? _current_base_animation, _current_temp_animation != null ? _temp_anim_speed : _base_anim_speed);
        }

        private void PlayAnimation(bool temp_anim, Sprite[] animation, float speed)
        {
            _current_time += Time.deltaTime;

            if (_current_time > speed)
            {
                _current_time = 0;
                ++_frame_index;

                if (_frame_index >= animation.Count())
                {
                    _frame_index = 0;

                    if (temp_anim)
                    {
                        _current_temp_animation = null;
                        animation = _current_base_animation;
                    }
                }

                _sprite_renderer.sprite = animation[_frame_index];
            }
        }

        public void SetBaseAnimation(Sprite[] sprites, float speed)
        {
            if (!sprites.Any())
                return;

            if (sprites == RunSpritesBackhand || sprites == RunSpritesForehand)
                _base_anim_speed = 1 / 5.0f / Mathf.Abs(speed);
            else
                _base_anim_speed = 1 / 5.0f;


            if (_current_base_animation == sprites)
                return;
            _current_base_animation = sprites;
            _frame_index = 0;
            _current_time = 0;
        }

        public void SetTempAnimation(Sprite[] sprites, float speed)
        {
            if (!sprites.Any())
                return;

            _temp_anim_speed = 1 / 5.0f / Mathf.Abs(speed);

            if (_current_temp_animation != null)
                return;

            _current_temp_animation = sprites;
            _frame_index = 0;
            _current_time = 0;
            _sprite_renderer.sprite = _current_temp_animation[_frame_index];
        }
    }
}
