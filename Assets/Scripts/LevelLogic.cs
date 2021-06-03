using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLogic : MonoBehaviour
{
    [SerializeField] private InputController _inputController;
    [SerializeField] private WayPointsHolder _wayPointsHolder;
    [SerializeField] private CharController _charControllerTemp;
    [SerializeField] private Weapon _weaponTemp;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private BulletsPool _bulletsPool;
    [SerializeField] private EnemyPool _enemyPool;

    private CharController _charController;
    private Animator _charAnimator;
    private string _idleCharName;
    private Weapon _weapon;
    private WayPoint _nextWayPoint;
    private WayPoint _nextWayPointLookAt;
    private WayPoint _activeWayPoint;
    private bool _isMoving;
    private int _activePointIndex = 0;

    private void Start()
    {
        _activeWayPoint = _wayPointsHolder.WayPointArray[_activePointIndex];
        _nextWayPoint = _wayPointsHolder.WayPointArray[_activePointIndex+1];
        _nextWayPointLookAt = _wayPointsHolder.WayPointArray[_activePointIndex + 2];
        _activePointIndex ++;
        
        _charController = Instantiate(_charControllerTemp, _activeWayPoint.transform.position, Quaternion.identity);
        _charController.PointAchieved += OnPointAchieved;
        _charController.Finish += RestartLevel;
        _charController.CharReady += OnCharReady;
        _charAnimator = _charController.GetComponent<Animator>();
        _idleCharName = _charController.NameCharIdleString;

        _cameraController.MainChar = _charController;
        _cameraController.EnemyPool = _enemyPool;
        _cameraController.EnemiesAtThisPoint.AddRange(_activeWayPoint.EnemiesAtThisPoint);
        _cameraController.enabled = true;

        _weapon = Instantiate(_weaponTemp, _charController.RightHand.position, _charController.RightHand.rotation, _charController.RightHand);
    }

    private void OnCharReady()
    {
        _isMoving = false;
    }

    private void RestartLevel()
    {
        _activePointIndex = 0;
        _activeWayPoint = _wayPointsHolder.WayPointArray[_activePointIndex];
        _nextWayPoint = _wayPointsHolder.WayPointArray[_activePointIndex + 1];
        _nextWayPointLookAt = _wayPointsHolder.WayPointArray[_activePointIndex + 2];
        _enemyPool.RestartLevel();
        _charController.RestartLevel(_activeWayPoint);
        _charAnimator.SetBool(_idleCharName, true);
    }

    private void OnEnable()
    {
        _inputController.Click += OnClick;
    }

    private void OnDisable()
    {
        _inputController.Click -= OnClick;
        _charController.PointAchieved -= OnPointAchieved;
    }

    private void OnPointAchieved()
    {
        _activeWayPoint = _wayPointsHolder.WayPointArray[_activePointIndex];
        _nextWayPoint = _wayPointsHolder.WayPointArray[_activePointIndex + 1];
        _nextWayPointLookAt = _wayPointsHolder.WayPointArray[_activePointIndex + 2];
        _activePointIndex++;

        _cameraController.EnemiesAtThisPoint.AddRange(_activeWayPoint.EnemiesAtThisPoint);

        if(_activeWayPoint.EnemyPoints.Length > 0)
        {
            _isMoving = false;
            _charAnimator.SetBool(_idleCharName, true);
        }
        else
        {
            _charController.MoveToPoint(_nextWayPoint, _nextWayPointLookAt);
        }
    }

    private void OnClick()
    {
        if (_activeWayPoint.EnemiesAtThisPoint.Count == 0 && !_isMoving)
        {
            _charController.MoveToPoint(_nextWayPoint, _nextWayPointLookAt);
            _isMoving = true;
            _charAnimator.SetBool(_idleCharName, false);
        }
        else
        {
            Vector3 target;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log(hit.transform.gameObject);
                target = hit.point;
            }
            else
            {
                target = ray.GetPoint(10);
            }
            _bulletsPool.GetFromPool(_weapon.BulletPoint.transform.position, target);
        }
    }
}
