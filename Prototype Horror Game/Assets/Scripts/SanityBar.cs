using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanityBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;

    public Image fill;

    public void SetMaxSanity(int maxSanity)
    {
        slider.maxValue = maxSanity;
        slider.value = maxSanity;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetSanity(float sanity)
    {
        slider.value = sanity;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

}
