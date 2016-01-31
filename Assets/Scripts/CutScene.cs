using UnityEngine;
using System.Collections;

public class CutScene : MonoBehaviour
{
    public InGameCameraController gameCamera;

    public Transform volcano;
    public Transform godLookatTarget, gameLookAtTarget;
    public Transform godCameraPositionTarget, gameCameraPositionTarget;

    public God theGod;
	public bool SkipInEditor = false;

	public CakeDude CakeDude;

	public AudioClip AngryGod, HappyGod;
    public AudioClip GodTalk;

    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void theGodShow()
    {
        StartCoroutine(godCutSceneCoroutine());
    }

	public IEnumerator godCutSceneCoroutine()
    {
#if UNITY_EDITOR
		if (SkipInEditor) yield break;
#endif
		yield return new WaitForSeconds(2f);

        gameCamera.LookAt(godLookatTarget, 2f);
        gameCamera.MoveToPosition(godCameraPositionTarget, 1f);
        theGod.GodAppear();
        yield return new WaitForSeconds(1);
        
        gameCamera.Zoom(25, 1);
        yield return new WaitForSeconds(1);
		gameCamera.Shake(0.10f);
		yield return new WaitForSeconds(1);
        source.PlayOneShot(GodTalk, 1);
        yield return new WaitForSeconds(2);
        theGod.GodDisappear();
        gameCamera.StopShake();
        yield return new WaitForSeconds(1);
        gameCamera.resetZoom(6);
		gameCamera.MoveToPosition(gameCameraPositionTarget, 2f);
		yield return new WaitForSeconds(0.5f);
        gameCamera.LookAt(gameLookAtTarget, 0.8f);
		yield return new WaitForSeconds(2f);
    }

	public IEnumerator MoveCakeUpVolcanoCutSceneCoroutine()
	{
		#if UNITY_EDITOR
		if (SkipInEditor) yield break;
		#endif
		yield return new WaitForSeconds(2f);

		CakeDude.DoYourThing();
		
		gameCamera.LookAt(CakeDude.transform, 2f);
		gameCamera.MoveToPosition(godCameraPositionTarget, 0.1f);

		yield return new WaitForSeconds(2f);
		gameCamera.Zoom(28, 0.5f);
		yield return new WaitForSeconds(8);

	}

	public IEnumerator godAngryCutSceneCoroutine()
	{
		#if UNITY_EDITOR
		if (SkipInEditor) yield break;
		#endif
		yield return new WaitForSeconds(2f);
		
		gameCamera.LookAt(godLookatTarget, 2f);
		gameCamera.MoveToPosition(godCameraPositionTarget, 1f);
		theGod.GodAppearAngry();
		yield return new WaitForSeconds(1);
        source.PlayOneShot(AngryGod, 1);
		gameCamera.Shake(0.3f);
		gameCamera.Zoom(28, 1);
		yield return new WaitForSeconds(5);
		theGod.GodDisappear();
		gameCamera.StopShake();
		yield return new WaitForSeconds(1);
		gameCamera.resetZoom(6);
		gameCamera.MoveToPosition(gameCameraPositionTarget, 2f);
		yield return new WaitForSeconds(0.5f);
		gameCamera.LookAt(gameLookAtTarget, 0.8f);
		yield return new WaitForSeconds(2f);
	}

	public IEnumerator godHappyCutSceneCoroutine()
	{
		#if UNITY_EDITOR
		if (SkipInEditor) yield break;
		#endif
		yield return new WaitForSeconds(2f);
		
		gameCamera.LookAt(godLookatTarget, 2f);
		gameCamera.MoveToPosition(godCameraPositionTarget, 1f);
		theGod.GodAppearHappy();
		yield return new WaitForSeconds(0.5f);
		source.PlayOneShot(HappyGod, 1);
		gameCamera.Zoom(28, 1);
		yield return new WaitForSeconds(5);
		gameCamera.Shake(0.3f);
		yield return new WaitForSeconds(10);
		theGod.GodDisappear();
		gameCamera.StopShake();
		yield return new WaitForSeconds(1);
		gameCamera.resetZoom(6);
		gameCamera.MoveToPosition(gameCameraPositionTarget, 2f);
		yield return new WaitForSeconds(0.5f);
		gameCamera.LookAt(gameLookAtTarget, 0.8f);
		yield return new WaitForSeconds(2f);
	}
}
