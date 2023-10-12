using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    private void Start()
    {
        UnitActionSystem.Instance.OnBusyChanged += UnityActionSystem_OnBusyChanged;

        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UnityActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        if (isBusy) { Show(); }
        else { Hide(); }
    }
}
