using UnityEngine;

namespace Assets
{
    public class Center : MonoBehaviour {

        // Use this for initialization
        void Start () {
	
        }
	
        // Update is called once per frame
        void Update ()
        {
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -1.5f);
        }
    }
}
