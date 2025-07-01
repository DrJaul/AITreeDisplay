using System.Collections;
using UnityEngine;
using AITree.Core;
using AITree.AI;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using System.Collections.Generic;

public class AITreeRunner : MonoBehaviour
{
    public int Width = 10;
    public int Height = 10;
    public int Depth = 10;
    public float GrowthInterval = 0.333f; // 3 growths per second
    public GameObject BranchPrefab;
    public GameObject StemPrefab;
    public GameObject LeafPrefab;

    private AITree.Core.Tree _tree;
    private TreeAI _treeAI;

    private Coroutine _growthCoroutine;

    void Start()
    {
        _tree = new AITree.Core.Tree(Width, Height, Depth, initialCurrentEnergy: 10);
        _treeAI = new TreeAI(_tree);

        var rootPosition = new System.Numerics.Vector3(Width / 2f, 0f, Depth / 2f);
        var root = new BranchNode(rootPosition);
        _tree.AddNode(root);
       

        SpawnVisual(root);

        _growthCoroutine = StartCoroutine(GrowLoop());
    }

    private IEnumerator GrowLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(GrowthInterval);

            if (_treeAI.TryGrow() is true)
            {
                var latest = GetLatestNode();
                if (latest != null)
                {
                    SpawnVisual(latest);
                }
            }
        }
    }

    private IGrowable GetLatestNode()
    {
        foreach (var node in _tree.Grid.Values)
        {
            if (node.IsOccupied && node.Growable != null &&
                !_spawnedPositions.Contains(Vector3Int.RoundToInt(ToUnityVec3(node.Position))))
            {
                return node.Growable;
            }
        }
        return null;
    }

    private readonly HashSet<Vector3Int> _spawnedPositions = new();

    private void SpawnVisual(IGrowable growable)
    {
        var pos = Vector3Int.RoundToInt(ToUnityVec3(growable.Position));
        if (_spawnedPositions.Contains(pos)) return;

        GameObject prefab = null;
        if (growable is BranchNode) prefab = BranchPrefab;
        else if (growable is StemNode) prefab = StemPrefab;
        else if (growable is LeafNode) prefab = LeafPrefab;

        if (prefab != null)
        {
            Instantiate(prefab, pos, UnityEngine.Quaternion.identity);
            _spawnedPositions.Add(pos);
        }
    }

    private Vector3 ToUnityVec3(System.Numerics.Vector3 v)
    {
        return new Vector3(v.X, v.Y, v.Z);
    }

    // Public methods for UIController
    public int GetCurrentEnergy() => _tree.CurrentEnergy;

    public void SetCurrentEnergy(int value)
    {
        _tree.CurrentEnergy = value;
    }

    public void SetGrowthInterval(float interval)
    {
        GrowthInterval = interval;

        if (_growthCoroutine != null)
        {
            StopCoroutine(_growthCoroutine);
            _growthCoroutine = StartCoroutine(GrowLoop());
        }
    }

    public int GetEnergyIn() => _tree.EnergyIn;
    public int GetEnergyOut() => _tree.EnergyOut;

    public AITree.Core.Tree GetTree() => _tree;
}
