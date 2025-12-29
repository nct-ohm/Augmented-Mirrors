using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDisplays : MonoBehaviour
{
   void Start()
    {
        // Check the number of monitors connected.
        if (Display.displays.Length > 1)
        {
            // Activate the display 1 (second monitor connected to the system).
            Display.displays[1].Activate();
        }
        if (Display.displays.Length > 2)
        {
            Display.displays[2].Activate();
        }
    }
}
