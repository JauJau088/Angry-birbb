using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apoint : MonoBehaviour
{
    private Vector2 _initialPosition;

    public void pointInitPos (Vector2 value) {
        _initialPosition = value;

        transform.position = _initialPosition;
    }
}
