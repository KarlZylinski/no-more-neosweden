using UnityEngine;
using System.Collections;

public class Visible : MonoBehaviour
{
    private SpriteRenderer _sprite;
    public string Button;

	// Use this for initialization
	void Start ()
	{
	    _sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var c = _sprite.color;
        c.a = Input.GetKey(Button) ? 1 : 0;
	    _sprite.color = c;
	}
}
