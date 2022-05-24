using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : TargetObject
{
    public override void OnDamage(int damage)
    {
        hp -= damage;

        Managers.User.PlayerScore--;
        if (hp <= 0)
        {
            PlayerController ctr = gameObject.GetComponent<PlayerController>();
            ctr.updateDestroied();
            DelayedDestroy();
            Managers.Stage.GameOver();
        }
        
    }

    public override void DestroyObject()
    {
        PlayerController ctr = gameObject.GetComponent<PlayerController>();
        ctr.updateDestroied();
        base.DestroyObject();
    }

    public override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            other.gameObject.GetComponent<TargetObject>() != null)
        {
            DestroyingEffect();
            PlayerController ctr = gameObject.GetComponent<PlayerController>();
            ctr.updateDestroied();
            DestroyObject();
            Managers.Stage.GameOver();
        }
    }
}
