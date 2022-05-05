using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaPointsBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxManaPoints(float maxManaPoints)
    {
        slider.maxValue = maxManaPoints;
        slider.value = maxManaPoints;
    }

    public void SetCurrentManaPoints(float currentManaPoints)
    {
        slider.value = currentManaPoints;
    }
}
