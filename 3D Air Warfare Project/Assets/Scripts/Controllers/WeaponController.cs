using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponController : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField]
    AudioClip ShootingAudioClip;

    public Transform gunTransform;
    public Transform target;
    public float gunRPM;
    float fireInterval;
    float time = 0;
    float aircraftSpeed;

    AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        Managers.Input.KeyAction -= onKeyBoard;
        Managers.Input.KeyAction += onKeyBoard;
        fireInterval = 60.0f / gunRPM;
        Debug.Log("Weapon equiped !");
    }

    public void UpdateAircraftSpeed(float speed)
    {
        aircraftSpeed = speed;
    }

    private void Update()
    {
        time += Time.deltaTime;
    }

    void onKeyBoard()
    {
        if (Input.GetKey(KeyCode.J))
        {
            if (time >= fireInterval)
            {
                time = 0;
                FireMachineGun();
            }
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
