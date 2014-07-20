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
        private float _cooldown;

        public IWeaponBehaviour Update(PlayerWeapon weapon, Animator animator)
        {
            _cooldown -= Time.deltaTime;

            if (_cooldown > 0)
                return this;

            if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown("s"))
                Lax(weapon, animator, weapon.BlueStrikePrototype);
            if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown("d"))
                Lax(weapon, animator, weapon.YellowStrikePrototype);
            if (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown("a"))
                Lax(weapon, animator, weapon.MagentaStrikePrototype);
            if (Input.GetKeyDown("joystick button 3") || Input.GetKeyDown("w"))
                Lax(weapon, animator, weapon.OrangeStrikePrototype);

            return this;
        }

        private void Lax(PlayerWeapon weapon, Animator animator, GameObject strike_prototype)
        {
            var renderer = weapon.GetComponent<SpriteRenderer>();
            var controller = weapon.GetComponent<PlayerController>();

            var strike = (GameObject)Object.Instantiate(strike_prototype);
            strike.transform.position = weapon.transform.position + new Vector3((strike.renderer.bounds.extents.x * 2 + renderer.bounds.extents.x / 2) + 0.1f, 0.025f);
            //strike.transform.localScale = new Vector3(controller.Facing * (_power + 1), 1);

            var strike_comp = strike.GetComponent<WeaponStrike>();
            //strike_comp.Power = (_power + 1);

            var strike_collider = strike.GetComponent<CircleCollider2D>();
            //strike_collider.radius *= (_power + 1);

            var x_vel = controller.GetHorizontalVelocity();
            animator.SetTempAnimation(controller.IsForehand() ? animator.HitSpritesForehand : animator.HitSpritesBackhand, Mathf.Abs(x_vel) > 0.1 ? x_vel : 1);
            controller.FlipHand();

            _cooldown = BeatMatcher.Beat/2.0f;
        }
    }

    class WeaponBehaviour : IWeaponBehaviour
    {
        private float _power;

        public IWeaponBehaviour Update(PlayerWeapon weapon, Animator animator)
        {
            

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
