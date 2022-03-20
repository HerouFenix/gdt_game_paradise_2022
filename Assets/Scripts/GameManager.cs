using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private int _souls;
    private int _police;
    private int _day;

    public int numberOfDays = 3;
    public int requiredSouls = 0;
    public int maxPoliceLevel = 10;



    [SerializeField] private List<GameObject> _possibleClients = new List<GameObject>();
    private List<GameObject> _currentClients = new List<GameObject>();

    private int _nextClientIndex;

    [HideInInspector] public Phase1Manager _phase1Manager;
    [HideInInspector] public Phase2Manager _phase2Manager;

    public event Action<int, int>  NewDay;
    public event Action EndDay;

    public event Action<int> FinishGame;

    public event Action StartPhase0;

    public event Action<GameObject> StartPhase1;
    public event Action EndPhase1;

    public event Action<List<int>> StartPhase2;
    public event Action EndPhase2;

    private JournalManager _journalManager;
    private AudioManager _audioManager;


    private int currentPhase = 0;

    #region Singleton

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    void Start()
    {
        _phase1Manager = this.GetComponent<Phase1Manager>();
        _phase2Manager = this.GetComponent<Phase2Manager>();

        _audioManager = AudioManager.Instance;

        _souls = 0;
        _police = 0;
        _day = 0;

        StartNewDay();
    }

    public void SetJournalManager()
    {
        _journalManager = JournalManager.Instance;
    }

    void StartNewDay()
    {
        _day++;
        /* Check win/lose conditions */
        if(_day > numberOfDays)
        {
            if(_souls >= requiredSouls)
            {
                Debug.Log("You win :)");
                FinishGame?.Invoke(0);
            }
            else
            {
                Debug.Log("You lose :(");
                FinishGame?.Invoke(2);
            }
        }

        if (_police > maxPoliceLevel)
        {
            Debug.Log("You lose :( (police)");
            FinishGame?.Invoke(1);
        }
        else
        {
            // Show start day screen
            NewDay?.Invoke(numberOfDays-_day, requiredSouls-_souls);
        }

        

        /* 1st Pick 3 clients*/
        PickClients(3);

        /* Get Today's tool cards */
        _phase2Manager.todaysClients = _currentClients;
        _phase2Manager.ChooseCurrentToolCards(6);
    }

    void PickClients(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int clientIndex = UnityEngine.Random.Range(0, _possibleClients.Count);
            GameObject client = _possibleClients[clientIndex];

            _currentClients.Add(client);
            _possibleClients.RemoveAt(clientIndex);
        }
    }

    public void IncrementValues(int souls, int police)
    {
        int clientID;
        bool killed = _souls > 0;

        _souls += souls;
        _police += police;

        Debug.Log("ID proximo cliente: " + _nextClientIndex);
        clientID =  _currentClients[_nextClientIndex].GetComponent<Client>().ClientID;


        _journalManager.ReceivedClientNews(clientID, killed, police);
    }

    public int GetCurrentPoliceValue()
    {
        return _police;
    }

    public int GetPhase()
    {
        return this.currentPhase;
    }

    public void SwapPhase(int nextPhase)
    {
        this.currentPhase = nextPhase;
        switch (this.currentPhase)
        {
            case 0:
                EndPhase2?.Invoke();

                StartPhase0?.Invoke();
                _nextClientIndex++;

                if(_nextClientIndex >= _currentClients.Count)
                { // No more clients ; End Day
                    EndDay?.Invoke();
                    StartCoroutine(WaitForNewDay());
                }
                else
                { // There are more clients ; Continue
                    StartCoroutine(WaitForNextClient());
                }
                
                break;
            
            case 1:
                StartPhase1?.Invoke(_currentClients[_nextClientIndex]);
                
                break;
            
            case 2:
                EndPhase1?.Invoke();
                
                StartPhase2?.Invoke(_phase1Manager.ReturnResults());
                _phase2Manager.client = _currentClients[_nextClientIndex].GetComponent<Client>();
                
                break;
        } 
    }

    public IEnumerator WaitForNextClient()
    {
        Debug.Log("Waiting for the next client");
        yield return new WaitForSeconds(UnityEngine.Random.Range(4, 6));
        _audioManager.PlaySound(AudioManager.soundList.DoorOpen);
        this.SwapPhase(1);
    }

    public IEnumerator WaitForNewDay()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(4, 6));
        StartNewDay();
    }
}
