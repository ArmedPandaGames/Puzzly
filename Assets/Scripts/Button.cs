using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{

    [SerializeField] private Color showingColor = Color.white;
    [SerializeField] private Color hidingColor = Color.blue;
    public bool chosen = false;

    public bool isPressed = false;

    public bool canBePressed = false;

    void Start()
    {
        Image image = GetComponent<Image>();
        image.color = hidingColor;
    }
    public void Show()
    {
        Image image = GetComponent<Image>();
        image.color = showingColor;
    }

    public void Hide()
    {
        Image image = GetComponent<Image>();
        image.color = hidingColor;
    }

    public void Toggle()
    {
        if (!canBePressed) return;


        isPressed = !isPressed;
        if (isPressed)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void ResetState()
    {
        chosen = false;
        isPressed = false;
        canBePressed = false;
        Hide();
    }
}
