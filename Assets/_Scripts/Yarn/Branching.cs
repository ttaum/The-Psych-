using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branching : MonoBehaviour
{
    [SerializeField]
    private GameObject input;
   // public PlayerInputHandler InputHandler { get; private set; }

    [SerializeField]
    private EdgeCollider2D colUp;

    [SerializeField]
    private EdgeCollider2D colDown;

    private void Start()
    {
   //     InputHandler = input.GetComponent<PlayerInputHandler>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
      //      if (InputHandler.BranchInput == 1)
            {
                colDown.enabled = false;
                colUp.enabled = true;
            }
      //      else if (InputHandler.BranchInput == -1)
            {
                colUp.enabled = false;
                colDown.enabled = true;
            }
        }
    }
}
