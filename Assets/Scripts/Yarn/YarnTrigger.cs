using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class YarnTrigger : MonoBehaviour
{
    private Light2D yarnLight;

    [SerializeField] private GameObject vinylManagerObj;

    private VinylManager VinylManager;

    [SerializeField]
    private bool isOn;

    private void Awake()
    {
        yarnLight = GetComponent<Light2D>();
        VinylManager = vinylManagerObj.GetComponent<VinylManager>();
        isOn = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player") && (VinylManager.currentYarnCharges > 0) && (isOn == false))
        {
            
            VinylManager.currentYarnCharges -= 1;

            yarnLight.intensity = 2f;

            isOn = true;
        }
    }
}
