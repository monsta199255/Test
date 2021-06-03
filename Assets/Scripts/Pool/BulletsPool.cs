using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsPool : MonoBehaviour
{
    [SerializeField] private Bullet _bulletTemp;
    [SerializeField] private int _bulletCount;

    private List<Bullet> _pool = new List<Bullet>();

    private void Awake()
    {
        for (int i = 0; i < _bulletCount; i++)
        {
            var bullet = Instantiate(_bulletTemp, Vector3.zero, Quaternion.identity);
            bullet.gameObject.SetActive(false);
            bullet.ReturnToPool += OnReturnToPool;
            _pool.Add(bullet);
        }
    }

    public void GetFromPool(Vector3 pos, Vector3 target)
    {
        if(_pool.Count != 0)
        {
            Bullet newBullet = _pool[_pool.Count - 1];
            _pool.RemoveAt(_pool.Count - 1);
            newBullet.gameObject.SetActive(true);
            newBullet.InitBullet(pos, target);
        }

    }

    public void OnReturnToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        _pool.Add(bullet);
    }

    private void OnDisable()
    {
        foreach(Bullet bullet in _pool)
        {
            bullet.ReturnToPool -= OnReturnToPool;
        }
    }

}
