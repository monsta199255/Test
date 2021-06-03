using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private Enemy _enemyTemp;
    [SerializeField] private WayPointsHolder _wayPointsHolder;

    public List<Enemy> _pool = new List<Enemy>();

    public event UnityAction<Enemy> EnemyKilled;

    private void Awake()
    {
        foreach (WayPoint wayPoint in _wayPointsHolder.WayPointArray)
        {
            List<Enemy> enemies = new List<Enemy>();
            foreach (EnemyPoint enemyPoint in wayPoint.EnemyPoints)
            {
                var enemy = Instantiate(_enemyTemp, enemyPoint.transform.position, enemyPoint.transform.rotation);
                enemy.MyWayPoint = wayPoint;
                enemy.EnemyKilled += OnReturnToPool;
                enemies.Add(enemy);
            }
            wayPoint.EnemiesAtThisPoint.AddRange(enemies);
        }
    }

    public Enemy GetFromPool(Vector3 pos, Quaternion rot)
    {
        Enemy enemy = _pool[_pool.Count - 1];
        _pool.RemoveAt(_pool.Count - 1);
        enemy.transform.position = pos; // REGDOLL
        enemy.transform.rotation = rot;
        enemy.SetHealth(100);
        enemy.gameObject.SetActive(true);
        return enemy;
    }

    public void OnReturnToPool(Enemy enemy)
    {
        Debug.Log("work");
        enemy.MyWayPoint.EnemiesAtThisPoint.Remove(enemy);
        enemy.gameObject.SetActive(false);
        EnemyKilled?.Invoke(enemy);
        _pool.Add(enemy);
    }

    public void RestartLevel()
    {
        foreach (WayPoint wayPoint in _wayPointsHolder.WayPointArray)
        {
            List<Enemy> enemies = new List<Enemy>();
            foreach (EnemyPoint enemyPoint in wayPoint.EnemyPoints)
            {
                var enemy = GetFromPool(enemyPoint.transform.position, enemyPoint.transform.rotation);
                enemy.MyWayPoint = wayPoint;
                enemies.Add(enemy);
            }
            wayPoint.EnemiesAtThisPoint.Clear();
            wayPoint.EnemiesAtThisPoint.AddRange(enemies);
        }
    }

    // REGDOLL
    private void OnDisable()
    {
        foreach (Enemy enemy in _pool)
        {
            enemy.EnemyKilled -= OnReturnToPool;
        }
    }

}
