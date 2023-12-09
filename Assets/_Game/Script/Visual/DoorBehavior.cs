using UnityEngine;
using System;

public class DoorBehavior : MonoBehaviour
{
    [SerializeField] PanelBehavior panel;
    private void OnEnable()
    {
        if(panel != null)
        {
            panel.OnPanelActivated += OpenDoor;
        }
    }

    private void OnDisable()
    {
        if (panel != null)
        {
            panel.OnPanelActivated -= OpenDoor;
        }
    }

    private void OpenDoor(bool isOpen)
    {
        if(isOpen)
        {
            Destroy(this.gameObject);
        }
    }
}
