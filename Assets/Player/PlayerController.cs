using UnityEngine;

namespace Assets.Player
{
    public class PlayerController : MonoBehaviour
    {
        // State.

        private PlayerInput _player_input;
        private Vector2 _velocity;


        // Public interface.

        public void Start ()
        {
            _player_input = GetComponent<PlayerInput>();
            _velocity = new Vector2(0, 0);
        }

        public void Update()
        {
            _velocity = _player_input.GetMovementInput() * Time.deltaTime;
            transform.position += new Vector3(_velocity.x, _velocity.y, 0);
        }
    }
}
