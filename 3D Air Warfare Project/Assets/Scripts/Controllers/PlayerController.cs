using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float rotateSpeedOnDestroy = 600;

    float maxSpeed = 301.7f; // 최대 속력
    float minSpeed = 30.0f;
    
    float speedReciprocal; // maxSpeed의 역수
    float _accelValue = 15.0f;
    float _breakValue = 15.0f;

    float _speed = 10.0f;
    float _gravity = 9.8f;          // 중력값
    float _fall_speed = 0;

    float turningForce = 1.0f;

    public Camera deathCam;


    Vector3 forward = new Vector3(0, 0, 1);
    Vector3 up = new Vector3(0, 1, 0);
    Vector3 right = new Vector3(1, 0, 0);



    public float Speed
    {
        get { return _speed; }
    }

    public enum PlayerState
    {
        Die,
        Acceleration,
        flying,
        Stop,
        Attack,
        Victory,
    }

    public PlayerState _state = PlayerState.flying;

    public void DisableControl()
    {
        this.enabled = false;
    }


    void Start()
    {

        Managers.Input.KeyAction -= OnKeyBoardPressed;
        Managers.Input.KeyAction += OnKeyBoardPressed;
 
        speedReciprocal = 1 / maxSpeed;

        rotateSpeedOnDestroy *= Random.Range(0.5f, 1.0f);
        
    }
    public void updateDestroied()
    {
        _state = PlayerState.Die;
        DisableControl();
        Managers.Input.KeyAction -= OnKeyBoardPressed;
        
        //deathCam.gameObject.SetActive(true);
        transform.position += transform.forward * Time.deltaTime * _speed;
        transform.Rotate(0, 0, rotateSpeedOnDestroy * Time.deltaTime);
        //Managers.UI.InGameUI.GameOver();
    }

    public void updateVictory()
    {
        _state = PlayerState.Victory;
        DisableControl();
        Managers.Input.KeyAction -= OnKeyBoardPressed;
    }
    void UpdateFlying()
    {
        if (_speed > 30.0f)
        {
            _speed *= 0.9999f;
            _fall_speed = 0;
        }

        else if (_speed > 0.0f) // 속력이 일정 수치 이하로 내려가면 양력이 중력보다 약해짐
        {
            if (_fall_speed <= 150.0f) // 종단 속도까지
                _fall_speed += _gravity;

            if (_speed <= 10.0f && _speed > 0.0f) // 속력이 일정 수치 이하로 내려가면 speed 절댓값 감소
            {
                _speed -= 0.01f;

            }

            if (_speed == 0.0f && _fall_speed == 0.0f)
                _state = PlayerState.Stop;

        }
        else
        {
            _speed = 0.0f;
        }
            
        transform.position += transform.forward * Time.deltaTime * _speed;
        //Managers.UI._sceneUI.SetSpeed(_speed);
        WeaponController weapon = gameObject.GetComponent<WeaponController>();
        weapon.UpdateAircraftSpeed(_speed);
    }

    void UpdateStop()
    {
        
    }

    void UpdateBreak()
    {
        if(_state == PlayerState.flying)
        {
            float brakeEase = (_speed - minSpeed) * speedReciprocal;
            _speed -= _breakValue * brakeEase * Time.deltaTime;
        }
    }



    void OnKeyBoardPressed()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (_state == PlayerState.Stop)
                _state = PlayerState.flying;
            float accelEase = (maxSpeed - _speed) * speedReciprocal;
            _speed += _accelValue * accelEase * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            UpdateBreak();
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.localRotation *= Quaternion.Euler(right * turningForce);
            //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
            //transform.position += Vector3.forward * Time.deltaTime * _speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.localRotation *= Quaternion.Euler(-right * turningForce);

        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.localRotation *= Quaternion.Euler(forward * turningForce);


        }
        if (Input.GetKey(KeyCode.L))
        {
            transform.localRotation *= Quaternion.Euler(-forward * turningForce);


        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.localRotation *= Quaternion.Euler(-up * turningForce);

        }
        if (Input.GetKey(KeyCode.P))
        {
            transform.localRotation *= Quaternion.Euler(up * 2.5f);

        }
       
    }

    void fire()
    {

    }

    void updateAttack()
    {

    }


    void Update()
    {
    
        switch (_state)
        {
            case PlayerState.flying:
                UpdateFlying();
                break;
            case PlayerState.Stop:
                UpdateStop();
                break;
            case PlayerState.Die:
                return;
            case PlayerState.Victory:
                return;
            }
       
    }
}
