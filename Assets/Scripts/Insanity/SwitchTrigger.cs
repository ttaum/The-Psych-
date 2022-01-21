using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTrigger : MonoBehaviour
{

    [SerializeField]
    private GameObject input;

    [SerializeField]
    private GameObject InsanityStatic;

    public PlayerInputHandler InputHandler { get; private set; }

    public void Start()
    {
        InputHandler = input.GetComponent<PlayerInputHandler>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && InputHandler.InteractionInput == true)
        {
            // Вызываем делегата с логикой триггера
            InsanityStatic.SetActive(false);
            InputHandler.UseInteractionInput();      
        }
    }
}
