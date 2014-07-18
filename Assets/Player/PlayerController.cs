using UnityEngine;

namespace Assets.Player
{
    public class PlayerController : MonoBehaviour
    {
        private Vector3 _velocity;

        public void Start ()
        {
            _velocity = new Vector3(0, 0, 0);
        }

        public void Update ()
        {
            transform.position += _velocity;
        }
    }
}
