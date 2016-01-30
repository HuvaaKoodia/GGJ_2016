using UnityEngine;
using System.Collections;

public class CutScene : MonoBehaviour
{
    public InGameCameraController gameCamera;

    public Transform volcano;
    public Transform godLookatTarget, gameLookAtTarget;
    public Transform godCameraPositionTarget, gameCameraPositionTarget;

    public God theGod;

    public void theGodShow()
    {
        StartCoroutine(godCutSceneCoroutine());
    }

	public IEnumerator godCutSceneCoroutine()
    {
        gameCamera.LookAt(godLookatTarget, 2f);
        gameCamera.MoveToPosition(godCameraPositionTarget, 1f);
        theGod.GodAppear();
        yield return new WaitForSeconds(1);
        gameCamera.Shake(0.3f);
        gameCamera.Zoom(25, 1);
        yield return new WaitForSeconds(8);
        theGod.GodDisappear();
        gameCamera.StopShake();
        yield return new WaitForSeconds(1);
        gameCamera.resetZoom(6);
		gameCamera.MoveToPosition(gameCameraPositionTarget, 2f);
		yield return new WaitForSeconds(0.5f);
        gameCamera.LookAt(gameLookAtTarget, 0.8f);
		yield return new WaitForSeconds(1f);
        
    }
}
