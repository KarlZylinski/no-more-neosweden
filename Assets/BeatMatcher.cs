using System.Linq;
using Assets.Player;
using UnityEngine;

namespace Assets
{
    public class BeatMatcher : MonoBehaviour
    {
        public const float Beat = 1.0f / (128 / 60.0f);
        private PlayerController _controller;
        private float _timer;
        private Vector2 _last_spawn_player_pos;
        public GameObject[] Blocks;
        private System.Random _random;
        public int[] Lanes;
        private int _lane_combo;
        public int Lane;
        private AudioSource _music;
        private bool _doubled;

        public void Start()
        {
            var player = GameObject.FindWithTag("Player");
            _controller = player.GetComponent<PlayerController>();
            _last_spawn_player_pos = new Vector2(_controller.transform.position.x, _controller.transform.position.y);
            _random = new System.Random();
            Lanes = new[] { 4, 8, 12 };
            Lane = Lanes[0];
            _lane_combo = _random.Next() % 10 + 5;
            _music = GetComponent<AudioSource>();
            NewLane();
            _doubled = false;
        }

        public void Awake()
        {
            _timer = 0;
        }

        private void NewLane()
        {
            Lane = Lanes[_random.Next() % Lanes.Count()];
            _lane_combo = _random.Next() % 10 + 5;
        }
    
        public void Update()
        {
            _timer += Time.deltaTime;
            var current_player_pos = new Vector2(_controller.transform.position.x, _controller.transform.position.y);
            var distance_since_last_spawn = Vector2.Distance(current_player_pos, _last_spawn_player_pos);

            if (_timer >= Beat)
            {
                _timer -= Beat;

                var distance_to_spawn_at = _controller.GetHorizontalVelocity() * Beat * 16;

                var imgettinghacked = _music.time >= 65;

                var divisor = _music.time >= 143 ? 64 : (imgettinghacked ? 16 : _music.time >= 35 ? 8 : 4);

                if (imgettinghacked && !_doubled)
                {
                    _controller.DoubleHorizontalVelocity();
                    _doubled = true;
                }

                var rand_increase = _random.Next() % 100;

                if (rand_increase < (40/divisor))
                    divisor *= 2;

                if (rand_increase < (20/divisor))
                    SpawnBlock(current_player_pos, _random.Next(), distance_to_spawn_at, 1 + _random.Next() % 2);

                var min_distance = distance_to_spawn_at / divisor;

                if (distance_since_last_spawn > min_distance)
                {
                    SpawnBlock(current_player_pos, _random.Next(), distance_to_spawn_at);
                }
            }
        }

        private void SpawnBlock(Vector2 current_player_pos, int r, float distance_to_spawn_at, int extra_beat = 0)
        {
            --_lane_combo;

            if (_lane_combo == 0)
                NewLane();

            _last_spawn_player_pos = current_player_pos;
            var block =
                (GameObject)
                    Instantiate(Blocks[r%Blocks.Count()],
                        new Vector2(distance_to_spawn_at + extra_beat * _controller.GetHorizontalVelocity() * Beat, Lane * 0.16f + 0.1f) + new Vector2(current_player_pos.x, 0),
                        Quaternion.identity);
            var block_comp = block.GetComponent<TileFloaty>();
            block_comp.SetHitTime(Time.time + Beat * (16 + extra_beat));
        }
    }
}
