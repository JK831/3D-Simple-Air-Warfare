using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.TPView;
    [SerializeField]
    private Vector3 _delta = new Vector3(0.0f, 4.0f, -8.0f);
    [SerializeField]
    private Quaternion _rotation = new Quaternion(7.0f, 0.0f, 0.0f, 0.0f);
    [SerializeField]
    private GameObject _player = null;

    public Transform thirdViewCameraPivot;

    float zoomValue;
    public float zoomLerpAmount;
    public float zoomAmount;

    float rollValue;
    public float rollLerpAmount;
    public float rollAmount;

    float pitchValue;
    public float pitchLerpAmount;
    public float pitchAmount;



    void Start()
    {

    }

    void LateUpdate()
    {
        if (_mode == Define.CameraMode.TPView)
        {
            RaycastHit hit;
            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Wall")))
            {
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;
                transform.position = _player.transform.position + _delta.normalized * dist;
            }
            else
            {
                

                transform.position = _player.transform.position + _delta;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, _player.transform.up + new Vector3(7.0f, 0.0f, 0.0f));
                transform.rotation = Quaternion.FromToRotation(Vector3.forward, _player.transform.forward + new Vector3(7.0f, 0.0f, 0.0f));
                

            }
        }
    }

    public void SetTPView(Vector3 delta, Quaternion rotation)
    {
        _mode = Define.CameraMode.TPView;
        _delta = delta;
        _rotation = rotation;
    }



    public void AdjustCameraValue(float aircraftAccelValue, float aircraftRollValue, float aircraftPitchValue)
    {
        zoomValue = Mathf.Lerp(zoomValue, aircraftAccelValue, zoomLerpAmount * Time.deltaTime);
        rollValue = Mathf.Lerp(rollValue, aircraftRollValue, rollLerpAmount * Time.deltaTime);
        pitchValue = Mathf.Lerp(pitchValue, aircraftPitchValue, pitchLerpAmount * Time.deltaTime);
    }

}
