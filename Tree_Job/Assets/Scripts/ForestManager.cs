using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manages trees in the scene throughout their lifetime
/// </summary>
public class ForestManager : MonoBehaviour
{
    [SerializeField]
    GameObject treeTrunk, genericTreePrefab; // those can become collections, if more types trees are introduced

    List<GameObject> treeTrunks;
    List<ITree> trees;

    [SerializeField]
    int minTreesAmount = 2, maxTreesAmount = 5, minTreesHeight = 10, maxTressHeight = 20;
    int randomHeight;
    int maxHeight = 0;
    int amountOfTrees;
    int currentTreeIndex = 0;

    [SerializeField]
    float distanceZBetweenTrees = 5f;

    Vector3 distanceBetweenTrees;

    readonly Quaternion treeTrunkRotation = Quaternion.Euler(-90, 0, 0);

    private void Start()
    {
       // get random number of trees
        amountOfTrees = Random.Range(minTreesAmount, maxTreesAmount + 1);
        trees = new List<ITree>(amountOfTrees);

        distanceBetweenTrees = new Vector3(0f, 0f, distanceZBetweenTrees);
        for (int i = 0; i < amountOfTrees; i++)
        {
            MakeRandomTreeTemplate(i);
        }

        PoolTreeTrunks();

        EventBroker.OnTreeDestroyed += OnTreeDestroyed;
    }

    /// <summary>
    /// Function used to create random template for tree, using given prefab
    /// </summary>
    /// <param name="treeTemplateIndex">index of the template</param>
    void MakeRandomTreeTemplate(int treeTemplateIndex)
    {
        randomHeight = Random.Range(minTreesHeight, maxTressHeight + 1);

        if (randomHeight > maxHeight)
        {
            maxHeight = randomHeight;
        }

        // Create new tree containers and assing heights to them 
        GameObject newTree = Instantiate(genericTreePrefab, treeTemplateIndex * distanceBetweenTrees, Quaternion.identity);
        GenericTree newGenericTree = newTree.GetComponent<GenericTree>();
        newGenericTree.Height = randomHeight;
        trees.Add(newGenericTree);
        // only one tree should be active at a time
        // trees should activate themselves at the start of BuildTree function
        newTree.SetActive(false);
    }

    /// <summary>
    /// Function used to pool trunks before the game starts
    /// </summary>
    void PoolTreeTrunks()
    {
        treeTrunks = new List<GameObject>(maxHeight);

        for (int i = 0; i < maxHeight; i++)
        {
            GameObject _treeChunk = Instantiate(treeTrunk, Vector3.zero, treeTrunkRotation);
            _treeChunk.SetActive(false);
            treeTrunks.Add(_treeChunk);
        }
    }

    /// <summary>
    /// Function used to invoke building logic on the concrete tree
    /// </summary>
    public void CreateTree()
    {
        trees[currentTreeIndex].BuildTree(treeTrunks, currentTreeIndex);
    }

    /// <summary>
    /// Function used to invoke logic, that removes trunks, on a concrete tree
    /// </summary>
    public void RemoveTreeTrunk()
    {
        trees[currentTreeIndex].RemoveTreeTrunk();
    }

    /// <summary>
    /// Function called when one of the trees is destroyed
    /// </summary>
    void OnTreeDestroyed()
    {
        // if there are still trees in the scene, the game is not over
        if (currentTreeIndex < amountOfTrees - 1)
        {
            currentTreeIndex++;
            EventBroker.CallOnTreeChanging(distanceBetweenTrees);
            CreateTree();
        }
        // if there are no more trees remaining the game is over
        else
        {
            EventBroker.OnTreeDestroyed -= OnTreeDestroyed;
            EventBroker.CallOnGameEnded();
        }
    }
}
