using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpeedSlider: MonoBehaviour
{

    public void SliderS_Changed(float newVal)
    {
        ProceduralGrid2.incSpeed = newVal;
    }
}
