using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace GD.Shaders
{
    /// <summary>
    /// Animates a shader by updating the current row and column of a texture grid.
    /// </summary>
    public class ShaderFrameAnimator : MonoBehaviour
    {
        #region Fields

        [Title("Animation Dimensions")]
        [SerializeField]
        [Tooltip("Material to animate (can be assigned in Inspector)")]
        private Material material; // Material to animate (can be assigned in Inspector)

        [Title("Animation Dimensions")]
        [SerializeField]
        [Tooltip("Number of rows in the animation grid")]
        [Min(1)]
        private int rowCount = 8;

        [SerializeField]
        [Tooltip("Number of columns in the animation grid")]
        [Min(1)]
        private int columnCount = 8;

        [Title("Timing")]
        [SerializeField]
        [Tooltip("Minimum time between frame updates in seconds")]
        [Min(0.01f)]
        private float minTimeBetweenFrames = 0.1f;

        [SerializeField]
        [Tooltip("Maximum time between frame updates in seconds")]
        [Min(0.01f)]
        private float maxTimeBetweenFrames = 0.5f;

        [Title("Optional")]
        [SerializeField]
        [Tooltip("Initial delay before the animation starts in seconds")]
        [Min(0.01f)]
        private float startDelay = 1.0f; // Initial delay before the animation starts

        [SerializeField]
        [Tooltip("The frame to start the animation from. 0 is the first frame.")]
        [Min(0)]
        private int startFrame = 0;

        #endregion Fields

        private int rowID; // Shader property ID for the current row
        private int columnID; // Shader property ID for the current column

        #region Methods

        private void Start()
        {
            // Access shader property IDs
            rowID = Shader.PropertyToID("_Current_Row");
            columnID = Shader.PropertyToID("_Current_Column");

            // Get the material from the MeshRenderer if not manually assigned
            if (material == null)
                throw new System.ArgumentNullException("MeshRenderer is missing on this GameObject.");

            // Start animation coroutine
            StartCoroutine(AnimateMaterial());
        }

        /// <summary>
        /// Move through the animation grid by updating the shader properties.
        /// </summary>
        /// <returns></returns>
        private IEnumerator AnimateMaterial()
        {
            // Wait for the initial delay
            if (startDelay > 0)
            {
                yield return new WaitForSeconds(startDelay);
            }

            int totalFrames = rowCount * columnCount; // Total number of frames in the grid
            int currentFrame = Mathf.Clamp(startFrame, 0, totalFrames - 1); // Ensure startFrame is within bounds

            while (true) // Infinite loop for continuous animation
            {
                for (int frame = currentFrame; frame < totalFrames; frame++)
                {
                    int row = frame / columnCount; // Calculate row from frame index
                    int column = frame % columnCount; // Calculate column from frame index

                    // Update shader properties
                    material.SetFloat(rowID, row);
                    material.SetFloat(columnID, column);

                    // Wait for a random time between the min and max range
                    float randomWaitTime = Random.Range(minTimeBetweenFrames, maxTimeBetweenFrames);
                    yield return new WaitForSeconds(randomWaitTime);
                }

                // Restart from the beginning after completing all frames
                currentFrame = 0;
            }
        }

        private void OnDisable()
        {
            // Stop the animation coroutine when the script is disabled
            StopAllCoroutines();
        }

        #endregion Methods
    }
}