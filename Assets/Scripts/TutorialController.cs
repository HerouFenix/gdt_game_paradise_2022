using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public List<Sprite> tutorialScreens = new List<Sprite>();
    public Image imageRenderer;
    public GameObject menuScreen;
    public GameObject tutorialScreen;
    private int currentIndex = 0;

    private void OnEnable()
    {
        currentIndex = 0;
        imageRenderer.sprite = tutorialScreens[currentIndex];
    }

    public void GoToNext()
    {
        currentIndex++;
        if(currentIndex >= tutorialScreens.Count)
        {
            menuScreen.SetActive(true);
            tutorialScreen.SetActive(false);
        }
        imageRenderer.sprite = tutorialScreens[currentIndex];
    }
}
