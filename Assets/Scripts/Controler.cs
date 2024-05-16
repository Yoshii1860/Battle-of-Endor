using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler : MonoBehaviour
{
    [Header("General Setup Settings")]
    [Tooltip("How fast ship moves up and down")] [SerializeField] float xControlSpeed = 30f;
    [Tooltip("How fast ship moves left and right")] [SerializeField] float yControlSpeed = 15f;
    [Tooltip("How far player moves horizontally")] [SerializeField] float xRange = 10f;
    [Tooltip("How far player moves vertically")] [SerializeField] float yRange = 2f;

    [Header("Laser Gun Array")]
    [Tooltip("Add all player lasers here")]
    [SerializeField] GameObject[] lasers;

    [Header("Screen position-based Tuning")]
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float positionYawFactor = 2f;

    [Header("Player input-based Tuning")]
    [SerializeField] float controlPitchFactor = -13f;
    [SerializeField] float controlRollFactor = -20f;

    AudioSource audioClip;

    float xThrow;
    float yThrow;

    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControl = yThrow * controlPitchFactor;
        
        float pitch = pitchDueToPosition + pitchDueToControl;
        float yaw = transform.localPosition.x * positionYawFactor;
        float roll = xThrow * controlRollFactor;
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }
    void ProcessTranslation()
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow * Time.deltaTime * xControlSpeed;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        float yOffset = yThrow * Time.deltaTime * yControlSpeed;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3 (clampedXPos, clampedYPos, 
        transform.localPosition.z);
    }

    void ProcessFiring()
    {
        if (Input.GetButton("Fire1"))
        {
            SetLasersActive(true);
            PlayAudio();
        }
        else{
            SetLasersActive(false);
            StopAudio();
        }
    }

    void SetLasersActive(bool isActive)
    {
        foreach (GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }

    void PlayAudio()
    {
        audioClip = GetComponent<AudioSource>();
        if (!audioClip.isPlaying)
        audioClip.Play(0);
    }

    void StopAudio()
    {
        audioClip = GetComponent<AudioSource>();
        if (audioClip.isPlaying)
        audioClip.Stop();
    }
}
