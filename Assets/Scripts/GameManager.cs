using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject dificultyButtons;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject settingsPannel;
    [SerializeField] GameObject puzzlePannel;
    [SerializeField] GameObject congratsPannel;
    [SerializeField] GameObject congratsImage;

    public static bool isZoomed = false;
    private GameObject[] pictures;
    private Color blinkColor = new Color32(200, 200, 200, 255); 
    void Start()
    {
        GameEvents.current.onPictureClick += ZoomToPuzzle;
        GameEvents.current.onBackButtonClick += GoToWall;
        GameEvents.current.onBackButtonClick += ClosePuzzlePanel;
        GameEvents.current.onModeSelected += HideDificultyButtons;
        GameEvents.current.onResetButtonClick += ClearSave;
        GameEvents.current.onPuzzleDone += ShowCongrats;
        pictures = GameObject.FindGameObjectsWithTag("Picture");
        foreach (GameObject picture in pictures)
        {
            if (!GameConfig.config[picture.GetComponent<PuzzleManager>().id].done) LeanTween.color(picture, blinkColor, 1f).setLoopPingPong();
        }
    }

    private void ZoomToPuzzle(GameObject puzzle) {
        StartCoroutine(GoToPuzzle(puzzle));
        isZoomed = true;
    }

    private void GoToWall() {
        StartCoroutine(OutOfPuzzle());
        isZoomed = false;
    }

    private void HideDificultyButtons() {
        dificultyButtons.SetActive(false);
    }
    IEnumerator GoToPuzzle(GameObject puzzle)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Camera camera = Camera.main;
        foreach (GameObject picture in pictures)
        {
            LeanTween.cancel(picture);
            LeanTween.color(picture, Color.white, 0.2f);
        }
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
        foreach (GameObject picture in pictures)
        {
            if (!GameConfig.config[picture.GetComponent<PuzzleManager>().id].done) LeanTween.color(picture, blinkColor, 1f).setLoopPingPong();
        }
        yield return new WaitForSeconds(1.1f);
        Cursor.lockState = CursorLockMode.None;
    }

    public void ClearSave()
    {
        PlayerPrefs.DeleteKey("donePictures");
        GoToWall();        
        CloseSettingsPanel();
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
        if (GameConfig.config[GameConfig.activePuzzle].done || dificultyButtons.activeSelf)
        {
            GoToWall();
            return;
        }
        OpenPuzzlePanel();
    }

    void ShowCongrats() {
        StartCoroutine(Congrats());
    }

    IEnumerator Congrats()
    {
        congratsPannel.SetActive(true);
        RectTransform image = congratsImage.GetComponent<UnityEngine.UI.Image>().rectTransform;
        LeanTween.alpha(image, 1f, .3f).setEase(LeanTweenType.easeInCirc);
        LeanTween.scale(image, Vector3.one*1.2f, .3f).setEase(LeanTweenType.easeInCirc);
        yield return new WaitForSeconds(0.3f);
        LeanTween.scale(image, Vector3.one, .3f).setEase(LeanTweenType.easeOutCirc);
        yield return new WaitForSeconds(1.3f);
        LeanTween.alpha(image, 0, 0.5f).setEase(LeanTweenType.easeInCirc);
        yield return new WaitForSeconds(0.6f);
        congratsPannel.SetActive(false);
    }
}
