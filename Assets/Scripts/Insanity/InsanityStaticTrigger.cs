using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsanityStaticTrigger : MonoBehaviour
{

    public GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // гдеяэ леярн дкъ декецюрю         
        }
    }
}
