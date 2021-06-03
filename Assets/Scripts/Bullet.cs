using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _lifetimeTemp;
    [SerializeField] private int _damage;

    private float _lifetime;
    private Rigidbody _rigidbody;

    public event UnityAction<Bullet> ReturnToPool;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _lifetime = _lifetimeTemp;
    }

    public void InitBullet(Vector3 pos, Vector3 target)
    {
        var direction = (target - pos).normalized;

        transform.position = pos;
        transform.rotation = Quaternion.identity;
        _lifetime = _lifetimeTemp;

        _rigidbody.AddForce(direction * _speed, ForceMode.Impulse);
    }

    private void Update()
    {
        float posY = transform.position.y;
        if (posY < 0)
        {
            _rigidbody.velocity = Vector3.zero;
            ReturnToPool?.Invoke(this);
        }
            

        _lifetime -= 1;
        if (_lifetime < 0)
        {
            _rigidbody.velocity = Vector3.zero;
            ReturnToPool?.Invoke(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.ApplyDamage(_damage);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (_rigidbody.useGravity == false)
        {
            ContactPoint contact = collision.contacts[0];
            _rigidbody.useGravity = true;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddExplosionForce(300, contact.point, 100);
        }
    }
}
