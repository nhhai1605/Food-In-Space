using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class DoorActivatorMecanim : MonoBehaviour 
{
    private Animator DoorAnimator;

	void Start()
	{
        DoorAnimator = GetComponent<Animator> ();
	}

	void OnTriggerEnter(Collider col)
	{
        DoorAnimator.SetTrigger("open");
	}
}
