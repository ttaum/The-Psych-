using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsanityDestrTrigger : MonoBehaviour
{
    Material material;

    float fade = 1f;

    private void Start()
    {
        material = GetComponent<SpriteRenderer>().material;     
    }

    private void Update()
    {
        if (fade < 1f)
        {
            fade += Time.deltaTime / 2f;
            Debug.Log("ek");
        }

        material.SetFloat("_Fade", fade);
    }
    public void Dissolving()
    {
        fade -= Time.deltaTime;
  
        if (fade <= 0f)
        {
            fade = 0f;
            gameObject.SetActive(false);
        }
    }
}
