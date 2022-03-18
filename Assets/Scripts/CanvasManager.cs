using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _buttonStartDay;
    [SerializeField] private GameObject _buttonGuess;
    [SerializeField] private GameObject _ClientCard;
    private Animator _clientCardAnimator;
    [SerializeField] private GameObject _Journal;
    [SerializeField] private GameObject _clientDiary;

    private Phase1Manager _phase1Manager;
    private GameManager _GameManager;

    void Start()
    {
        _GameManager = GameManager.Instance;
        _GameManager.StartPhase1 += CloseJournal;

        _phase1Manager = Phase1Manager.Instance;
        _phase1Manager.StartDayEvent += StartDay;
        _phase1Manager.Guessing += GuessingCard;
        _phase1Manager.Guess += GuessCard;

        _clientCardAnimator = _ClientCard.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseJournal(Client c)
    {
        _Journal.SetActive(false);
        _buttonStartDay.SetActive(true);
    }

    public void StartDay()
    {
        _buttonStartDay.SetActive(false);
        _buttonGuess.SetActive(true);
    }

    public void GuessingCard()
    {
        _buttonGuess.SetActive(false);
        _ClientCard.SetActive(true);
    }

    public void GuessCard()
    {
        _ClientCard.SetActive(false);
        _Journal.SetActive(true);
    }

    public void FlipClientCard360()
    {
        _clientCardAnimator.SetTrigger("flip360");
    }

    public void FlipClientCard180()
    {
        _clientCardAnimator.SetTrigger("flip180");
    }

    public void ResetClientCard()
    {
        _clientCardAnimator.SetTrigger("reset");
    }
}
