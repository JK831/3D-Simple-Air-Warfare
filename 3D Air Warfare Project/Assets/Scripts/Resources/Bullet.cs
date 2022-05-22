using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    int damage;

    TrailRenderer trailRenderer;

    Rigidbody rb;

    public Transform target;

    public float turningForce;

    public float speed;
    public float lifetime;
    private Vector3 direction;

    public float boresightAngle = 30.0f;

    public void Fire(float launchSpeed, int layer)
    {
        speed += launchSpeed;
        gameObject.layer = layer;
        rb.velocity = transform.forward * speed;
    }

    public void launch(Transform target)
    {
        this.target = target;
    }

    void LookAtTarget()
    {
        if (target == null)
            return;

        Vector3 targetDir = target.position - transform.position;
        if (targetDir.magnitude > 300)
            return;
        float angle = Vector3.Angle(targetDir, transform.forward);

        if (angle > boresightAngle)
        {
            target = null;
            return;
        }

        Quaternion lookRotation = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turningForce * Time.deltaTime);
        Debug.Log("회전 !");
    }

    void OnCollisionEnter(Collision other)
    {
        CheckLayer(other);
        
        CreateHitEffect();
        DisableBullet();
    }

    void CheckLayer(Collision other)
    {
        if (gameObject.layer != other.gameObject.layer && other.gameObject.layer != 9)
        {
            Debug.Log("CheckLayer");
            other.gameObject.GetComponent<TargetObject>()?.OnDamage(damage);
        }

    }

    void CreateHitEffect()
    {
        GameObject effect = EffectPool.GetObject();
        effect.transform.position = transform.position;
        effect.transform.rotation = transform.rotation;
        effect.SetActive(true);
    }

    void DisableBullet()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        ObjectPool.ReturnObject(gameObject);
    }

    public void OnEnable()
    {
        
        Invoke("DisableBullet", lifetime);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
        LookAtTarget();
    }
}
