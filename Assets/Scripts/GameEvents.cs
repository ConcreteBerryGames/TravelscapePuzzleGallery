using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action<GameObject> onPictureClick;
    public event Action onBackButtonClick;
    public event Action onResetButtonClick;
    public event Action onModeSelected;
    public event Action<GameObject> onGamePiceClick;
    public event Action onPuzzleDone;

    public void GamePiceClick(GameObject clickedPice)
    {
        if (onGamePiceClick != null)
        {
            onGamePiceClick(clickedPice);
        }
    }

    public void PictureClick(GameObject clickedPicture)
    {
        if (onPictureClick != null)
        {
            onPictureClick(clickedPicture);
        }
    }

    public void BackButtonClick()
    {
        if (onBackButtonClick != null)
        {
            onBackButtonClick();
        }
    }

    public void ResetButtonClick()
    {
        if (onResetButtonClick != null)
        {
            onResetButtonClick();
        }
    }

    public void ModeSelected()
    {
        if (onModeSelected != null)
        {
            onModeSelected();
        }
    }

    public void PuzzleDone()
    {
        if (onPuzzleDone != null)
        {
            onPuzzleDone();
        }
    }
}
