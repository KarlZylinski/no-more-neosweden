using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour {
    private float _timer;
    private SpriteRenderer _sprite;
    public float Cooldown = 1.0f;
    private static System.Random _random = new System.Random();

    // Use this for initialization
	void Start ()
	{
	    _timer = Time.time + (_random.Next()%255)/255.0f;
	    _sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Time.time > _timer)
	    {
	        var c = _sprite.color;
	        c.a = c.a > 0.5f ? 0 : 1;
	        _sprite.color = c;
            _timer = Time.time + Cooldown;
	    }
	}
}
