using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _possibleClientCards = new List<GameObject>();
    [SerializeField] private List<GameObject> _possibleInteractionCards = new List<GameObject>();

    private List<GameObject> _currentClientCards = new List<GameObject>();
    private List<GameObject> _currentInteractionCards = new List<GameObject>();

    [SerializeField] private Vector3 _spawnPosition = new Vector3(0,0,0);
    [SerializeField] public List<Vector3> cardPositions = new List<Vector3>();


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
        int randomCard = Random.Range(0, _possibleClientCards.Count);

        GameObject newCard = Instantiate(_possibleClientCards[randomCard], _spawnPosition, Quaternion.identity);

        IClientCard newCardScript = newCard.GetComponent<IClientCard>();

        newCardScript.SetIndex(_currentClientCards.Count, true);
        newCardScript.cardManager = this;

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

    /* ************************ */
}
