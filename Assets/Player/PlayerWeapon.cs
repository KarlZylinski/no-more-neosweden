using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Player
{
    interface IWeaponBehaviour
    {
        IWeaponBehaviour Update(Vector2 input, PlayerWeapon weapon);
    }

    class IdleWeaponBehaivour : IWeaponBehaviour
    {
        public IWeaponBehaviour Update(Vector2 input, PlayerWeapon weapon)
        {
            if (input.magnitude < 0.1f)
                return this;

            var rotation = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;

            if (rotation >= 135 || rotation <= -135)
                return new WeaponBehaviour(weapon.MagentaStrikePrototype);
            if (rotation < 135 && rotation > 45)
                return new WeaponBehaviour(weapon.OrangeStrikePrototype);
            if (rotation <= 45 && rotation >= -45)
                return new WeaponBehaviour(weapon.YellowStrikePrototype);
            if (rotation < -45 && rotation > -135)
                return new WeaponBehaviour(weapon.BlueStrikePrototype);

            return this;
        }
    }

    class WeaponBehaviour : IWeaponBehaviour
    {
        private float _power;
        private readonly GameObject _strike_prototype;

        public WeaponBehaviour(GameObject strike_prototype)
        {
            _strike_prototype = strike_prototype;
        }

        public IWeaponBehaviour Update(Vector2 input, PlayerWeapon weapon)
        {
            _power += Time.deltaTime;
            var collider = weapon.GetComponent<CircleCollider2D>();
            var controller = weapon.GetComponent<PlayerController>();

            if (input.magnitude < 0.1f)
            {
                var magenta_strike = (GameObject)Object.Instantiate(_strike_prototype);
                magenta_strike.transform.position = weapon.transform.position + new Vector3((magenta_strike.renderer.bounds.extents.x * 2  + collider.radius) * controller.Facing, 0);
                magenta_strike.transform.localScale = new Vector3(controller.Facing, 1);
                
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

        // Public interface.

        void Start ()
        {
            _input = GetComponent<PlayerInput>();
            _current_weapon_behaviour = new IdleWeaponBehaivour();
        }

        void Update ()
        {
            var attack_input = _input.GetAttackInput();
            _current_weapon_behaviour = _current_weapon_behaviour.Update(attack_input, this);
        }
    }
}
