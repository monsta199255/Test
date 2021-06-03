using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputController : MonoBehaviour
{
    public event UnityAction Click;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Click?.Invoke();
        }
    }
}
