using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextDayScreen : MonoBehaviour
{
    public GameObject journal;
    private AudioManager _audioManager;

    public Animator evil_lord;

    private void OnEnable()
    {
        _audioManager = AudioManager.Instance;
        Animator animator = this.gameObject.GetComponent<Animator>();
        if (!animator.GetBool("first"))
            animator.SetTrigger("newDay");
    }

    void DisableScreen()
    {
        _audioManager.PlaySound(AudioManager.soundList.Newspaper);
        this.gameObject.SetActive(false);
    }

    void EnableJournal()
    {
        journal.SetActive(true);
    }
}
