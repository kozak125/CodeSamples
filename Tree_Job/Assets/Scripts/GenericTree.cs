using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenericTreeAnimationManager))]
/// <summary>
/// Main class for a concrete, generic tree
/// Later it can become an abstract class, that can be derived from by specific trees
/// </summary>
public class GenericTree : MonoBehaviour, ITree
{
    public int Height { set => height = value; }

    int height;

    GenericTreeAnimationManager animationManager;

    float timer;
    float amountYToMoveChunk;
    readonly float errorMargin = 0.01f;

    Stack<GameObject> activeTreeTrunks;

    Transform treeTransform;

    GameObject thisTree;

    private void Awake()
    {
        animationManager = GetComponent<GenericTreeAnimationManager>();
    }

    /// <summary>
    /// Building function, that tree uses to build itself out of pooled trunks.
    /// This is also, where it starts a coroutine to pop out of the ground.
    /// Call it from ForestManager script.
    /// </summary>
    /// <param name="treeTrunks">The collection of pooled trunks</param>
    /// <param name="treeIndex">Index of the tree</param>
    public void BuildTree(List<GameObject> treeTrunks, int treeIndex)
    {
        // make sure to make the game object active
        thisTree = gameObject;
        thisTree.SetActive(true);

        treeTransform = transform;

        activeTreeTrunks = new Stack<GameObject>(height);

        for (int i = 0; i < height; i++)
        {
            treeTrunks[i].transform.SetParent(treeTransform);
        }

        // Get the height of the mesh of the prefab
        MeshRenderer mesh = treeTrunks[0].GetComponent<MeshRenderer>();
        amountYToMoveChunk = mesh.bounds.size.y;

        // Tree starts under ground, so build the tree going top to bottom
        for (int i = 0; i < height; i++)
        {
            Vector3 chunkPosition = new Vector3(treeTransform.position.x, -i * amountYToMoveChunk, treeTransform.position.z);
            treeTrunks[i].transform.position = chunkPosition;
            treeTrunks[i].SetActive(true);

            activeTreeTrunks.Push(treeTrunks[i]);
        }

        StartCoroutine(RaiseTreeCoroutine(treeIndex));
    }

    /// <summary>
    /// Remove trunk from a tree, call it from ForestManager script
    /// </summary>
    public void RemoveTreeTrunk()
    {
        height--;
        activeTreeTrunks.Peek().SetActive(false);
        activeTreeTrunks.Pop();

        if (activeTreeTrunks.Count > 0)
        {
            // make sure to stop coroutines in case player clicks multiple times, before it's finished
            StopAllCoroutines();
            StartCoroutine(CollapseTreeCoroutine());
        }
        else
        {
            // if there are no trunks left, raise an even and deactivate
            EventBroker.CallOnTreeDestroyed();
            thisTree.SetActive(false);
        }
    }

    /// <summary>
    /// Coroutine used to animate collapsing of a tree after removing a trunk
    /// </summary>
    IEnumerator CollapseTreeCoroutine()
    {
        float targetHeight = height * amountYToMoveChunk;
        float fallingSpeed = Random.Range(0.5f, 1f);
        timer = 0;

        while (treeTransform.position.y >= targetHeight + errorMargin)
        {
            timer += Time.deltaTime * fallingSpeed;
            Vector3 treePosition = treeTransform.position;
            treePosition.y = Mathf.Lerp(treeTransform.position.y, targetHeight, timer);
            treeTransform.position = treePosition;

            yield return null;
        }
    }

    /// <summary>
    /// Coroutine used to raise the tree above the ground
    /// </summary>
    /// <param name="treeIndex">Index of the tree</param>
    IEnumerator RaiseTreeCoroutine(int treeIndex)
    {
        float targetHeight = height * amountYToMoveChunk;
        float raisingSpeed = Random.Range(0.1f, 0.3f);
        timer = 0;

        // start bouncing animation before the tree starts raising
        animationManager.Bounce();

        while (treeTransform.position.y <= targetHeight - errorMargin)
        {
            timer += Time.deltaTime * raisingSpeed;

            Vector3 treePosition = treeTransform.position;
            treePosition.y = Mathf.Lerp(treeTransform.position.y, targetHeight, timer);
            treeTransform.position = treePosition;

            yield return null;
        }

        // if this is the first tree, we want to raise an OnAnimationEnded here, rather than on camera
        // this is because for the first tree camera is already in place, but we don't want players to be able to remove trunks until after tree has risen
        if (treeIndex == 0)
        {
            EventBroker.CallOnAnimationEnded();
        }
    }
}
