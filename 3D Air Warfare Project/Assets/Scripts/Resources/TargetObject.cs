using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TargetObject : MonoBehaviour
{
    [SerializeField]
    protected ObjectInfo objectInfo;

    [SerializeField]
    protected GameObject destroyEffect;


    protected int hp;
    public bool isNextTarget;

    public ObjectInfo Info
    {
        get
        {
            return objectInfo;
        }
    }

    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }

    protected GameObject CommonDestroyFunction()
    {
        return Instantiate(destroyEffect, transform.position, Quaternion.identity);
        
    }

    public virtual void OnDamage(int damage)
    {
        hp -= damage;
        Managers.User.PlayerScore += 10;

        if (hp <= 0)
        {
            if (--Managers.Stage.RemainEnemy == 0)
            {
                Managers.Stage.Victory = true;
                Managers.Stage.GameOver();
            }
            DelayedDestroy();
        }
    }


    public virtual void DestroyObject()
    {
        DestroyingEffect();
        Destroy(gameObject);
    }

    public virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            other.gameObject.GetComponent<TargetObject>() != null)
        {
            DestroyObject();
        }
    }



    protected void DelayedDestroy()
    {
        DestroyingEffect();
        Invoke("DestroyingEffect", 2.0f);
        Destroy(gameObject, 2.0f);

        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }

    void DestroyingEffect() // 폭발 이펙트 생성 후 2초 후 파괴
    {
        GameObject obj = CommonDestroyFunction();
        Destroy(obj, 2.0f);
    }

    protected virtual void Start()
    {

        hp = objectInfo.HP;
    }

    protected void OnDestroy()
    {

    }
}
