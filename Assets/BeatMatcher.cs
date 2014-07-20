using System.Linq;
using Assets.Player;
using UnityEngine;

namespace Assets
{
    public class BeatMatcher : MonoBehaviour
    {
        private PlayerController _controller;
        private float _beat;
        private float _timer;
        private Vector2 _last_spawn_player_pos;
        public GameObject[] Blocks;
        private System.Random _random;
        private int[] _lanes;

        public void Start()
        {
            _controller = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            _last_spawn_player_pos = new Vector2(_controller.transform.position.x, _controller.transform.position.y);
            _random = new System.Random();
            _lanes = new[] {4, 8, 12, 16};
        }

        public void Awake()
        {
            _beat = 1.0f/(128/60.0f);
            _timer = 0;
        }
    
        public void Update()
        {
            _timer += Time.deltaTime;
            var current_player_pos = new Vector2(_controller.transform.position.x, _controller.transform.position.y);
            var distance_since_last_spawn = Vector2.Distance(current_player_pos, _last_spawn_player_pos);

            if (_timer >= _beat)
            {
                var r = _random.Next();
                _timer -= _beat;

                var distance_to_spawn_at = _controller.GetHorizontalVelocity()*_beat*6;
                if (distance_since_last_spawn > distance_to_spawn_at)
                {
                    _last_spawn_player_pos = current_player_pos;
                    var block = (GameObject)Instantiate(Blocks[r % Blocks.Count()], new Vector2(distance_to_spawn_at, 0) + current_player_pos, Quaternion.identity);
                    var block_comp = block.GetComponent<TileFloaty>();
                    block_comp.SetHitTime(Time.time + _beat);
                }
            }
        }
    }
}
