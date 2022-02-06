using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public float lifetime;


    public void OnEnable()
    {
        Invoke("DisableEffect", lifetime);
    }

    void DisableEffect()
    {
        EffectPool.ReturnObject(gameObject);
    }
}
