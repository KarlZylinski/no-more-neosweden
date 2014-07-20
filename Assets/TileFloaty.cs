using UnityEngine;

namespace Assets
{
    public class TileFloaty : MonoBehaviour
    {
        private float _start_y;
        private float _shadow_start_y;
        private Transform _shadow;
        private float _hit_time;
        private Transform _player;
        public GameObject Explosion;
        public Color ExplosionTint;

        public void Start()
        {
            _start_y = transform.position.y;
            _shadow = transform.GetChild(0);
            _shadow_start_y = _shadow.transform.position.y;
            _player = GameObject.FindWithTag("Player").transform;
        }

        public void Update()
        {
            FloatVertical();
            NearPlayer();
        }

        private void NearPlayer()
        {
            var time_left = (_hit_time - Time.time);

            var distance_to_player = Vector3.Distance(new Vector3(_player.position.x, 0, 0), (new Vector3(transform.position.x, 0, 0)));

            if (time_left > 0)
                transform.position -= new Vector3(distance_to_player / time_left, 0, 0) * Time.deltaTime;

            if (_player.position.x > transform.position.x)
            {
                var rel_dist = distance_to_player/1.5f;
                transform.GetComponent<SpriteRenderer>().color = new Color(1 - rel_dist, 1 - rel_dist, 1 - rel_dist, 1 - rel_dist);
                if (distance_to_player > 1.5f)
                    Destroy(gameObject);
            }
        }

        public void Explode()
        {
            var go = (GameObject)Instantiate(Explosion, transform.position, transform.rotation);
            go.GetComponent<SpriteRenderer>().color = ExplosionTint;
        }

        private void FloatVertical()
        {
            var offset_y = Mathf.Sin(Time.time*10)/50.0f;
            transform.position = new Vector3(transform.position.x, _start_y + offset_y);
            _shadow.position = new Vector3(_shadow.position.x, _shadow_start_y - offset_y/4);
            _shadow.localScale = new Vector3(1 - offset_y*4, 1 - offset_y*4);
        }

        public void SetHitTime(float time)
        {
            _hit_time = time;
        }
    }
}
