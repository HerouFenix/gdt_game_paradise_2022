using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenStoreButton : MonoBehaviour
{
    private bool _hovered = false;
    public GameObject child;

    // Update is called once per frame
    void Update()
    {
        if (_hovered)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameManager.Instance._phase1Manager.StartDay();
            }

        }
    }

    private void OnMouseEnter()
    {
        _hovered = true;
        child.SetActive(true);
    }

    private void OnMouseExit()
    {
        _hovered = false;
        child.SetActive(false);
    }
}
