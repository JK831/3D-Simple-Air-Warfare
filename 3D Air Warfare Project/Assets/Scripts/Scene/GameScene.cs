using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    GameObject _player;
    GameObject _enemy;
    GameObject _topDownCamera;
    GameObject _targetUI;
    GameObject _minimapCanvas;
    GameObject _wayPointArea;
    GameObject _wayPoint;
    List<GameObject> terrainList = new List<GameObject>();


    void Awake()
    {

        _player = Managers.Resource.Instantiate("Player");
        _player.transform.position = new Vector3(0, 400, 0);

        CinemachineVirtualCamera cmV = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        cmV.Follow = _player.transform;
        cmV.LookAt = _player.transform;

        _enemy = Managers.Resource.Instantiate("Enemy");
        _enemy.transform.position = new Vector3(800, 400, 1800);

        _wayPointArea = Managers.Resource.Instantiate("WayPointArea");
        _wayPoint = Managers.Resource.Instantiate("WayPoint");

        EnemyController enemy = _enemy.GetComponent<EnemyController>();
        enemy._player = _player;
        enemy.areaCollider = _wayPointArea.GetComponent<BoxCollider>();
        enemy.waypointObject = _wayPoint;

        terrainList.Add(Managers.Resource.Instantiate("Terrains"));
        terrainList.Add(Managers.Resource.Instantiate("Terrain_01_low2"));

        _topDownCamera = Managers.Resource.Instantiate("Top-Down Camera");
        _topDownCamera.GetComponent<MinimapCamera>().target = _player.transform;

        enemy.transform.Find("New Sprite").GetComponent<MinimapSprite>().minimap_Camera = _topDownCamera.GetComponent<Camera>();
        enemy.transform.Find("New Sprite").GetComponent<MinimapSprite>().minimapCamera = _topDownCamera.GetComponent<MinimapCamera>();

        _minimapCanvas = Managers.Resource.Instantiate("MinimapCanvas");
        _topDownCamera.GetComponent<Camera>().targetTexture = _minimapCanvas.transform.Find("MinimapCamera").GetComponent<RawImage>().texture as RenderTexture;

        _player.GetComponent<WeaponController>().target = _enemy.transform;
        EnemyWeaponController ewc = _enemy.GetComponent<EnemyWeaponController>();
        ewc.target = _player.transform;
        ewc.gunTransform = ewc.gameObject.transform.Find("Aircraft_No2").Find("AirCraft02_Gun").Find("AirCraft02_GunRight");


        Managers.UI.ShowSceneUI<UI_Explanation>();
        _targetUI = Managers.UI.ShowPopupUI<TargetUI>().gameObject;
        ObjectPool.Init();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }
    }
}
