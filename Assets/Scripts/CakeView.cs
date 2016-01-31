using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CakeView : MonoBehaviour 
{
	public SpriteRenderer Sprite;
	public Image Image;
	public List<Sprite> Sprites;

	private int currentIndex = -1;

	void Start()
	{
		SetCompletionPercentage(0);
	}

	public void SetCompletionPercentage(float percentComplete)
	{
		int correctIndex = Mathf.RoundToInt(percentComplete * (Sprites.Count - 1));
		if (currentIndex == correctIndex) return;
		currentIndex = correctIndex;
		if (Sprite)
		{
			Sprite.sprite = Sprites[correctIndex];
		}
		if (Image)
		{
			Image.sprite = Sprites[correctIndex];
		}
	}
}
