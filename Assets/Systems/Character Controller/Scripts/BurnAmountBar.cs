using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BurnAmountBar : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = 100;
        slider.value = 100;
        gameObject.SetActive(false);
    }

    public void SetBurnAmount(int burnAmount)
    {
        slider.value = burnAmount;
    }
}
