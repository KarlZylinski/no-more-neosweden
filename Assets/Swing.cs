using UnityEngine;
using System.Collections;

public class Swing : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
        transform.rotation = Quaternion.Euler(Mathf.Cos(Time.time), 0, Mathf.Sin(Time.time));
	}
}
