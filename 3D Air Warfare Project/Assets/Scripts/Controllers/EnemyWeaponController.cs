using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField]
    AudioClip ShootingAudioClip;

    float time = 0;

    public Transform gunTransform;
    public Transform target;
    public float gunRPM;
    float fireInterval;
    float aircraftSpeed;
    EnemyController enemyController;

    AudioSource audioSource;

    public void UpdateAircraftSpeed(float speed)
    {
        aircraftSpeed = speed;
    }

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        fireInterval = 60.0f / gunRPM;
    }

    void Update()
    {
        if (target == null)
            return;
        time += Time.deltaTime;
        Vector3 targetDirection = target.position - transform.position;
        float angle = Vector3.Angle(targetDirection, transform.forward);
        if (targetDirection.magnitude <= 350 && angle <=45)
        {
            if (time >= fireInterval)
            {
                time = 0;
                FireMachineGun();
            }
                
        }
        else
        {
            CancelInvoke("FireMachineGun");
        }
    }

    void FireMachineGun()
    {

        GameObject bullet = ObjectPool.GetObject();
        bullet.layer = gameObject.layer;
        bullet.transform.position = gunTransform.position;
        bullet.transform.rotation = transform.rotation;
        bullet.SetActive(true);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.launch(target);
        bulletScript.Fire(aircraftSpeed, bullet.layer);

        audioSource.PlayOneShot(ShootingAudioClip);
    }
}
