using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientDiary : MonoBehaviour
{
    // Used to animate the diary depending on results received //
    private Animator _animator;
    
    private void Start()
    {
        _animator = this.gameObject.GetComponent<Animator>(); 
    }
}
