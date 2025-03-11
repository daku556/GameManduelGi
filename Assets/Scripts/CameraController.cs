using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] float trackTime = 0.3f;
    private GameObject player;
    private Vector3 targetCameraPosition;
    private Queue<Vector3> positionHistory = new Queue<Vector3>();
    private Queue<float> timeStamps = new Queue<float>(); // ����� �ð� ���
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    targetCameraPosition = player.transform.position + cameraOffset;
    //    positionHistory.Enqueue(targetCameraPosition);
    //    timeStamps.Enqueue(Time.time);

    //    while (timeStamps.Count > 0 && Time.time - timeStamps.Peek() > trackTime)
    //    {
    //        positionHistory.Dequeue();
    //        timeStamps.Dequeue();
    //    }
    //}

    void LateUpdate()
    {
        targetCameraPosition = player.transform.position + cameraOffset;
        transform.position = targetCameraPosition;
        //transform.position = GetPositionTrackTimeAgo();
    }

    private Vector3 GetPositionTrackTimeAgo()
    {
        if (positionHistory.Count > 0)
        {
            return positionHistory.Peek(); // 1�� �� ��ġ ��ȯ
        }
        return transform.position; // �����Ͱ� ������ ���� ��ġ ��ȯ
    }
}
