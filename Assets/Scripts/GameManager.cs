using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject dificultyButtons;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject settingsPannel;
    [SerializeField] GameObject puzzlePannel;
    void Start()
    {
        GameEvents.current.onPictureClick += ZoomToPuzzle;
        GameEvents.current.onBackButtonClick += GoToWall;
        GameEvents.current.onBackButtonClick += ClosePuzzlePanel;
        GameEvents.current.onModeSelected += HideDificultyButtons;
        GameEvents.current.onResetButtonClick += ClearSave;
    }

    private void ZoomToPuzzle(GameObject puzzle) {
        StartCoroutine(GoToPuzzle(puzzle));
    }

    private void GoToWall() {
        StartCoroutine(OutOfPuzzle());
    }

    private void HideDificultyButtons() {
        dificultyButtons.SetActive(false);
    }
    IEnumerator GoToPuzzle(GameObject puzzle)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Camera camera = Camera.main;
        LeanTween.move(camera.gameObject, new Vector3(puzzle.transform.position.x, puzzle.transform.position.y, 0), 1f).setEase(LeanTweenType.easeOutQuad);
        LeanTween.value(camera.gameObject, camera.orthographicSize, 1.5f, 1f).setOnUpdate((float flt) => {
            camera.orthographicSize = flt;
        });
        yield return new WaitForSeconds(1.5f);
        Cursor.lockState = CursorLockMode.None;
        if (!GameConfig.GetPuzzleConfig(puzzle.GetComponent<PuzzleManager>().id).done)
        {
            dificultyButtons.SetActive(true);
        }
        backButton.SetActive(true);
    }

    IEnumerator OutOfPuzzle()
    {
        Cursor.lockState = CursorLockMode.Locked;
        dificultyButtons.SetActive(false);
        backButton.SetActive(false);
        Camera camera = Camera.main;
        LeanTween.move(camera.gameObject, new Vector3(0, 0, 0), 1f).setEase(LeanTweenType.easeOutQuad);
        LeanTween.value(camera.gameObject, camera.orthographicSize, 5f, 1f).setOnUpdate((float flt) => {
            camera.orthographicSize = flt;
        });
        yield return new WaitForSeconds(1.5f);
        Cursor.lockState = CursorLockMode.None;
    }

    public void ClearSave()
    {
        GoToWall();
        PlayerPrefs.DeleteKey("donePictures");    
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenSettingsPanel()
    {
        settingsPannel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        settingsPannel.SetActive(false);
    }

    public void OpenPuzzlePanel()
    {
        puzzlePannel.SetActive(true);
    }

    public void ClosePuzzlePanel()
    {
        puzzlePannel.SetActive(false);
    }

    public void QuitPuzzle()
    {
        if (GameConfig.config[GameConfig.activePuzzle].done)
        {
            GoToWall();
            return;
        }
        OpenPuzzlePanel();
    }
}
