    /// <summary>
    /// For simplicity I've skipped namespaces
    /// Also, I now know, that fields are supposed to be private and would create properties along side them
    /// If I needed to expose something to editor, I would use [SerializeField] attribute
    /// </summary>    
    public class PlayGrid : MonoBehaviour
    {
        private static PlayGrid _instance;

        public static PlayGrid Instance
        {
            get { return _instance; }
        }

        public GameObject movementTile;
        public GameObject fenceTile;

        public Vector2 GridSize;

        public float TileRadius;
        float tileDiameter;

        int gridSizeX, gridSizeY;

        public MovementTile playerOneStartingTile;
        public MovementTile playerTwoStartingTile;

        public Dictionary<MovementTile, GameObject> TileScriptToTileGameObjectMapping = new Dictionary<MovementTile, GameObject>();
        public Dictionary<GameObject, FenceTile> FenceGameObjectToFenceScriptMapping = new Dictionary<GameObject, FenceTile>();

        MovementTile[,] movementGrid;
        FenceTile[,] fenceGridHorizontal;// down-up
        FenceTile[,] fenceGridVertical;// left-right
        // up,right no change to coordinates, left y-1, down x-1

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

            tileDiameter = TileRadius * 2;
            gridSizeX = Mathf.RoundToInt(GridSize.x / tileDiameter);
            gridSizeY = Mathf.RoundToInt(GridSize.y / tileDiameter);

            if (gridSizeX > 1 && gridSizeY > 1)
            {
                CreateGrid();
            }
            else
            {
		/// <summary>
		/// I could also throw an OutOfRangeException
		/// </summary>
                Debug.Log("Could not create grid, grid size cannot be lower than 2");
            }
        }

        private void Start()
        {
            playerOneStartingTile = TileFromWorldPoint(GameObject.FindGameObjectWithTag("Player").transform.position);
            playerTwoStartingTile = TileFromWorldPoint(GameObject.FindGameObjectWithTag("Player2").transform.position);

            GraphicsManager.Instance.AssignStartingTilesMaterial(playerOneStartingTile, playerTwoStartingTile);
        }
        void CreateGrid()
        {
            movementGrid = new MovementTile[gridSizeX, gridSizeY];
            Vector3 worldBottomLeft = transform.position - Vector3.right * GridSize.x / 2 - Vector3.forward * GridSize.y / 2;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * tileDiameter + TileRadius) + Vector3.forward * (y * tileDiameter + TileRadius);
                    movementGrid[x, y] = new MovementTile(worldPoint, x, y);
                    GameObject tile = Instantiate(movementTile, worldPoint, Quaternion.identity, transform);
                    TileScriptToTileGameObjectMapping.Add(movementGrid[x, y], tile);
                }
            }

            fenceGridHorizontal = new FenceTile[gridSizeX-1, gridSizeY];
            Quaternion horizontalRotation = Quaternion.Euler(0, 90, 0);

            for (int x = 0; x < gridSizeX-1; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * tileDiameter + tileDiameter) + Vector3.forward * (y * tileDiameter + TileRadius);
                    fenceGridHorizontal[x, y] = new FenceTile(worldPoint, x, y);
                    GameObject fence = Instantiate(fenceTile, worldPoint, horizontalRotation, transform);
                    FenceGameObjectToFenceScriptMapping.Add(fence, fenceGridHorizontal[x, y]);
                }
            }

            fenceGridVertical = new FenceTile[gridSizeX, gridSizeY - 1];

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY-1; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * tileDiameter + TileRadius) + Vector3.forward * (y * tileDiameter + tileDiameter);
                    fenceGridVertical[x, y] = new FenceTile(worldPoint, x, y);
                    GameObject fence = Instantiate(fenceTile, worldPoint, Quaternion.identity, transform);
                    FenceGameObjectToFenceScriptMapping.Add(fence, fenceGridVertical[x, y]);
                }
            }
        }

        public MovementTile TileFromWorldPoint(Vector3 worldPoint)
        {
            float percentX = Mathf.Clamp01((worldPoint.x + GridSize.x / 2) / GridSize.x);
            float percentY = Mathf.Clamp01((worldPoint.z + GridSize.y / 2) / GridSize.y);
        
            int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((gridSizeX - 1) * percentY);

            return movementGrid[x, y];
        }

        public List<MovementTile> GetMovementNeighbours(MovementTile movementTile)
        {
            List<MovementTile> neighbours = new List<MovementTile>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
		    // we don't want diagonal neighbours
                    if (x==0 && y==0 || x==-1 && y==-1 || x==1 && y==1 || x==-1 && y==1 || x==1 && y==-1)
                    {
                        continue;
                    }

                    int checkX = movementTile.GridX + x;
                    int checkY = movementTile.GridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >=0 && checkY < gridSizeY)
                    {
                        neighbours.Add(movementGrid[checkX, checkY]);
                    }
                }
            }
            return neighbours;
        }

        public List<FenceTile> GetFenceNeighbours(MovementTile movementTile)
        {
            List<FenceTile> neighbours = new List<FenceTile>();

            int checkVerticalY = movementTile.GridY;
            int checkHorizontalX = movementTile.GridX;

            /// make sure, that tiles are always added in the following order relative to the tile you passed: DOWN LEFT RIGHT UP
            /// this ensures consistency with the list of neigbouring MovementTiles
            // if this is last or earlier, but not first movement tile
            if (checkHorizontalX > 0 && checkHorizontalX <= gridSizeX-1)///down
            {
                // then we are sure to have a space for fence below it
                neighbours.Add(fenceGridHorizontal[movementTile.GridX - 1, movementTile.GridY]);
            }
            // if this is last or earlier, but not first movement tile
            if (checkVerticalY > 0 && checkVerticalY <= gridSizeY-1)///left
            {
                // then we are sure to have a space for fence on it's left
                neighbours.Add(fenceGridVertical[movementTile.GridX, movementTile.GridY - 1]);
            }
            // if this is first or later, but not last movement tile
            if (checkVerticalY >= 0 && checkVerticalY < gridSizeY-1)///right
            {
                // then we are sure to have a space for fence on it's right
                neighbours.Add(fenceGridVertical[movementTile.GridX, movementTile.GridY]);
            }
            // if this is first or later, but not last movement tile
            if (checkHorizontalX >= 0 && checkHorizontalX < gridSizeX-1)///up
            {
                // then we are sure to have a space for fence above it
                neighbours.Add(fenceGridHorizontal[movementTile.GridX, movementTile.GridY]);
            }
            return neighbours;
        }
	/// <summary>
	/// The point of this function is to return tiles, that current player can move to
	/// Player can only move vertically and horizontally
	/// Player cannot move to a tile if there is a fence between player and said tile
	/// Player cannot move back to the starting tile
	/// Player cannot jump outside of the grid
	/// If there is an opponent on one of the available tiles, player can jump to the tile behind the opponent, unless there is a fence inbetween
	/// If there is and opponent on the only available tile and the tile behind the opponent is unavailable, player can jump to opponent's either side (provided it's available)
	/// Pathfinding script ensures, that there is always a path between player and player's goal, so there is always at least one possible move
	/// </summary>
        public List<MovementTile> CalculateAvailableTiles(Vector3 playerPosition, Vector3 otherPlayerPosition)
        {
            MovementTile currentPlayerTile = TileFromWorldPoint(playerPosition);
            MovementTile otherPlayersTile = TileFromWorldPoint(otherPlayerPosition);

            List<MovementTile> neighbouringTiles = GetMovementNeighbours(currentPlayerTile);
            List<FenceTile> fences = GetFenceNeighbours(currentPlayerTile);
            List<MovementTile> checkForSpecialMovementTiles = new List<MovementTile>();
            List<MovementTile> availableTiles = new List<MovementTile>();

            bool specialMoveAvailable = false;

            Directions specialMovementDirection = Directions.Up;

            for (int i = 0; i < neighbouringTiles.Count; i++)
            {
                // if fence blocks path then continue
                if (!fences[i].isWalkable)
                {
                    continue;
                }
                // if starting tile then continue
                if (BallGameManager.Instance.isPlayerOneTurn && neighbouringTiles[i] == playerOneStartingTile || !BallGameManager.Instance.isPlayerOneTurn && neighbouringTiles[i] == playerTwoStartingTile)
                {
                    continue;
                }
                checkForSpecialMovementTiles.Add(neighbouringTiles[i]);
            }

            for (int i = 0; i < checkForSpecialMovementTiles.Count; i++)
            {
                if (checkForSpecialMovementTiles[i] == otherPlayersTile)
                {
                    specialMoveAvailable = true;

                    if (currentPlayerTile.GridX > otherPlayersTile.GridX)
                    {
                        specialMovementDirection = Directions.Down;
                    }
                    else if (currentPlayerTile.GridX < otherPlayersTile.GridX)
                    {
                        specialMovementDirection = Directions.Up;
                    }
                    else if (currentPlayerTile.GridY > otherPlayersTile.GridY)
                    {
                        specialMovementDirection = Directions.Left;
                    }
                    else if (currentPlayerTile.GridY < otherPlayersTile.GridY)
                    {
                        specialMovementDirection = Directions.Right;
                    }
                    continue;
                }
                availableTiles.Add(checkForSpecialMovementTiles[i]);
            }

            if (specialMoveAvailable)
            {
                neighbouringTiles = GetMovementNeighbours(otherPlayersTile);
                fences = GetFenceNeighbours(otherPlayersTile);
                checkForSpecialMovementTiles.Clear();

                for (int i = 0; i < neighbouringTiles.Count; i++)
                {
                    // if fence blocks path then continue
                    if (!fences[i].isWalkable)
                    {
                        continue;
                    }
		    // we don't want to include the tile, that player is standing on
                    if (neighbouringTiles[i] == currentPlayerTile)
                    {
                        continue;
                    }
                    // if starting tile than continue
                    if (BallGameManager.Instance.isPlayerOneTurn && neighbouringTiles[i] == playerOneStartingTile || !BallGameManager.Instance.isPlayerOneTurn && neighbouringTiles[i] == playerTwoStartingTile)
                    {
                        continue;
                    }
                    checkForSpecialMovementTiles.Add(neighbouringTiles[i]);
                }

                List<MovementTile> sideTiles = new List<MovementTile>();

		// make sure, that we don't move outside of grid
                for (int i = 0; i < checkForSpecialMovementTiles.Count; i++)
                {
                    if (specialMovementDirection == Directions.Up)
                    {
                        if (checkForSpecialMovementTiles[i].GridX > otherPlayersTile.GridX)
                        {
                            availableTiles.Add(checkForSpecialMovementTiles[i]);
                            break;
                        }
                        else
                        {
                            sideTiles.Add(checkForSpecialMovementTiles[i]);
                        }
                    }
                    else if (specialMovementDirection == Directions.Down)
                    {
                        if (checkForSpecialMovementTiles[i].GridX < otherPlayersTile.GridX)
                        {
                            availableTiles.Add(checkForSpecialMovementTiles[i]);
                            break;
                        }
                        else
                        {
                            sideTiles.Add(checkForSpecialMovementTiles[i]);
                        }
                    }
                    else if (specialMovementDirection == Directions.Left)
                    {
                        if (checkForSpecialMovementTiles[i].GridY < otherPlayersTile.GridY)
                        {
                            availableTiles.Add(checkForSpecialMovementTiles[i]);
                            break;
                        }
                        else
                        {
                            sideTiles.Add(checkForSpecialMovementTiles[i]);
                        }
                    }
                    else if (specialMovementDirection == Directions.Right)
                    {
                        if (checkForSpecialMovementTiles[i].GridY > otherPlayersTile.GridY)
                        {
                            availableTiles.Add(checkForSpecialMovementTiles[i]);
                            break;
                        }
                        else
                        {
                            sideTiles.Add(checkForSpecialMovementTiles[i]);
                        }
                    }
                }
		// if no other tiles are available, we can move to the opponent's side
                if (availableTiles.Count == 0)
                {
                    return sideTiles;
                }
            }
            return availableTiles;
        }
