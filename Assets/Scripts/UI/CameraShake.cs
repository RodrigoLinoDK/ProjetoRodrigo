using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.5f;

    private Vector3 originalPos;
    private float currentShakeTime = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentShakeTime > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeMagnitude;
            currentShakeTime -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = originalPos;
        }
    }

    public void Shake(float duration, float magnitude)
    {
        Debug.Log("Treme");
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        currentShakeTime = duration;
    }
}
