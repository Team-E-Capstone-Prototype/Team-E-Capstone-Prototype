using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAppear : MonoBehaviour
{
    [SerializeField] private Image HandGrabImage;
    public bool b_isHandGrabShowing = false;

    public void ShowHandGrabUI()
    {
        if (b_isHandGrabShowing == false)
        {
            HandGrabImage.enabled = true;
            b_isHandGrabShowing = true;
        }
    }

    public void HideHandGrabUI()
    {
        if (b_isHandGrabShowing == true)
        {
            HandGrabImage.enabled = false;
            b_isHandGrabShowing = false;
        }
    }
}
