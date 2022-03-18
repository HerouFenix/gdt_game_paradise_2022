using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _buttonStartDay;
    [SerializeField] private GameObject _ClientCard;
    private Animator _clientCardAnimator;
    [SerializeField] private GameObject _Journal;

    private Phase1Manager _phase1Manager;
    private GameManager _GameManager;


    void Start()
    {
        _GameManager = GameManager.Instance;
        _GameManager.StartPhase0 += RemoveCurrentClient;

        _phase1Manager = Phase1Manager.Instance;
        _phase1Manager.StartDayEvent += StartDay;
        _phase1Manager.PlayInvCard += FlipClientCard360;

        _clientCardAnimator = _ClientCard.GetComponent<Animator>();

        _GameManager.StartPhase2 += RevealResultsOnClientCard;
    }

    public void RemoveCurrentClient()
    {
        StartCoroutine(GameObject.FindGameObjectWithTag("client").GetComponent<Client>().FadeAway());
        _ClientCard.SetActive(false);
    }
    
    public void CloseJournal()
    {
        _Journal.SetActive(false);
        _buttonStartDay.SetActive(true);
    }

    public void StartDay()
    {
        _buttonStartDay.SetActive(false);
        _ClientCard.SetActive(true);
    }


    public void RevealResultsOnClientCard(List<int> l)
    {
        ClientCard card = _ClientCard.GetComponent<ClientCard>();
        Client client = GameObject.FindGameObjectWithTag("client").GetComponent<Client>();

        List<string> hints = new List<string>(client._hints);
        int police = client._police_value;
        int soul = client._soul_value;

        if (l[2] > 8)
        { // Pretty right, remove 1 hint
            hints.RemoveAt(2);
            hints.RemoveAt(1);
            hints.RemoveAt(0);
        }else if(l[2] > 5)
        { // Kind of right, remove 2 hints
            hints.RemoveAt(2);
            hints.RemoveAt(1);
        }else if(l[2] > 0)
        { // Entirely wrong, remove all hints
            hints.RemoveAt(2);
        }

        if(l[1] == 0)
        { // Wrong Suit
            police = -1;
        }

        if(l[0] == 0)
        { // Wrong
            soul = -1;
        }

        card.RevealResults(hints, police, soul);
        FlipClientCard180();
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
