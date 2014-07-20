using UnityEngine;
using Animator = Assets.Player.Animator;

namespace Assets
{
    public class WallE : MonoBehaviour
    {
        public GameObject Explosion;
        private bool _up = true;
        private System.Random _random;
        private float _start_y;

        void Start ()
        {
            _random = new System.Random();
            _start_y = transform.position.y;
        }

        void Update ()
        {
            transform.position += new Vector3(-0.6f, 0, 0) * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, _start_y + (_up ? 0.005f : -0.005f), 0);
            _up = !_up;
        }

        public void Explode()
        {
            for (var i = 0; i < 10; ++i)
            {
                var go = (GameObject)Instantiate(Explosion, transform.position + new Vector3((_random.Next() % 100) / 500.0f - 0.1f, (_random.Next() % 100) / 500.0f - 0.1f, (_random.Next() % 100) / 500.0f - 0.1f), transform.rotation);
                go.GetComponent<Animator>().AnimationSpeed = _random.Next()%255/512.0f;
                go.GetComponent<SpriteRenderer>().color = new Color((_random.Next()%255)/255.0f, (_random.Next()%255)/255.0f, (_random.Next()%255)/255.0f, 1);
            }
        }
    }
}
