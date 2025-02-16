using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveLight : MonoBehaviour
{
    public ParticleSystem mainParticleSystem;
    public ParticleSystem subEmitter;
    public List<GameObject> activationLights;

    private void Start()
    {
        StartCoroutine(TriggerLightning());
    }

    private IEnumerator TriggerLightning()
    {
        while (true)
        {
            GameObject randomLightObject = activationLights[Random.Range(0, activationLights.Count)];

            Light randomLight = randomLightObject.GetComponent<Light>();

            if (randomLight != null)
            {
                randomLightObject.SetActive(true);

                yield return new WaitForSeconds(0.2f);

                subEmitter.Play();

                yield return new WaitForSeconds(0.2f);

                subEmitter.Stop();
                randomLightObject.SetActive(false);
            }
        }
    }
}
