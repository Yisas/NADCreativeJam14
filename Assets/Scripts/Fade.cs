using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
	static float fadeDistance = 25;
	static float fadingSpeed = 0.4f;

	Transform player;
	bool fading = false;

	void Start()
	{
		player = GameObject.FindWithTag("Player").transform;
		foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
		{
			renderer.material = new Material(renderer.material);
		}
		GetComponentInChildren<Animator>().SetBool("flapping", true);
	}
	
	void Update()
	{
		if (fading || player == null)
			return;
		var distance = Vector3.Distance(transform.position, player.position);
		if (!fading && distance <= fadeDistance)
			StartCoroutine(FadeCoroutine());
	}

	IEnumerator FadeCoroutine()
	{
		fading = true;
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
		Destroy(gameObject);
	}
}
