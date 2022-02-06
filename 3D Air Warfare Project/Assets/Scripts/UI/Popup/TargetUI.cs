using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetUI : UI_Popup
{
    public GameObject target;
    void Start()
    {
        
    }

    void Update()
    {
        if (target == null)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
        
    }
}
