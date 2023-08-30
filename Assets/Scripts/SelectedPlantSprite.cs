using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedPlantSprite : MonoBehaviour
{

    private Image image;
    private bool IsActivated;

    private void Awake()
    {
        image = GetComponent<Image>();

        Deactivate();
    }
    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    private void Update()
    {
        if (IsActivated)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void Activate()
    {
        IsActivated = true;
        image.enabled = true;
    }

    public void Deactivate()
    {
        IsActivated = false;
        image.enabled = false;
    }
}
