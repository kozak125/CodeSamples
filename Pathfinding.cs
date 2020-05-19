    /// <summary>
    /// For simplicity I've skipped namespaces
    /// </summary>    

    public class Pathfinding : MonoBehaviour
    {
        private static Pathfinding _instance;

        public static Pathfinding Instance
        {
            get { return _instance; }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        public bool FindPath(Vector3 startPos, Vector3 targetPos)
        {
            MovementTile startTile = PlayGrid.Instance.TileFromWorldPoint(startPos);
            MovementTile targetTile = PlayGrid.Instance.TileFromWorldPoint(targetPos);

            List<MovementTile> openSet = new List<MovementTile>();
            HashSet<MovementTile> closedSet = new HashSet<MovementTile>();

            openSet.Add(startTile);

            while (openSet.Count > 0)
            {
                MovementTile currentMovementTile = openSet[0];

		/// <summary>
		/// While I'm aware, that I could optimize searching through the list by using Heap
		/// with such a small grid as ours (5x5) the gain would be less than 0.1 ms
		/// so I decided not to waste time writing extra code
		/// </summary>
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentMovementTile.fCost || openSet[i].fCost == currentMovementTile.fCost && openSet[i].hCost < currentMovementTile.hCost)
                    {
                        currentMovementTile = openSet[i];
                    }
                }
                openSet.Remove(currentMovementTile);
                closedSet.Add(currentMovementTile);

                if (currentMovementTile == targetTile)
                {
                    print("Found a valid path");
                    return true;
                }

                List<MovementTile> movementNeighbours = PlayGrid.Instance.GetMovementNeighbours(currentMovementTile);
                List<FenceTile> fenceNeighbours = PlayGrid.Instance.GetFenceNeighbours(currentMovementTile);

                for(int x = 0; x < movementNeighbours.Count; x++)
                {
                    // if fence is detected or movement tile has already been checked, we want to skip it
                    if (!fenceNeighbours[x].isWalkable || closedSet.Contains(movementNeighbours[x]))
                    {
                        continue;
                    }
                    // we want to skip a starting tile for respective player, that started on it
                    if (BallGameManager.Instance.playerOne.position == startPos && movementNeighbours[x] == PlayGrid.Instance.playerOneStartingTile || BallGameManager.Instance.playerTwo.position == startPos && movementNeighbours[x] == PlayGrid.Instance.playerTwoStartingTile)
                    {
                        continue;
                    }

                    int newMovementCostToNeighbor = currentMovementTile.gCost + GetDistance(currentMovementTile, movementNeighbours[x]);

                    if (newMovementCostToNeighbor < movementNeighbours[x].gCost || !openSet.Contains(movementNeighbours[x]))
                    {
                        movementNeighbours[x].gCost = newMovementCostToNeighbor;
                        movementNeighbours[x].hCost = GetDistance(movementNeighbours[x], targetTile);
                        movementNeighbours[x].parent = currentMovementTile;

                        if (!openSet.Contains(movementNeighbours[x]))
                        {
                            openSet.Add(movementNeighbours[x]);
                        }
                    }
                }
            }

            // if openSet.Count == 0 then no path exists and fence placement is invalid!
            print("No path has been found");
            return false;
        }

        int GetDistance(MovementTile tileA, MovementTile tileB)
        {
            int distanceX = Mathf.Abs(tileA.GridX - tileB.GridX);
            int distanceY = Mathf.Abs(tileA.GridY - tileB.GridY);

            return 10 * distanceX + 10 * distanceY;
        }
    }
