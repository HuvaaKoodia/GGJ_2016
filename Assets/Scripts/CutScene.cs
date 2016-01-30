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

    private IEnumerator godCutSceneCoroutine()
    {
        gameCamera.LookAt(godLookatTarget);
        gameCamera.MoveToPosition(godCameraPositionTarget);
        theGod.GodAppear();
        yield return new WaitForSeconds(1);
        gameCamera.Shake(0.3f);
        gameCamera.Zoom(10, 1);
        yield return new WaitForSeconds(8);
        theGod.GodDisappear();
        gameCamera.StopShake();
        yield return new WaitForSeconds(1);
        gameCamera.resetZoom(6);
        gameCamera.LookAt(gameLookAtTarget);
        gameCamera.MoveToPosition(gameCameraPositionTarget);
    }
}
