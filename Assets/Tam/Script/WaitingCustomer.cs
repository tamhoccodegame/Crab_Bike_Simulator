using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingCustomer : MonoBehaviour
{
    private Animator animator;

    public string[] animationsName;
    public float timeToChangeAnim;
    public float changeAnimTimer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        changeAnimTimer = timeToChangeAnim;
		string animToPlay = animationsName[Random.Range(0, animationsName.Length)];
		animator.Play(animToPlay);
	}

    // Update is called once per frame
    void Update()
    {
        if(changeAnimTimer > 0)
        {
            changeAnimTimer -= Time.deltaTime;
        }
        else
        {
            string animToPlay = animationsName[Random.Range(0, animationsName.Length)];
            animator.Play(animToPlay);
            changeAnimTimer = timeToChangeAnim;
        }
    }
}
