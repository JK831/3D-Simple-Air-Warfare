using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("사라집니다 !");
        if(collision.gameObject.layer == LayerMask.GetMask("Enemy"))
            Destroy(gameObject, 2.0f);
    }

   
}
