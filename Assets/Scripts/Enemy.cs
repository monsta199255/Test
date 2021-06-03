using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBarTemp;
    [SerializeField] private Transform _healthBarPoint;

    private HealthBar _healthBar;
    private int _health = 100;

    public WayPoint MyWayPoint;
    public event UnityAction<Enemy> EnemyKilled;

    private void Start()
    {
        _healthBar = Instantiate(_healthBarTemp, _healthBarPoint.position, Quaternion.identity);
        _healthBar.SetMaxHealth(_health);
    }

    public void ApplyDamage(int damage)
    {
        _health -= damage;
        _healthBar.SetHealth(_health);
        if (_health <= 0)
        {
            EnemyKilled?.Invoke(this);
            _healthBar.gameObject.SetActive(false);
        }
    }
    public void SetHealth(int health)
    {
        _health = health;
        _healthBar.gameObject.SetActive(true);
        _healthBar.transform.position = _healthBarPoint.position;
        _healthBar.SetMaxHealth(health);
    }
}
