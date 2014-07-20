using UnityEngine;

namespace Assets
{
    public class SettingsSetter : MonoBehaviour {
        void Update()
        {

            if (Input.GetKeyDown("joystick button 7") || Input.GetKeyDown(KeyCode.Escape))
            {
                Application.LoadLevel(2);
            }
        }
    }
}
