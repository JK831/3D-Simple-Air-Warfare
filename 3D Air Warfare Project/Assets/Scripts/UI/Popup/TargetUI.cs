using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetUI : UI_Popup
{
    public GameObject target;
    public GameObject _uiOnTarget;
    void Start()
    {
        target = GameObject.Find("Enemy(Clone)");
        _uiOnTarget = gameObject.transform.Find("UIOnTarget").gameObject;
    }

    void Update()
    {
        if (target == null)
        {
            gameObject.SetActive(false);
            return;
        }

        _uiOnTarget.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
        
    }
}
