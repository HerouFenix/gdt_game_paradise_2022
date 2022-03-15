using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCardInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            IClientCard card = hit.collider.gameObject.GetComponent<IClientCard>();

            // TODO: Must also check if its not the other type of cards
            if (card == null)
            {
                return;
            }

        }
    }
}
