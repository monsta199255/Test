using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform _bulletPoint;
    public Transform BulletPoint => _bulletPoint;
}
