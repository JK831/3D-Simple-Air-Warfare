using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MinimapSprite : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public MinimapCamera minimapCamera;
    public Camera minimap_Camera;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    bool IsVisibleToCamera(Transform transform)
    {
        Vector3 visTest = minimap_Camera.WorldToViewportPoint(transform.position);
        return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (IsVisibleToCamera(transform))
        {
            Debug.Log("난 안보여!");
            minimapCamera.HideBorderInditator();
        }
        else
        {
            Debug.Log("난 보여!");
            minimapCamera.ShowBorderIndicator(transform.position);
        }
    }
}
