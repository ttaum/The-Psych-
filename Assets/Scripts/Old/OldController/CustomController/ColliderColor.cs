using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderColor : MonoBehaviour
{
    public float timer = 0.0f;
    public bool isEnter = false;
    public int numberOfJumps;
   // public Animator animator;

    public PlayerController player;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (numberOfJumps < 3)
        {
            StartCoroutine("jumpCoroutine");
        }
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (numberOfJumps < 3)
        {
            StartCoroutine("jumpCoroutine");
        }
        Debug.Log("KEK");
    }

    IEnumerator jumpCoroutine()
    {
        while (numberOfJumps < 3)
        {
            while (timer < 0.3f)
            {
                //numberOfJumps = 0;
                Debug.Log("coroutine " + numberOfJumps);

                if (Input.GetButtonDown("Jump") && player.grounded)
                {
                    numberOfJumps += 1;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            timer = 0.0f;
            yield return new WaitForSeconds(2.0f);          
        }

        StopCoroutine("jumpCoroutine");
        Debug.Log("vse " + numberOfJumps);

    }
}
