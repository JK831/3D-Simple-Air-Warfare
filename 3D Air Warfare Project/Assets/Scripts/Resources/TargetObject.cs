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

    protected bool isEnemy;
    protected int hp;
    public bool isNextTarget;

    public ObjectInfo Info
    {
        get
        {
            return objectInfo;
        }
    }

    protected GameObject CommonDestroyFunction()
    {
        return Instantiate(destroyEffect, transform.position, Quaternion.identity);
        
    }

    public virtual void OnDamage(int damage)
    {
        hp -= damage;
        if (gameObject.tag.Equals("Player"))
            UIController.Instance.hp = hp;
        if (hp <= 0)
        {
            DelayedDestroy();
        }
    }


    public virtual void DestroyObject()
    {
        DestroyingEffect();
        Destroy(gameObject);
        UIController.Instance.GameOver();

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            other.gameObject.GetComponent<TargetObject>() != null)
        {
            PlayerController ctr = gameObject.GetComponent<PlayerController>();
            ctr.updateDie();
            DestroyObject();
        }
    }



    void DelayedDestroy()
    {
        DestroyingEffect();
        Invoke("DestroyingEffect", 2.0f);
        Destroy(gameObject, 2.0f);
        if (objectInfo.ObjectName.Equals("Player"))
        {
            UIController.Instance.GameOver();
            PlayerController ctr = gameObject.GetComponent<PlayerController>();
            ctr.updateDie();
        }
        else
        {
            UIController.Instance.GameOver();
        }

        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }

    void DestroyingEffect()
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
