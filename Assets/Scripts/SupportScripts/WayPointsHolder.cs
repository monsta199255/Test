using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointsHolder : MonoBehaviour
{
    public WayPoint[] WayPointArray => GetComponentsInChildren<WayPoint>();
}
