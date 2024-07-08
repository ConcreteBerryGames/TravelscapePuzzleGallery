using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleTrigger : MonoBehaviour, IPointerClickHandler
{
    void Start()
    {
        GameEvents.current.onBackButtonClick += EnableColider;
    }

    private void EnableColider()
    {
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        GameEvents.current.PictureClick(gameObject);
    }

}
