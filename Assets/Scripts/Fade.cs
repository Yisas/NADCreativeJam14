using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
	static float fadeDistance = 7.5f;
	static float fadingSpeed = 0.1f;

	Transform player;
	bool fading = false;

	void Start()
	{
		player = GameObject.FindWithTag("Player").transform;
		foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
		{
			renderer.material = new Material(renderer.material);
		}
		StartCoroutine(FadeCoroutine());		
	}
	
	void Update()
	{
		if (fading)
			return;
		var distance = Vector3.Distance(transform.position, player.position);
		//if (distance <= fadeDistance)
		//	StartCoroutine(FadeCoroutine());
	}

	IEnumerator FadeCoroutine()
	{
		float fade = 0;
		while (fade < 1)
		{
			fade = Mathf.Clamp01(fade + fadingSpeed * Time.deltaTime);
			foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
			{
				renderer.material.SetFloat("_Amount", fade);
			}
			yield return null;
		}
	}
}
