using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ToolCards : MonoBehaviour
{
    [SerializeField] public int ClientID = 0;
    [SerializeField] public int probabilityOfKilling = 0;


    [SerializeField] public GameObject _front;
    [SerializeField] private Sprite _image;

    [HideInInspector] protected int positionIndex;
    private Phase2Manager _phase2Manager;

    private Animator _animator;
    public bool _interactible = false;
    private bool _hovered = false;

    public event Action<GameObject> Played;

    private void Start()
    {
        _animator = this.gameObject.GetComponent<Animator>();
        _phase2Manager = Phase2Manager.Instance;
        _front.GetComponent<SpriteRenderer>().sprite = _image;
    }

    private void Update()
    {
        if (_interactible && _hovered)
        {
            if (Input.GetMouseButtonDown(0))
                SelectCard();
        }
    }

    public void SetIndex(int index, bool setAnimator=false)
    {
        if (_animator == null && setAnimator)
            _animator = this.gameObject.GetComponent<Animator>();

        positionIndex = index;

        if (_animator != null && setAnimator)
            _animator.SetInteger("position_index", positionIndex);
    }

    public int GetIndex()
    {
        return positionIndex;
    }

    public void MoveCard(Vector3 position)
    {
        StartCoroutine(LerpPosition(position, .1f, true));
    }

    public void HideCard()
    {
        StartCoroutine(Slide(new Vector3(this.transform.position.x, -10f, -0.5f), .4f));
    }

    public void ShowCard()
    {
        StartCoroutine(Slide(new Vector3(this.transform.position.x, -2.85f, this._phase2Manager.cardPositions[this.positionIndex].z), .4f));
    }


    public void SetPosition()
    {
        _animator.enabled = false;
        _interactible = true;
    }


    private void OnMouseEnter()
    {
        if (_interactible)
        {
            StartCoroutine(LerpPosition(new Vector3(this.transform.position.x, -2.0f, -0.5f), .1f));
            _hovered = true;

        }
    }

    private void OnMouseOver()
    {
        if (_interactible && !_hovered)
        {
            StartCoroutine(LerpPosition(new Vector3(this.transform.position.x, -2.0f, -0.5f), .1f));
            _hovered = true;

        }
    }

    private void OnMouseExit()
    {
        if (_interactible)
        {
            StartCoroutine(LerpPosition(new Vector3(this.transform.position.x, -2.85f, this._phase2Manager.cardPositions[this.positionIndex].z), .1f));
            _hovered = false;
        }
    }

    private void SelectCard()
    {

        StartCoroutine(LerpPositionOffscreen(new Vector3(this.transform.position.x, -6f, -0.5f), .2f));
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration, bool lockInteraction=false)
    {
        if (lockInteraction)
            _interactible = false;

        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        if (lockInteraction)
            _interactible = true;
    }

    IEnumerator LerpPositionOffscreen(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        Played?.Invoke(this.gameObject);

    }

    IEnumerator Slide(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }

}
