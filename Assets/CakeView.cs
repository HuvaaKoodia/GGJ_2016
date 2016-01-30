using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CakeView : MonoBehaviour 
{
	public SpriteRenderer Sprite;
	public List<Sprite> Sprites;

	void Start()
	{
		SetCakeCompletionPercentage(0);
	}

	public void SetCakeCompletionPercentage(float percentComplete)
	{
		int correctIndex = Mathf.RoundToInt(percentComplete * (Sprites.Count - 1));
		Sprite.sprite = Sprites[correctIndex];
	}
}
