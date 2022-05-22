using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : TargetObject
{
    public override void OnDamage(int damage)
    {
        hp -= damage;

        //Managers.UI.InGameUI.hp = hp;

        //Managers.UI.InGameUI.score++;  

        if (hp <= 0)
        {
            DelayedDestroy();
            Managers.Stage.GameOver();
        }
        
    }

    public override void DestroyObject()
    {
        PlayerController ctr = gameObject.GetComponent<PlayerController>();
        ctr.updateDie();
        base.DestroyObject();
    }

    public override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            other.gameObject.GetComponent<TargetObject>() != null)
        {
            PlayerController ctr = gameObject.GetComponent<PlayerController>();
            ctr.updateDie();
            DestroyObject();
        }
    }
}
