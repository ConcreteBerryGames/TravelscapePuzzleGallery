using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleTrigger : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!GameManager.isZoomed)
        {
            GameEvents.current.PictureClick(gameObject);
        }
    }
}
