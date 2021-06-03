using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField] bool _isNeedToJump;
    [SerializeField] bool _isFinish;

    public List<Enemy> EnemiesAtThisPoint;

    public bool IsNeedToJump => _isNeedToJump;
    public bool IsFinish => _isFinish;

    public EnemyPoint[] EnemyPoints => GetComponentsInChildren<EnemyPoint>();
}