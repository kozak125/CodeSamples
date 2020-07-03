using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Interface implemented by trees
/// </summary>
public interface ITree
{
    /// <summary>
    /// Building function, that tree uses to build itself out of pooled trunks.
    /// This is also, where it starts a coroutine to pop out of the ground.
    /// </summary>
    /// <param name="treeTrunks">The collection of pooled trunks</param>
    /// <param name="treeIndex">Index of the tree</param>
    void BuildTree(List<GameObject> treeTrunks, int treeIndex);

    /// <summary>
    /// Remove trunk from a tree, call it from ForestManager script
    /// </summary>
    void RemoveTreeTrunk();

    int Height { set; }
}
