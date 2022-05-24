using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform target;
    public float offsetRatio;

    Camera cam;
    Vector2 size;
    public Transform indicator;

    void Start()
    {
        cam = GetComponent<Camera>();
        size = new Vector2(cam.orthographicSize, cam.orthographicSize * cam.aspect);
        indicator = transform.Find("New Sprite");
    }

    void Update()
    {
        if (target == null)
        {
            Debug.Log("Target이 null");
            return;
        }

        Vector3 targetForwardVector = target.forward;
        targetForwardVector.y = 0;
        targetForwardVector.Normalize();

        Vector3 position = new Vector3(target.transform.position.x, 1000, target.transform.position.z)
                           + targetForwardVector * offsetRatio * cam.orthographicSize;
        transform.position = position;
        transform.eulerAngles = new Vector3(90, 0, -target.eulerAngles.y);
    }

    public void ShowBorderIndicator(Vector3 position)
    {
        if (target == null)
        {
            Debug.Log("Target이 null");
            return;
        }
        float reciprocal;
        float rotation;
        Vector2 distance = new Vector3(transform.position.x - position.x, transform.position.z - position.z); //오브젝트와의 거리 계산

        distance = Quaternion.Euler(0, 0, target.eulerAngles.y) * distance; // 카메라는 아래를 향하고 있으므로 eulerAngles.y가 월드 좌표계에서 z축 방향이 된다


        // X axis
        if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
        {
            reciprocal = Mathf.Abs(size.x / distance.x);
            rotation = (distance.x > 0) ? 90 : -90; //화살표의 회전 값! (왼쪽일 땐 왼쪽 방향, 오른쪽일 땐 오른쪽 방향 바라보게)
        }
        // Y axis
        else
        {
            reciprocal = Mathf.Abs(size.y / distance.y);
            rotation = (distance.y > 0) ? 180 : 0;
        }

        indicator.localPosition = new Vector3(distance.x * -reciprocal, distance.y * -reciprocal, 1);
        indicator.localEulerAngles = new Vector3(0, 0, rotation);
        indicator.gameObject.SetActive(true);
    }

    public void HideBorderInditator()
    {
        if (target == null)
            return;
        indicator.gameObject.SetActive(false);
    }
}
