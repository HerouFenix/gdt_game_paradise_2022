using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _possibleClientCards = new List<GameObject>();
    [SerializeField] private List<GameObject> _possibleInteractionCards = new List<GameObject>();

    //Tunes
    [SerializeField] private List<InvestigationCard> _deckCards;
    private List<InvestigationCard> _handCards = new List<InvestigationCard>();
    private List<InvestigationCard> _reserveCards = new List<InvestigationCard>();

    private List<GameObject> _currentClientCards = new List<GameObject>();
    private List<GameObject> _currentInteractionCards = new List<GameObject>();

    [SerializeField] private Vector3 _spawnPosition = new Vector3(0,0,0);
    [SerializeField] public List<Vector3> cardPositions = new List<Vector3>();

    public void Start()
    {
        DrawDailyInvCards();
    }

    //Vai buscar as cartas de investigação diárias, para a mão e para a reserva
    public void DrawDailyInvCards()
    {
        for (int i = 7; i > 0; i--)
        {
            int cardIndex = Random.Range(0, _deckCards.Count);
            Debug.Log("Saiu carta de mão " + _deckCards[cardIndex].number);

            _handCards.Add(_deckCards[cardIndex]);
            _deckCards.Remove(_deckCards[cardIndex]);
            Debug.Log("Restam" + _deckCards.Count + "cartas");
        }

        for (int i = 10; i > 0; i--)
        {
            int cardIndex = Random.Range(0, _deckCards.Count);
            Debug.Log(_deckCards[cardIndex].number);
            Debug.Log("Saiu carta de reserva" + _deckCards[cardIndex].number);

            _reserveCards.Add(_deckCards[cardIndex]);
            _deckCards.Remove(_deckCards[cardIndex]);
            Debug.Log("Restam" + _deckCards.Count + "cartas");
        }
    }


    /* Client Card Methods */
    public void DrawClientCards(int amount)
    {
        StartCoroutine(DrawClientCardsCor(amount));
    }

    private IEnumerator DrawClientCardsCor(int amount)
    {
        WaitForSeconds wait = new WaitForSeconds(.85f);

        for (int i = 0; i < amount; i++)
        {
            DrawClientCard();
            yield return wait;
        }

    }

    public void DrawClientCard()
    {
        int randomCard = Random.Range(0, _handCards.Count);

        _possibleClientCards[0].GetComponent<IClientCard>()._invCard = _handCards[randomCard];

        GameObject newCard = Instantiate(_possibleClientCards[0], _spawnPosition, Quaternion.identity);

        IClientCard newCardScript = newCard.GetComponent<IClientCard>();

        newCardScript.SetIndex(_currentClientCards.Count, true);

        _currentClientCards.Add(newCard);
    }

    public IEnumerator RemoveClientCard(GameObject card)
    {
        int cardIndex = card.GetComponent<IClientCard>().GetIndex();
        this._currentClientCards.Remove(card);


        WaitForSeconds wait = new WaitForSeconds(.05f);
        for (int i = cardIndex; i < this._currentClientCards.Count; i++)
        {
            IClientCard clientCard = this._currentClientCards[i].GetComponent<IClientCard>();

            clientCard.SetIndex(i);
            clientCard.MoveCard(this.cardPositions[i]);
            yield return wait;
        }

        Destroy(card);
    }

    #region Singleton

    private static CardManager _instance;

    public static CardManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<CardManager>();
            return _instance;
        }
    }

    #endregion

    /* ************************ */
}
