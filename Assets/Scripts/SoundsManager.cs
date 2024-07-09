using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    [SerializeField] AudioClip clickButton;
    [SerializeField] AudioClip congrats;
    private static AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onPictureClick += PlayButtonClick;
        GameEvents.current.onModeSelected += PlayButton;
        GameEvents.current.onBackButtonClick += PlayButton;
        GameEvents.current.onResetButtonClick += PlayButton;
        GameEvents.current.onPuzzleDone += Congrats;
        audioSource = GetComponent<AudioSource>();
    }
    private void PlayButtonClick(GameObject pice)
    {
        audioSource.PlayOneShot(clickButton);
    }
    public void PlayButton()
    {
        audioSource.PlayOneShot(clickButton);
    }

    public void Congrats()
    {
        audioSource.PlayOneShot(congrats);
    }
}
