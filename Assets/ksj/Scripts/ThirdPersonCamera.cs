using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{
    public float distanceAway;
    public float distanceUp;
    public float smooth;

    private GameObject hovercraft;
    private Vector3 targetPosition;
    public static int attackCamera;

    Transform follow;

    void Start()
    {
        attackCamera = 0;
        follow = GameManager.Instance.playerObject[GameManager.Instance.prevCharacter].transform;
        //follow = GameObject.FindWithTag("Player").transform;
    }

    void LateUpdate()
    {
        if (attackCamera == 1)
        {
            targetPosition = follow.position + Vector3.up * distanceUp + follow.forward * 2.1f - follow.right * distanceAway;

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);

            transform.LookAt(follow);
        }
        else if (attackCamera == 2)
        {
            targetPosition = follow.position + Vector3.up * 4f - follow.forward * 15f;

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);

            transform.LookAt(GameManager.Instance.foundEnemy.transform);
        }
        else if (attackCamera == 3)
        {
            targetPosition = follow.position + Vector3.up * 4f - follow.forward * 6f;

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);

            transform.LookAt(GameManager.Instance.foundEnemy.transform);
        }
        else if (attackCamera == 0)
        {
            targetPosition = follow.position + Vector3.up * distanceUp - follow.forward * distanceAway;

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);

            transform.LookAt(follow);
        }

    }
    void Update()
    {
        follow = GameManager.Instance.playerObject[GameManager.Instance.prevCharacter].transform;
    }
}