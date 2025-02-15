using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingEffect : MonoBehaviour
{
    public GameObject lightningEffect;
    public GameObject thunderEffect;
    /*public AudioSource thunderSound; // Uncomment to enable sound*/
    public float lightningDelayMin = 0.5f;
    public float lightningDelayMax = 1f;
    public float thunderDelayMin = 1f;
    public float thunderDelayMax = 3f;
    private void Start()
    {
        StartCoroutine(TriggerLightning());
        StartCoroutine(TriggerThunder());
    }

    private IEnumerator TriggerLightning()
    {
        while (true)
        {
            float lightningDelay = Random.Range(lightningDelayMin, lightningDelayMax);
            yield return new WaitForSeconds(lightningDelay);

        
            yield return new WaitForSeconds(0.2f); 

            lightningEffect.SetActive(true);

            yield return new WaitForSeconds(0.2f);

            lightningEffect.SetActive(false);
        }
    }
    private IEnumerator TriggerThunder()
    {
        while (true)
        {
            float lightningDelay = Random.Range(thunderDelayMin, thunderDelayMax);
            yield return new WaitForSeconds(lightningDelay);


            yield return new WaitForSeconds(0.3f);

            thunderEffect.SetActive(true);

            yield return new WaitForSeconds(2f);

            thunderEffect.SetActive(false);
        }
    }

}
