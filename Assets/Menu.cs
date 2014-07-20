using Assets.Player;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject StartItem;
    public GameObject QuitItem;
    private GameObject _current_item;
    private PlayerInput _input;

	void Start ()
	{
	    _current_item = StartItem;
	    QuitItem.renderer.enabled = false;
	    _input = GetComponent<PlayerInput>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var movement = _input.GetMovementInput();

        if (movement.y > 0.9f)
            SetActive(StartItem);
        else if (movement.y < -0.9f)
            SetActive(QuitItem);

	    if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Return))
	    {
	        DoSelected();
	    }
	}

    private void DoSelected()
    {
        if (_current_item == StartItem)
        {
            Application.LoadLevel(1);
        }
        else if (_current_item == QuitItem)
        {
            Application.Quit();
        }
    }

    void SetActive(GameObject item)
    {
        _current_item.renderer.enabled = false;
        _current_item = item;
        _current_item.renderer.enabled = true;
    }
}
