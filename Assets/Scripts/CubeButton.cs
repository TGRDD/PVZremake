using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CubeButton : MonoBehaviour
{
    public UnityEvent OnButtonClick;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnButtonClick.Invoke();
        }
    }
}
