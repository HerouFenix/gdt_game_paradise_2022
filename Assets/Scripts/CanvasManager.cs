using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _buttonStartDay;
    [SerializeField] private GameObject _ClientCard;
    private Animator _clientCardAnimator;
    [SerializeField] private GameObject _Journal;

    [SerializeField] private GameObject _nextDayScreen;
    [SerializeField] private TextMeshPro _soulCounter;
    [SerializeField] private TextMeshPro _dayCounter;

    [SerializeField] private GameObject _WinScreen;
    [SerializeField] private GameObject _LoseScreenSouls;
    [SerializeField] private GameObject _LoseScreenCops;


    private Phase1Manager _phase1Manager;
    private GameManager _GameManager;
    private AudioManager _audioManager;


    void Start()
    {
        _GameManager = GameManager.Instance;
        _audioManager = AudioManager.Instance;
        _GameManager.StartPhase0 += RemoveCurrentClient;
        _GameManager.StartPhase0 += ResetClientCard;
        _GameManager.FinishGame += RevealEndScreen;

        _GameManager.NewDay += ShowNewDayScreen;

        _phase1Manager = Phase1Manager.Instance;
        _phase1Manager.StartDayEvent += StartDay;
        _phase1Manager.PlayInvCard += FlipClientCard360;

        _clientCardAnimator = _ClientCard.GetComponent<Animator>();

        _GameManager.StartPhase2 += RevealResultsOnClientCard;
    }

    public void ShowNewDayScreen(int daysLeft, int soulsLeft)
    {
        _audioManager.MusicStop();
        _nextDayScreen.gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = daysLeft + " DAYS UNTIL RECKONING";
        _nextDayScreen.gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = soulsLeft + " SOULS REQUIRE HARVESTING";
        _nextDayScreen.SetActive(true);
        _nextDayScreen.GetComponent<Animator>().SetBool("first", false);
    }

    public void RemoveCurrentClient()
    {
        StartCoroutine(GameObject.FindGameObjectWithTag("client").GetComponent<Client>().FadeAway());
        _audioManager.PlaySound(AudioManager.soundList.DoorClose);
        _ClientCard.SetActive(false);
    }
    
    public void CloseJournal()
    {
        _Journal.SetActive(false);
        //_buttonStartDay.SetActive(true);
        _audioManager.ChangeMusic(UnityEngine.Random.Range(0,3));
        StartCoroutine(_GameManager.WaitForNextClient());
    }

    public void StartDay()
    {
        _buttonStartDay.SetActive(false);
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
        _ClientCard.GetComponent<ClientCard>().ResetClientCard();
        _clientCardAnimator.SetTrigger("reset");
    }

    public void RevealEndScreen(int type)
    {
        if(type == 0)
        { // Win
            _WinScreen.SetActive(true);
        }else if(type == 1)
        { // Lose to cops
            _LoseScreenCops.SetActive(true);
        }
        else
        { // Lose to souls
            _LoseScreenSouls.SetActive(true);
        }
    }
}
