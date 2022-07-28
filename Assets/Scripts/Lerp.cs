using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;

public class Lerp : MonoBehaviour
{
    public bool isBool;
    TilemapRenderer tilemap;
    public Transform eventPos;
    CinemachineVirtualCamera cm;
    CinemachineBasicMultiChannelPerlin noise;

    private void Awake()
    {
        tilemap = GetComponent<TilemapRenderer>();
        cm = FindObjectOfType<CinemachineVirtualCamera>();
        noise = cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public IEnumerator WallOpenEvent()
    {
        isBool = true;
        FindObjectOfType<CinemachineRange>().ChangeCollider(2);
        cm.Follow = eventPos;
        cm.LookAt = eventPos;
        noise.m_AmplitudeGain = 1;
        noise.m_FrequencyGain = 1;
        while (isBool)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, -10f, 2 * Time.deltaTime));
            yield return null;
            if (Vector3.Distance(transform.position, new Vector3(transform.position.x, -10f)) < 0.1f)
            {
                isBool = false;
            }
        }
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
        FindObjectOfType<CinemachineRange>().ChangeCollider(1);
        cm.Follow = FindObjectOfType<Character>().transform;
        cm.LookAt = FindObjectOfType<Character>().transform;
    }

    public IEnumerator WallCloseEvent()
    {
        isBool = true;
        cm.Follow = eventPos;
        cm.LookAt = eventPos;
        noise.m_AmplitudeGain = 1;
        noise.m_FrequencyGain = 1;
        while (isBool)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, 0f, 2 * Time.deltaTime));
            yield return null;
            if (Vector3.Distance(transform.position, new Vector3(transform.position.x, 0f)) < 0.1f)
            {
                isBool = false;
            }
        }
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
        cm.Follow = FindObjectOfType<Character>().transform;
        cm.LookAt = FindObjectOfType<Character>().transform;
        FindObjectOfType<Event>().isComplete = true;
    }
}
