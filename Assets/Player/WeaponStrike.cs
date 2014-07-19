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

        public void Start()
        {
            _start_time = Time.time;
            _collider = GetComponent<CircleCollider2D>();
        }

        public void Update()
        {
            if (Time.time > _start_time + Lifetime)
            {
                Destroy(gameObject);
                return;
            }

            var overlapping = Physics2D.OverlapCircleAll(transform.position, _collider.radius, LayerMask);

            foreach (var o in overlapping)
            {
                Destroy(o.gameObject);
            }
        }
    }
}
