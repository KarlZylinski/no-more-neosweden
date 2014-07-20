using System.Linq;
using UnityEngine;

namespace Assets
{
    public class RandomSprite : MonoBehaviour
    {
        public Sprite[] Sprites;

        public void Start ()
        {
            var random = new System.Random();
            GetComponent<SpriteRenderer>().sprite = Sprites[random.Next()%Sprites.Count()];
        }

        public void Update()
        {

        }
    }
}
