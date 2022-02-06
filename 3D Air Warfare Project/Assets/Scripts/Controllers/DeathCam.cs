using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCam : MonoBehaviour
{
    CinemachineOrbitalTransposer orbitalTransposer;

    // Start is called before the first frame update
    void Start()
    {
        CinemachineVirtualCamera vCam = GetComponent<CinemachineVirtualCamera>();
        orbitalTransposer = vCam?.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        orbitalTransposer.m_XAxis.Value = Random.Range(0, 360);
    }

    void Update()
    {
        orbitalTransposer.m_XAxis.m_InputAxisValue = 1;
    }
}
