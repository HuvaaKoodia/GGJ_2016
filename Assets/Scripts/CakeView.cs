using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CakeView : MonoBehaviour 
{
	public SpriteRenderer Sprite;
	public Image Image;
	public List<Sprite> Sprites;

	public Sprite OptionalEndSprite;

	private int currentIndex = -1;

	void Start()
	{
		SetCompletionPercentage(0);
	}

	public void SetCompletionPercentage(float percentComplete)
	{
		int correctIndex = (int)Mathf.Floor(percentComplete * (Sprites.Count));

		if (percentComplete == 1 && OptionalEndSprite != null && ((Image && Image.sprite != OptionalEndSprite) || (Sprite && Sprite.sprite != OptionalEndSprite)))
		{
			if (Sprite) Sprite.sprite = OptionalEndSprite;
			if (Image) Image.sprite = OptionalEndSprite;
			return;
		}

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
