using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteDisplay : MonoBehaviour
{

    private SpriteRenderer sRenderer;

    private void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
    }

    public void Display(Sprite sprite)
    {
        sRenderer.sprite = sprite;
    }

}
