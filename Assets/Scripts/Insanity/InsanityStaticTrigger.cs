using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsanityStaticTrigger : MonoBehaviour
{
    [SerializeField]
    private EventManager eventManager;

    public GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            eventManager.DeathInvoke();
        }
    }
}
