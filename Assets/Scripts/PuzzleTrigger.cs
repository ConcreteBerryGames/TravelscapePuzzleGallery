using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTrigger : MonoBehaviour
{
    void Start()
    {
        GameEvents.current.onBackButtonClick += EnableColider;
    }

    private void EnableColider()
    {
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    void OnMouseDown()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        GameEvents.current.PictureClick(gameObject);
    }

}
