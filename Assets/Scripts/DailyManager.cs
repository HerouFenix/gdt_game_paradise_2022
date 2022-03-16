using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _possibleClients = new List<GameObject>();
    private List<GameObject> _usedClients = new List<GameObject>(); // List of clients that have already been used (to prevent using them again)
    private List<GameObject> _currentClients = new List<GameObject>();

    private int _nextClientIndex;
    private GameObject _currentClient;

    void Start()
    {
        /* 1st Pick 3 clients*/
        PickClients(1);

        /* Have client show up */
        _currentClient = Instantiate(_currentClients[_nextClientIndex]);        
        _nextClientIndex++;

    }

    void PickClients(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int clientIndex = UnityEngine.Random.Range(0, _possibleClients.Count);
            GameObject client = _possibleClients[clientIndex];

            while(_usedClients.Contains(client))
            { // Repeat until we get a client that is not yet been added to the used clients list
                clientIndex = UnityEngine.Random.Range(0, _possibleClients.Count);
                client = _possibleClients[clientIndex];
            }

            _currentClients.Add(client);
            _usedClients.Add(client);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
