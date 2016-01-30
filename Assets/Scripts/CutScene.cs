using UnityEngine;
using System.Collections;

public class CutScene : MonoBehaviour
{
    public InGameCameraController gameCamera;

    public Transform volcano;
    public Transform villager;
    public Transform target;

    // Use this for initialization
    void Start()
    {
        testCutScene();
    }

    private void testCutScene(){
        StartCoroutine(testCutSceneCoroutine());
    }

    private IEnumerator testCutSceneCoroutine()
    {
        gameCamera.Shake(0.3f);
        gameCamera.Zoom(10, 1);
        gameCamera.LookAt(volcano);
        yield return new WaitForSeconds(5);
        gameCamera.StopShake();
        gameCamera.Zoom(40, 10);       
        gameCamera.LookAt(villager);
        gameCamera.MoveToPosition(target);
    }
}
