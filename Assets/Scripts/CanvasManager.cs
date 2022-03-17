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
    private Phase1Manager _phase1Manager;

    void Start()
    {
        Phase1Manager _phase1Manager = Phase1Manager.Instance;
        _phase1Manager.StartDay += StartDay;
        _phase1Manager.GuessCard += GuessCard;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDay()
    {
        _buttonStartDay.SetActive(false);
        _buttonGuess.SetActive(true);
    }

    public void GuessCard()
    {
        _buttonGuess.SetActive(false);
        _ClientCard.SetActive(true);
    }
}
