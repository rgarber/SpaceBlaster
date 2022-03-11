using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform cameraTransform = default;
    private Vector3 _originalPosOfCam = default;
    public float shakeFrequency = default;
    
    float _duration = 50f;
    float _totalTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
        _originalPosOfCam = cameraTransform.position;   
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void CameraShakeStart()
    {

        StartCoroutine(ShakeTheCamera());

    }
    
    IEnumerator ShakeTheCamera()
    {
        while(_totalTime <= _duration)
        {
            yield return new WaitForSeconds(.01f);
            cameraTransform.position = _originalPosOfCam + Random.insideUnitSphere * shakeFrequency;
            _totalTime = _totalTime + 1;
            
        }

        cameraTransform.position = _originalPosOfCam;
        _totalTime = 0f;
    }


    private void StopShake()
    {
        cameraTransform.position = _originalPosOfCam;
    }




}
