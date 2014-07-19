using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Player
{
    interface IWeaponBehaviour
    {
        IWeaponBehaviour Update(PlayerWeapon weapon, Animator animator);
    }

    class IdleWeaponBehaivour : IWeaponBehaviour
    {
        public IWeaponBehaviour Update(PlayerWeapon weapon, Animator animator)
        {
            if (Input.GetKeyDown("joystick button 0"))
                return new WeaponBehaviour("joystick button 0", weapon.BlueStrikePrototype);
            if (Input.GetKeyDown("joystick button 1"))
                return new WeaponBehaviour("joystick button 1", weapon.YellowStrikePrototype);
            if (Input.GetKeyDown("joystick button 2"))
                return new WeaponBehaviour("joystick button 2", weapon.MagentaStrikePrototype);
            if (Input.GetKeyDown("joystick button 3"))
                return new WeaponBehaviour("joystick button 3", weapon.OrangeStrikePrototype);

            return this;
        }
    }

    class WeaponBehaviour : IWeaponBehaviour
    {
        private float _power;
        private readonly GameObject _strike_prototype;
        private readonly string _key_code;

        public WeaponBehaviour(string key_code, GameObject strike_prototype)
        {
            _key_code = key_code;
            _strike_prototype = strike_prototype;
        }

        public IWeaponBehaviour Update(PlayerWeapon weapon, Animator animator)
        {
            _power += Time.deltaTime;
            var collider = weapon.GetComponent<CircleCollider2D>();
            var controller = weapon.GetComponent<PlayerController>();

            if (!Input.GetKey(_key_code))
            {
                var strike = (GameObject)Object.Instantiate(_strike_prototype);
                strike.transform.position = weapon.transform.position + new Vector3((strike.renderer.bounds.extents.x * 2  + collider.radius / 2) * controller.Facing, 0);
                strike.transform.localScale = new Vector3(controller.Facing * (_power + 1), 1);

                var strike_comp = strike.GetComponent<WeaponStrike>();
                strike_comp.Power = (_power + 1);

                var strike_collider = strike.GetComponent<CircleCollider2D>();
                strike_collider.radius *= (_power + 1);

                var x_vel = controller.rigidbody2D.velocity.x;
                animator.SetTempAnimation(controller.IsForehand() ? animator.HitSpritesForehand : animator.HitSpritesBackhand, Mathf.Abs(x_vel) > 0.1 ? x_vel : 1);
                controller.FlipHand();
                
                return new IdleWeaponBehaivour();
            }

            return this;
        }
    }

    public class PlayerWeapon : MonoBehaviour
    {
        // State.

        public GameObject MagentaStrikePrototype;
        public GameObject YellowStrikePrototype;
        public GameObject OrangeStrikePrototype;
        public GameObject BlueStrikePrototype;
        private PlayerInput _input;
        private IWeaponBehaviour _current_weapon_behaviour;
        private Animator _animator;

        // Public interface.

        public void Start ()
        {
            _input = GetComponent<PlayerInput>();
            _current_weapon_behaviour = new IdleWeaponBehaivour();
            _animator = GetComponent<Animator>();
        }

        public void Update()
        {
            _current_weapon_behaviour = _current_weapon_behaviour.Update(this, _animator);
        }
    }
}
