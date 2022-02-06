using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject _player;

    [SerializeField]
    float maxSpeed;
    [SerializeField]
    float minSpeed;
    [SerializeField]
    float defaultSpeed;

    float speed;


    [SerializeField]
    List<Transform> initialWaypoints;
    Queue<Transform> waypointQueue;

    float prevWaypointDistance;
    float waypointDistance;
    bool isComingClose;

    float prevRotY;
    float currRotY;
    float rotateAmount;
    float zRotateValue;

    float turningTime;
    float currentTurningForce;
    float currentTurningTime;

    [SerializeField]
    float newWaypointDistance;
    [SerializeField]
    float waypointMinHeight;
    [SerializeField]
    float waypointMaxHeight;
    [SerializeField]
    BoxCollider areaCollider;

    Vector3 currentWaypoint;

    [SerializeField]
    GameObject waypointObject;

    [Header("Accel/Rotate Values")]
    [SerializeField]
    float accelerateAmount = 50.0f;
    [SerializeField]
    float turningForce;

    [Header("Z Rotate Value")]
    [SerializeField]
    float zRotateMaxThreshold = 0.5f;
    [SerializeField]
    float zRotateAmount = 90;

    [SerializeField]
    [Range(0, 1)]
    float playerTrackingRate = 0.5f;

    float currentAccelerate;
    float accelerateReciprocal;
    bool isAcceleration = true;
    float targetSpeed = 80.0f;

    enum EnemyState
    {
        Tracking,
        InBattle,
        Patroll,
    }

    EnemyState _state = EnemyState.Patroll;

    public void updateDie()
    {
        EnemyWeaponController wpctr = gameObject.GetComponent<EnemyWeaponController>();
        wpctr.enabled = false;
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    void RandomizeSpeedAndTurn()
    {
        // Speed
        targetSpeed = Random.Range(minSpeed, maxSpeed);
        isAcceleration = (speed < targetSpeed);

        currentTurningForce = Random.Range(0.5f * turningForce, turningForce);
        turningTime = 1 / currentTurningForce;
    }

    void AdjustSpeed()
    {
        currentAccelerate = 0;
        if (isAcceleration == true && speed < targetSpeed)
        {
            currentAccelerate = accelerateAmount;
        }
        else if (isAcceleration == false && speed > targetSpeed)
        {
            currentAccelerate = -accelerateAmount;
        }
        speed += currentAccelerate * Time.deltaTime;

        currentTurningTime = Mathf.Lerp(currentTurningTime, turningTime, 1);
    }

    Vector3 CreateWayPointToPlayer()
    {
        Vector3 direction = _player.transform.position - transform.position;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, _player.transform.position, out hit, LayerMask.GetMask("Ground")))
        {
            return CreateWaypointAroundItSelf();

        }
        else
        {
            if (_state == EnemyState.Tracking)
            {
                Debug.Log("추적 ->정찰!");
                _state = EnemyState.Patroll;
            }
            return _player.transform.position;
        }
    }


    Vector3 CreateWaypoint()
    {
        float rate = Random.Range(0.0f, 1.0f);
        if (_state == EnemyState.InBattle ||_state == EnemyState.Tracking)
        {
            return CreateWayPointToPlayer();
        }

        else if (rate < playerTrackingRate && _state == EnemyState.Patroll)
        {
            Debug.Log("추적!");
            _state = EnemyState.Tracking;
            return CreateWayPointToPlayer();
            
        }
        else
        {
            if (areaCollider == null)
            {
                return CreateWaypointAroundItSelf();
            }
            else
                return CreateWaypointWithInArea();
        }
    }

    Vector3 CreateWaypointWithInArea()
    {
        float height = Random.Range(waypointMinHeight, waypointMaxHeight);
        Vector3 waypointPosition = RandomPointInBounds(areaCollider.bounds);
        RaycastHit hit;
        
        Physics.Raycast(waypointPosition, Vector3.down, out hit);

        if (hit.distance != 0)
        {
            waypointPosition.y += height - hit.distance;

        }
        // New waypoint is below ground
        else
        {
            Physics.Raycast(waypointPosition, Vector3.up, out hit);
            waypointPosition.y += height + hit.distance;

        }

        Vector3 waypointDirection = waypointPosition - transform.position;
        int layermask = 1 << LayerMask.NameToLayer("Ground");

        

        Instantiate(waypointObject, waypointPosition, Quaternion.identity);

        currentWaypoint = waypointPosition;

        Debug.Log("새 지점 생성");
        return waypointPosition;
    }

    Vector3 CreateWaypointAroundItSelf()
    {
        float height = Random.Range(waypointMinHeight, waypointMaxHeight);
        Vector3 waypointPosition = RandomPointInBounds(areaCollider.bounds);

        RaycastHit hit;
        Physics.Raycast(waypointPosition, Vector3.down, out hit);

        if (hit.distance != 0)
        {
            waypointPosition.y += height - hit.distance;
            
        }
        // New waypoint is below ground
        else
        {
            Physics.Raycast(waypointPosition, Vector3.up, out hit);
            waypointPosition.y += height + hit.distance;
            
        }

        Vector3 waypointDirection = waypointPosition - transform.position;

        int layermask = 1 << LayerMask.NameToLayer("Ground");


        Instantiate(waypointObject, waypointPosition, Quaternion.identity);

        currentWaypoint = waypointPosition;
        return waypointPosition;
    }


    void ChangeWaypoint()
    {
        if (waypointQueue.Count == 0)
        {
            currentWaypoint = CreateWaypoint();
        }
        else
        {
            currentWaypoint = waypointQueue.Dequeue().position;
        }

        isComingClose = false;
        RandomizeSpeedAndTurn();
    }

    void CheckWaypoint()
    {
        
        waypointDistance = Vector3.Distance(transform.position, currentWaypoint);
        if (waypointDistance >= prevWaypointDistance) // Aircraft is going farther from the waypoint
        {
            if (isComingClose == true)
            {
                ChangeWaypoint();
            }
        }
        else
        {
            isComingClose = true;
        }
        prevWaypointDistance = waypointDistance;
    }

    void Rotate()
    {
    
        Vector3 targetDir = currentWaypoint - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(targetDir);
        float delta = Quaternion.Angle(transform.rotation, lookRotation);
        if (delta > 0f)
        {
            float lerpAmount = Mathf.SmoothDampAngle(delta, 0.0f, ref rotateAmount, currentTurningTime);
            lerpAmount = 1.0f - (lerpAmount / delta);

            Vector3 eulerAngle = lookRotation.eulerAngles;
            eulerAngle.z += zRotateValue * zRotateAmount;
            lookRotation = Quaternion.Euler(eulerAngle);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, lerpAmount);
        }
    }

    void ZAxisRotate()
    {
        currRotY = transform.eulerAngles.y;
        float diff = prevRotY - currRotY;

        if (diff > 180) diff -= 360;
        if (diff < -180) diff += 360;

        prevRotY = transform.eulerAngles.y;
        zRotateValue = Mathf.Lerp(zRotateValue, Mathf.Clamp(diff / zRotateMaxThreshold, -1, 1), turningForce * Time.deltaTime);
    }

    void Move()
    {
        transform.Translate(new Vector3(0, 0, speed) * Time.deltaTime);
    }


    void Start()
    {
        speed = targetSpeed = defaultSpeed;
        accelerateReciprocal = 1 / accelerateAmount;

        turningTime = 1 / turningForce;
        currentTurningTime = turningTime;
        waypointQueue = new Queue<Transform>();
        foreach (Transform t in initialWaypoints)
        {
            waypointQueue.Enqueue(t);
        }
        ChangeWaypoint();
    }

    void Update()
    {
        if (_player == null)
            return;
        Vector3 targetDirection = _player.transform.position - transform.position;
        if (targetDirection.magnitude <= 500)
        {
            if (_state != EnemyState.InBattle)
            {
                Debug.Log("전투모드 돌입!");
                _state = EnemyState.InBattle;
                ChangeWaypoint();
                return;
            }

        }
        else if (_state == EnemyState.InBattle)
        {
            Debug.Log("전투 -> 추적모드 !");
            _state = EnemyState.Tracking;
        }
        else
            _state = EnemyState.Patroll;
        CheckWaypoint();
            ZAxisRotate();
            Rotate();
            AdjustSpeed();
            Move();
        Debug.DrawRay(transform.position, currentWaypoint - transform.position, Color.red);
    }
}
