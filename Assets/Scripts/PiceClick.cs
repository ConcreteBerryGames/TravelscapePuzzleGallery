using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiceClick : MonoBehaviour
{
    void OnMouseDown()
    {
        GameEvents.current.GamePiceClick(gameObject);
    }
}
