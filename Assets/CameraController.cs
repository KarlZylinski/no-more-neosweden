using Assets.Player;
using UnityEngine;

namespace Assets
{
    public class CameraController : MonoBehaviour
    {
        // State.
        private GameObject _player;
        private PlayerController _player_controller;
        private Camera _camera;


        // Public interface.

        public void Start ()
        {
            _camera = GetComponent<Camera>();
            _player = GameObject.FindWithTag("Player");
            _player_controller = _player.GetComponent<PlayerController>();
        }

        public void Update ()
        {
            SetCameraPosition(_camera, _player.transform.position, _player_controller.Facing);
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
