using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BurnBuildUpBar : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = 100;
        slider.value = 0;
        gameObject.SetActive(false);
    }

    public void SetBurnBuildUpAmount(int currentBurnBuildup)
    {
        slider.value = currentBurnBuildup;
    }
}
