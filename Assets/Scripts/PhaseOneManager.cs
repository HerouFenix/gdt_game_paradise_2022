using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PhaseOneManager : MonoBehaviour
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
    [HideInInspector] public ClientCard _clientCard;
    private GameManager _GameManager;

    //Actions
    public event Action StartDayEvent;
    public event Action GuessCard;



    public void Start()
    {
        _GameManager = GameManager.Instance;
        _GameManager.StartPhase1 += ReceiveClient;

        DrawReserveCards();
    }

    public void ReceiveClient(Client _dailyClient)
    {
        client = _dailyClient;
        //_clientCard = client.GetClientCard();
    }

    //Vai buscar as cartas de investiga��o di�rias, para a m�o e para a reserva
    public void DrawReserveCards()
    {
        for (int i = 17; i > 0; i--)
        {
            int cardIndex = UnityEngine.Random.Range(0, _deckCards.Count);
            //Debug.Log(_deckCards[cardIndex].number);
            //Debug.Log("Saiu carta de reserva" + _deckCards[cardIndex].number);

            _reserveCards.Add(_deckCards[cardIndex]);
            _deckCards.Remove(_deckCards[cardIndex]);
            //Debug.Log("Restam" + _deckCards.Count + "cartas");
        }
    }


    #region DrawCard

    public void StartDay()
    {
        StartDayEvent?.Invoke();
        StartCoroutine(DrawInvestigationCardCor(7));
    }

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

    public void PressGuessCard()
    {
        GuessCard?.Invoke();
        foreach (GameObject obj in this._currentInvestigationCards)
        {
            InvestigationCard card = obj.GetComponent<InvestigationCard>();
            card.HideCard();
        }
    }
}
