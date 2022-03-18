using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Phase1Manager : MonoBehaviour
{
    [SerializeField] private GameObject _baseInvestigationCard;

    //Tunes
    [SerializeField] private List<InvestigationCardProperties> _deckCards;
    private List<InvestigationCardProperties> _handCards = new List<InvestigationCardProperties>();
    private List<InvestigationCardProperties> _reserveCards = new List<InvestigationCardProperties>();
    private List<InvestigationCardProperties> _playedCards = new List<InvestigationCardProperties>();
    [SerializeField] private CardEffects _cardEffects;

    private List<GameObject> _currentInvestigationCards = new List<GameObject>();

    [SerializeField] private Vector3 _spawnPosition = new Vector3(0, 0, 0);
    [SerializeField] public List<Vector3> cardPositions = new List<Vector3>();

    //TempFix
    //[SerializeField] public ClientCard clientCard;
    private Client client;
    public ClientCard _clientCard;
    private GameManager _GameManager;
    List<int> _currentResultsList;


    //Actions
    public event Action StartDayEvent;
    public event Action PlayInvCard;
    public event Action Guess;

    private bool _cardsDrawn = false;


    #region Singleton

    private static Phase1Manager _instance;
    public static Phase1Manager Instance { get { return _instance; } }
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

    public void Start()
    {
        _GameManager = GameManager.Instance;
        _GameManager.StartPhase1 += ReceiveClient;
        //_GameManager.EndPhase1 += ReturnResults;


        DrawReserveCards();
    }

    public void StartDay()
    {
        StartDayEvent?.Invoke();
        _GameManager.SwapPhase(1);
    }

    public void DrawCards()
    {
        StartCoroutine(DrawInvestigationCardCor(7));
    }

    public void ReceiveClient(GameObject _newClient)
    {
        client = Instantiate(_newClient, new Vector3(3f, 0.12f, -0.1f), Quaternion.identity).GetComponent<Client>();
        client.manager = this.gameObject.GetComponent<GameManager>();
        client.manager1 = this;
        client.manager2 = this.gameObject.GetComponent<Phase2Manager>();

        _clientCard.ResetClientCard();
        _clientCard.gameObject.SetActive(true);
    }

    //Vai buscar as cartas de investigação diárias, para a mão e para a reserva
    public void DrawReserveCards()
    {
        for (int i = 17; i > 0; i--)
        {
            int cardIndex = UnityEngine.Random.Range(0, _deckCards.Count);

            _reserveCards.Add(_deckCards[cardIndex]);
            _deckCards.Remove(_deckCards[cardIndex]);
        }
    }


    #region DrawCard

    private IEnumerator DrawInvestigationCardCor(int amount)
    {
        WaitForSeconds wait = new WaitForSeconds(.85f);

        if (amount > _reserveCards.Count)
        {
            amount = _reserveCards.Count;
        }

        for (int i = 0; i < amount; i++)
        {
            DrawInvestigationCard();
            yield return wait;
        }

        client._canGuess = true;
    }

    public void DrawInvestigationCard()
    {
        int randomCard = UnityEngine.Random.Range(0, _reserveCards.Count);

        _baseInvestigationCard.GetComponent<InvestigationCard>()._invCard = _reserveCards[randomCard];

        GameObject newCard = Instantiate(_baseInvestigationCard, _spawnPosition, Quaternion.identity);

        _handCards.Add(newCard.GetComponent<InvestigationCard>()._invCard);
        _reserveCards.Remove(newCard.GetComponent<InvestigationCard>()._invCard);

        InvestigationCard newCardScript = newCard.GetComponent<InvestigationCard>();

        newCardScript.SetIndex(_currentInvestigationCards.Count, true);
        newCardScript.Played += RemoveInvestigationCard;

        _currentInvestigationCards.Add(newCard);
    }

    #endregion

    #region RemoveCard

    public void RemoveInvestigationCard(GameObject card)
    {
        InvestigationCardProperties _card = card.GetComponent<InvestigationCard>()._invCard;
        ApplyEffect(_card);
        _playedCards.Add(_card);
        _handCards.Remove(_card);
        StartCoroutine(RemoveInvestigationCardCor(card));

        StartCoroutine(DrawInvestigationCardCor(1));

    }

    public IEnumerator RemoveInvestigationCardCor(GameObject card)
    {
        int cardIndex = card.GetComponent<InvestigationCard>().GetIndex();
        this._currentInvestigationCards.Remove(card);


        WaitForSeconds wait = new WaitForSeconds(.05f);
        for (int i = cardIndex; i < this._currentInvestigationCards.Count; i++)
        {
            InvestigationCard clientCard = this._currentInvestigationCards[i].GetComponent<InvestigationCard>();

            clientCard.SetIndex(i);
            clientCard.MoveCard(this.cardPositions[i]);
            yield return wait;
        }

        Destroy(card);
    }

    #endregion

    public void ApplyEffect(InvestigationCardProperties card)
    {
        InvestigationCardProperties.Type type = card.type;
        PlayInvCard?.Invoke();

        switch (type)
        {
            case InvestigationCardProperties.Type.Number:
                _cardEffects.NumberCardEffect(card, _clientCard);
                break;

            case InvestigationCardProperties.Type.Even:
                _cardEffects.EvenCardEffect(_clientCard);
                break;

            case InvestigationCardProperties.Type.DoubleD:
                _cardEffects.DoubleDCardEffect(_clientCard);
                break;

            case InvestigationCardProperties.Type.Suit:
                _cardEffects.SuitCardEffect(card, _clientCard);
                break;

            case InvestigationCardProperties.Type.Color:
                _cardEffects.ColorCardEffect(card, _clientCard);
                break;
        }
    }

    public IEnumerator HideCards()
    {
        WaitForSeconds wait = new WaitForSeconds(.08f);

        foreach (GameObject obj in this._currentInvestigationCards)
        {
            InvestigationCard card = obj.GetComponent<InvestigationCard>();
            card.HideCard();

            yield return wait;
        }
    }

    public IEnumerator ShowCards()
    {
        WaitForSeconds wait = new WaitForSeconds(.08f);

        foreach (GameObject obj in this._currentInvestigationCards)
        {
            InvestigationCard card = obj.GetComponent<InvestigationCard>();
            card.ShowCard();

            yield return wait;
        }
    }

    public void GuessCard()
    {
        StartCoroutine(HideCards());

        Guess?.Invoke();
        _currentResultsList = _clientCard.RevealAll();

        _GameManager.SwapPhase(2);

        //Debug.Log("cor " + _resultsList[0]);
        //Debug.Log("naipe " + _resultsList[1]);
        //Debug.Log("nº " + _resultsList[2]);
    }

    public List<int> ReturnResults()
    {
        client._canGuess = false;
        client.ToggleTextBubble();
        return _currentResultsList;
    }
}
