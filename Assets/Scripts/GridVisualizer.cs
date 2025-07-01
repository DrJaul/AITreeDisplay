using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    public int Width = 10;
    public int Height = 10;
    public int Depth = 10;

    public GameObject GridCellPrefab;

    void Start()
    {
        for (int x = 0; x < Width; x++)
        for (int y = 0; y < Height; y++)
        for (int z = 0; z < Depth; z++)
        {
            Vector3 position = new Vector3(x, y, z);
            var cell = Instantiate(GridCellPrefab, position, Quaternion.identity, this.transform);
        }
    }
}
