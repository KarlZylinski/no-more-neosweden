using UnityEngine;

namespace Assets
{
    public class CameraController : MonoBehaviour
    {
        // State.
        public Transform Player;
        private Camera _camera;


        // Public interface.

        public void Start ()
        {
            _camera = GetComponent<Camera>();
        }

        public void Update ()
        {
            SetCameraPosition(_camera, Player.position);
        }


        // Implementation.

        private static void SetCameraPosition(Camera camera, Vector2 position)
        {
            var camera_position = camera.transform.position;
            camera_position.x = position.x;
            camera_position.y = position.y;
            camera.transform.position = camera_position;
        }
    }
}
