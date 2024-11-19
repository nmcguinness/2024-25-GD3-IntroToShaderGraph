using System.Collections;
using UnityEngine;

public class DripBehaviour : MonoBehaviour
{
    public Material material;
    public int rowCount = 8, columnCount = 8;
    public float timeBetwenFrames = 0.1f;

    private int rowID;
    private int columnID;
    private int row;
    private int column;

    private void Start()
    {
        // Accessing the shader propertes using their references and converting to ints
        rowID = Shader.PropertyToID("_Current_Row");
        columnID = Shader.PropertyToID("_Current_Column");

        StartCoroutine(SetNextFrame());
    }

    private IEnumerator SetNextFrame()
    {
        for (row = 0; row < rowCount; row++)
        {
            for (column = 0; column < columnCount; column++)
            {
                material.SetFloat(rowID, row);
                material.SetFloat(columnID, column);

                yield return new WaitForSeconds(timeBetwenFrames);
            }
        }

        // Restart the animation
        if (row == rowCount && column == columnCount)
        {
            row = 0;
            column = 0;
        }

        StartCoroutine(SetNextFrame());
    }
}