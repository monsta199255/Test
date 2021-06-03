using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class CharController : MonoBehaviour
{
    [SerializeField] private Transform _rightHand;
    [SerializeField] private float _speedScalar;
    [SerializeField] private float _jumpForceScalar;
    [SerializeField] private Transform _camBody;
    [SerializeField] private string _nameCharIdleString;

    private WayPoint _targetWayPoint;
    private WayPoint _wayPointLookAt;
    private Rigidbody _rigidbody;
    private bool isLandedAtStart = false;

    public event UnityAction CharReady;
    public event UnityAction PointAchieved;
    public event UnityAction Finish;

    public Transform RightHand => _rightHand;
    public Transform CamBody => _camBody;
    public string NameCharIdleString => _nameCharIdleString;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void MoveToPoint(WayPoint wayPoint, WayPoint wayPointLookAt)
    {
        _targetWayPoint = wayPoint;
        _wayPointLookAt = wayPointLookAt;

        float dist = Vector3.Distance(transform.position, wayPoint.transform.position);

        Vector3 direction = (_wayPointLookAt.transform.position - transform.position).normalized;
        transform.DORotate(Quaternion.LookRotation(direction).eulerAngles, 1);
        if (wayPoint.IsNeedToJump)
        {
            transform.DOJump(wayPoint.transform.position, dist * _jumpForceScalar, 1, dist * _speedScalar).SetEase(Ease.Linear).OnComplete(WentNewPoint);
        }
        else
        {
            transform.DOMove(wayPoint.transform.position, dist * _speedScalar).SetEase(Ease.Linear).OnComplete(WentNewPoint);
        }
        
    }

    private void WentNewPoint()
    {
        if(!_targetWayPoint.IsFinish)
        {
            Vector3 direction = (_wayPointLookAt.transform.position - transform.position).normalized;
            transform.DORotate(Quaternion.LookRotation(direction).eulerAngles, 1);
            PointAchieved?.Invoke();
        }
        else
        {
            Finish?.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isLandedAtStart)
        {
            CharReady?.Invoke();
            isLandedAtStart = true;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
        }
    }

    public void RestartLevel(WayPoint wayPoint)
    {
        transform.position = wayPoint.transform.position;
        transform.rotation = wayPoint.transform.rotation;
        isLandedAtStart = false;
        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = false;
    }
}
