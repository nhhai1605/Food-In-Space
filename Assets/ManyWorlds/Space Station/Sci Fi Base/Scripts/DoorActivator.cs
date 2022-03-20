using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animation))]
public class DoorActivator : MonoBehaviour 
{
	public AnimationClip AnimPlay;
	public AnimationClip AnimReset;
	public float TimeToReset;
	private Animation AnimatorEffect;
	private float timer;
	private bool collide = false;

	void Start()
	{
		AnimatorEffect = GetComponent<Animation> ();
	}

	void OnTriggerEnter(Collider col)
	{

		if(timer == 0)
		{
		
			AnimatorEffect.Play(AnimPlay.name);
			timer = 0f;
			if (!collide)
			{
				collide = true;
			}

		}
	}

	void Update()
	{
		if (collide) 
		{
			timer = timer + Time.deltaTime;

			if (timer > TimeToReset)
			{
				AnimatorEffect.Play(AnimReset.name);

				timer=0f;
				collide=false;
			}

		}
	}
}
