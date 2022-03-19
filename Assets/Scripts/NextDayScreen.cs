using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextDayScreen : MonoBehaviour
{
    public GameObject journal;

    private void OnEnable()
    {
        Animator animator = this.gameObject.GetComponent<Animator>();
        if(!animator.GetBool("first"))
            animator.SetTrigger("newDay");
    }

    void DisableScreen()
    {
        this.gameObject.SetActive(false);
    }

    void EnableJournal()
    {
        journal.SetActive(true);
    }
}
