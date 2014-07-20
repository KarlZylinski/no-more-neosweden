using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    public GameObject StartItem;
    public GameObject QuitItem;
    private GameObject _current_item;

	void Start ()
	{
	    _current_item = StartItem;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
