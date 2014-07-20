using UnityEngine;

namespace Assets.Player
{
    public class WeaponStrike : MonoBehaviour
    {
        public float Lifetime;
        public float Power;
        public LayerMask LayerMask;
        private float _start_time;
        private CircleCollider2D _collider;
        private PlayerController _player_controller;
        private SpriteRenderer _sprite;
        private System.Random _random;
        private int _spin_start;
        private bool _used = false;

        public void Start()
        {
            _start_time = Time.time;
            _collider = GetComponent<CircleCollider2D>();
            _player_controller = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            _sprite = GetComponent<SpriteRenderer>();
            _random = new System.Random();
            _spin_start = _random.Next();
        }

        public void Update()
        {
            var life_end = _start_time + Lifetime;
            if (Time.time > life_end)
            {
                Destroy(gameObject);
                return;
            }

            var color = _sprite.color;
            color.a = (life_end - Time.time)/Lifetime;
            _sprite.color = color;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time + _spin_start) * 10);
            var scale = Mathf.Abs(Mathf.Sin(Time.time)) + 1;
            transform.localScale = new Vector3(scale, scale, scale);

            if (_used)
                return;

            var overlapping = Physics2D.OverlapCircleAll(transform.position, _collider.radius, LayerMask);

            foreach (var o in overlapping)
            {
                _player_controller.BoostHorizontalVelocity();

                var floaty = o.GetComponent<TileFloaty>();

                if (floaty != null)
                    floaty.Explode();
                    
                var walle = o.GetComponent<WallE>();

                if (walle != null)
                {
                    walle.Explode();
                    _player_controller.Boost();
                }

                GetComponent<AudioSource>().Play();

                Destroy(o.gameObject);

                _used = true;
                return;
            }
            _used = true;
        }
    }
}
