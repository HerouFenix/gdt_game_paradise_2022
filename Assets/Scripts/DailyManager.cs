using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyManager : MonoBehaviour
{
    private int _souls;
    private int _police;
    private int _day;


    [SerializeField] private List<GameObject> _possibleClients = new List<GameObject>();
    private List<GameObject> _usedClients = new List<GameObject>(); // List of clients that have already been used (to prevent using them again)
    private List<GameObject> _currentClients = new List<GameObject>();

    private int _nextClientIndex;
    private Client _currentClient;

    private PhaseOneManager _phaseOneManager;
    private PhaseTwoManager _phaseTwoManager;

    private int currentPhase = -1;

    void Start()
    {
        _phaseOneManager = this.GetComponent<PhaseOneManager>();
        _phaseTwoManager = this.GetComponent<PhaseTwoManager>();


        /* 1st Pick 3 clients*/
        PickClients(1);

        /* Have client show up */
        _currentClient = Instantiate(_currentClients[_nextClientIndex]).GetComponent<Client>();
        _currentClient.manager = this;
        _nextClientIndex++;

    }

    void PickClients(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int clientIndex = UnityEngine.Random.Range(0, _possibleClients.Count);
            GameObject client = _possibleClients[clientIndex];

            while (_usedClients.Contains(client))
            { // Repeat until we get a client that is not yet been added to the used clients list
                clientIndex = UnityEngine.Random.Range(0, _possibleClients.Count);
                client = _possibleClients[clientIndex];
            }

            _currentClients.Add(client);
            _usedClients.Add(client);
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
            case -1:

                break;
            case 0:
                this._phaseOneManager.client = _currentClient;
                this._phaseOneManager.phaseIndex = 0;
                break;
            case 1:
                this._phaseOneManager.phaseIndex = -1; // Deactivate Phase One Manager
                break;
        } 
    }

    // Update is called once per frame
    void Update()
    {

    }
}
