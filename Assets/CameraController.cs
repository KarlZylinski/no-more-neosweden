using Assets.Player;
using UnityEngine;

namespace Assets
{
    public class CameraController : MonoBehaviour
    {
        // State.
        public GameObject Player;
        private PlayerController _player_controller;
        private Camera _camera;


        // Public interface.

        public void Start ()
        {
            _camera = GetComponent<Camera>();
            _player_controller = Player.GetComponent<PlayerController>();
        }

        public void Update ()
        {
            SetCameraPosition(_camera, Player.transform.position, _player_controller.Facing);
        }


        // Implementation.

        private static void SetCameraPosition(Camera camera, Vector2 position, float facing)
        {
            var camera_position = camera.transform.position;
            camera_position.x = position.x + (camera.rect.width / 320);
            camera_position.y = position.y + (camera.rect.height / 180 / 2);
            camera.transform.position = camera_position;
        }
    }
}
