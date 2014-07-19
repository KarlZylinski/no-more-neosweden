using UnityEngine;

namespace Assets
{
    public class TileFloaty : MonoBehaviour
    {
        private float _start_y;
        private float _shadow_start_y;
        private Transform _shadow;

        public void Start()
        {
            _start_y = transform.position.y;
            _shadow = transform.GetChild(0);
            _shadow_start_y = _shadow.transform.position.y;
        }

        public void Update()
        {
            var time = Time.time;
            var offset_y = Mathf.Sin(Time.time)/100.0f;
            transform.position = new Vector3(transform.position.x, _start_y + offset_y);
            _shadow.position = new Vector3(_shadow.position.x, _shadow_start_y - offset_y / 4);
            _shadow.localScale = new Vector3(1 - offset_y * 4, 1 - offset_y * 4);
        }
    }
}
