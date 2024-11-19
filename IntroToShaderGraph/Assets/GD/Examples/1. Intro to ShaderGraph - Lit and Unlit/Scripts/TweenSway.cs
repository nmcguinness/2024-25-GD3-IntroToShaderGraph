using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

/// <summary>
/// This script makes a light sway over time around a user-defined axis using DOTween.
/// </summary>
[RequireComponent(typeof(Light))]
public class TweenSway : MonoBehaviour
{
    [SerializeField, Tooltip("The ease function to use for the sway.")]
    private Ease easeFunction = Ease.InOutSine;

    [SerializeField, Tooltip("The axis to sway the light around.")]
    private Vector3 swayAxis = Vector3.up;

    [SerializeField, Tooltip("The angle of the sway in degrees.")]
    [Range(0f, 90f)]
    private float swayAngle = 5f;

    [SerializeField, Tooltip("The duration of the sway in secs.")]
    private float swayDuration = 2f;

    [SerializeField]
    private bool isPositiveSway = true; // Tracks the direction of the sway.

    private Tween swayTween;

    private void Start()
    {
        // Start the sway animation on play.
        StartSway();
    }

    /// <summary>
    /// Starts the sway animation using DOTween with step callbacks.
    /// </summary>
    private void StartSway()
    {
        // Ensure any previous tween is killed to avoid stacking animations.
        swayTween?.Kill();

        // Save the original rotation to base all sway on.
        Quaternion originalRotation = transform.localRotation;

        // Calculate the sway rotations based on the axis and sway angle.
        Quaternion positiveSway = Quaternion.AngleAxis(swayAngle, swayAxis) * originalRotation;
        Quaternion negativeSway = Quaternion.AngleAxis(-swayAngle, swayAxis) * originalRotation;

        // Begin the swaying sequence.
        PlaySwayStep(originalRotation, positiveSway, negativeSway);
    }

    /// <summary>
    /// Animates a single sway step and sets up the next step.
    /// </summary>
    /// <param name="originalRotation">The original rotation of the light.</param>
    /// <param name="positiveSway">The positive sway rotation.</param>
    /// <param name="negativeSway">The negative sway rotation.</param>
    private void PlaySwayStep(Quaternion originalRotation, Quaternion positiveSway, Quaternion negativeSway)
    {
        // Determine the target rotation based on the current sway direction.
        Quaternion targetRotation = isPositiveSway ? positiveSway : negativeSway;

        // Animate the rotation to the target rotation.
        swayTween = transform.DOLocalRotateQuaternion(targetRotation, swayDuration)
            .SetEase(easeFunction) // Smooth in-out easing for a natural sway.
            .OnComplete(() =>
            {
                // Switch the direction of the sway.
                isPositiveSway = !isPositiveSway;

                // Trigger the next step.
                PlaySwayStep(originalRotation, positiveSway, negativeSway);
            });
    }

    /// <summary>
    /// Cleanup any active DOTween animations when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        swayTween?.Kill(); // Kill the tween to prevent memory leaks.
    }
}