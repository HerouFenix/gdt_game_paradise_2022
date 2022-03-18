using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private int _souls;
    private int _police;
    private int _day;


    [SerializeField] private List<GameObject> _possibleClients = new List<GameObject>();
    private List<GameObject> _currentClients = new List<GameObject>();

    private int _nextClientIndex;

    [HideInInspector] public Phase1Manager _phase1Manager;
    [HideInInspector] public Phase2Manager _phase2Manager;

    public event Action<GameObject> StartPhase1;
    public event Action EndPhase1;

    public event Action<List<int>> StartPhase2;
    public event Action EndPhase2;


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
        //Tunes
        _phase1Manager = this.GetComponent<Phase1Manager>();
        _phase2Manager = this.GetComponent<Phase2Manager>();


        /* 1st Pick 3 clients*/
        PickClients(1);

        /* Get Today's tool cards */
        _phase2Manager.todaysClients = _currentClients;
        _phase2Manager.ChooseCurrentToolCards(3);
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
                break;
            case 1:
                Debug.Log("Start phase 1");
                StartPhase1?.Invoke(_currentClients[_nextClientIndex]);
                _nextClientIndex++;
                break;
            case 2:
                Debug.Log("Start phase2");
                StartPhase2?.Invoke(_phase1Manager.ReturnResults());

                EndPhase1?.Invoke();
                break;
        } 
    }

    // Update is called once per frame
    void Update()
    {

    }
}
