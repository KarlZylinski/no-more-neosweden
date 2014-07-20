using UnityEngine;

namespace Assets
{
    public class ScoreObject : MonoBehaviour
    {
        public int Score;

        // Use this for initialization
        void Start () {
            DontDestroyOnLoad(this);
        }
	
        // Update is called once per frame
        void Update () {
	
        }
    }
}
