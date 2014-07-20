using System.Collections.Generic;
using Assets.Player;
using UnityEngine;

namespace Assets
{
    public class Score : MonoBehaviour
    {
        public Sprite[] Numbers;
        private int _current_score;
        private PlayerController _player_controller;
        private List<GameObject> _letters;

        void Start ()
        {
            _letters = new List<GameObject>();
            _current_score = 0;
            _player_controller = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }

        void Update ()
        {
            var new_score = _player_controller.DistanceTravelled();

            if (new_score == _current_score)
                return;

            _current_score = new_score;

            var score_as_string = _current_score.ToString();

            foreach (var go in _letters)
            {
                Destroy(go);
            }

            var width_of_letter = Numbers[0].bounds.extents.x + 0.028f;
            var current_x = 0.09f;
            foreach (var c in score_as_string)
            {
                var p = new Vector2(current_x, -0.82f);

                switch (c)
                {
                    case '1': SpawnLetter(Numbers[0], p); break;
                    case '2': SpawnLetter(Numbers[1], p); break;
                    case '3': SpawnLetter(Numbers[2], p); break;
                    case '4': SpawnLetter(Numbers[3], p); break;
                    case '5': SpawnLetter(Numbers[4], p); break;
                    case '6': SpawnLetter(Numbers[5], p); break;
                    case '7': SpawnLetter(Numbers[6], p); break;
                    case '8': SpawnLetter(Numbers[7], p); break;
                    case '9': SpawnLetter(Numbers[8], p); break;
                    case '0': SpawnLetter(Numbers[9], p); break;
                }

                current_x += width_of_letter;
            }
        }

        void SpawnLetter(Sprite letter, Vector2 position)
        {
            var go = new GameObject("letter");
            var ren = go.AddComponent<SpriteRenderer>();
            ren.sortingLayerName = "GUI_text";
            ren.sprite = letter;
            ren.transform.parent = transform;
            ren.transform.localPosition = position;
            _letters.Add(go);
        }
    }
}
