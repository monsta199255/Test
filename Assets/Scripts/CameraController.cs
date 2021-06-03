using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _camTarget;
    [SerializeField] private Transform _camBody;

    public EnemyPool EnemyPool;
    public CharController MainChar;
    public List<Enemy> EnemiesAtThisPoint;

    private void Start()
    {
        EnemyPool.EnemyKilled += OnEnemyKilled;
    }

    private void OnDisable()
    {
        EnemyPool.EnemyKilled -= OnEnemyKilled;
    }

    private void OnEnemyKilled(Enemy enemy)
    {
        EnemiesAtThisPoint.Remove(enemy);
        SetCamTargetPos(EnemiesAtThisPoint);
    }

    private void Update()
    {
        _camBody.position = MainChar.CamBody.position;
        _camBody.rotation = MainChar.CamBody.rotation;
        SetCamTargetPos(EnemiesAtThisPoint);
    }

    public void SetCamTargetPos(List<Enemy> enemies)
    {
        Vector3 calculatedVector;
        if (enemies.Count > 0)
        {
            int countOfEnemies = enemies.Count;
            calculatedVector = Vector3.zero;
            foreach (Enemy enemy in enemies)
            {
                Vector3 pos = enemy.transform.position;
                for (int i=0; i < 3; i++)
                {
                    calculatedVector[i] += pos[i] / countOfEnemies;
                }
            }
        }
        else
        {
            calculatedVector = _camBody.position;
        }
        _camTarget.position = calculatedVector;
    }
}
