using UnityEngine;
using System.Collections;

public class Visible : MonoBehaviour
{
    private SpriteRenderer _sprite;
    public string[] Button;

	// Use this for initialization
	void Start ()
	{
	    _sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var c = _sprite.color;
        foreach(var s in Button)
        {
            var down = Input.GetKey(s);

            if (down)
            {
                c.a = 1;
                break;
            }

            c.a = 0;
        }
	    _sprite.color = c;
	}
}
