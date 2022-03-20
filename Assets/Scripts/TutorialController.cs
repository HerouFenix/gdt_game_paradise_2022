using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public List<Sprite> tutorialScreens = new List<Sprite>();
    public SpriteRenderer renderer;
    private int currentIndex = 0;
    private void OnEnable()
    {
        currentIndex = 0;
        renderer.sprite = tutorialScreens[currentIndex];
    }

    public void GoToNext()
    {
        currentIndex++;
        renderer.sprite = tutorialScreens[currentIndex];
    }
}
