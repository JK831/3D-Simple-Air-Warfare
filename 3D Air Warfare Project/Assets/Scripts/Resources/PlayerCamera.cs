using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    void Start()
    {
        transform.localPosition = new Vector3(0, 1, -5);
        transform.rotation = Quaternion.Euler(new Vector3(1, 0, 0));
    }

    void Update()
    {
        
    }
}
