using UnityEngine;

namespace Assets
{
    public class ScoreScreen : MonoBehaviour {
        public Sprite[] Numbers;

        void Start()
        {
            var score_object = GameObject.Find("ScoreObject");

            if (score_object == null)
                return;

            var score_as_string = score_object.GetComponent<ScoreObject>().Score.ToString();

            var width_of_letter = Numbers[0].bounds.extents.x + 0.028f;
            var current_x = 0.09f;
            foreach (var c in score_as_string)
            {
                var p = new Vector2(current_x, 0);

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
        }

        void Update()
        {
            if (Input.GetKeyDown("joystick button 7") || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
            {
                Application.LoadLevel(0);
            }
        }
    }
}
