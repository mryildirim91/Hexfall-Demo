using System;
using Mryildirim.ScriptableObjects;
using Mryildirim.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mryildirim.CoreGame
{
    public class GridGenerator : MonoBehaviour
    {
        private int _width, _height;
        private GameTile[,] _gameTiles;
        [SerializeField] private GameObject _gameTile, _matchDetector, _newTileSpawner;
        [SerializeField] private GridData _gridData;

        private void Awake()
        {
            _width = _gridData.Width;
            _height = _gridData.Height;
        }

        private void Start()
        {
            SpawnInitialTiles();
            SpawnMatchDetectors();
            SpawnNewTileSpawner();
        }
        
        private void SpawnInitialTiles()
        {
            var numberOfTypes = Enum.GetValues(typeof(GameTileType)).Length;
            
            var xOffset = (_width - 1) / 2.0f * (Mathf.Sqrt(3) / 2.0f);
            var yOffset = (_height - 1) / 2.0f;
            
            _gameTiles = new GameTile[_width, _height];

            for (int x = 0; x < _width; x++)
            {
                float heightShift = 0;
            
                if (x % 2 != 0)
                {
                    heightShift = 0.5f;
                }

                for (int y = 0; y < _height; y++)
                {
                    GameObject gameTileClone = ObjectPool.Instance.GetObject(_gameTile);
                    gameTileClone.transform.position = CalculateTilePosition(x, y, xOffset, yOffset, heightShift);;
                    
                    _gameTiles[x, y] = gameTileClone.GetComponent<GameTile>();
                    SetTilesInitially(numberOfTypes, x ,y);
                }
            }
            
            Tiles.TilesSetByGrid = true;
        }
    
        private Vector2 CalculateTilePosition(int x, int y, float xOffset, float yOffset, float heightShift)
        {
            return new Vector2(x * Mathf.Sqrt(3) / 2 - xOffset , y - heightShift - yOffset);
        }

        //This function stops tiles from matching in the beginning of the game.
        private void SetTilesInitially(int numberOfTypes, int x, int y)
        {
            int randomTileIndex;
                    
            randomTileIndex = x % 2 == 0 ? Random.Range(0, numberOfTypes - 4) : Random.Range(numberOfTypes - 4, numberOfTypes);
                    
            _gameTiles[x,y].SetTileType(randomTileIndex);
        }

        private void SpawnMatchDetectors()
        {
            const float verticalOffset = 0.5f;
            
            for (int y = 0; y < _height - 1; y++)
            {
                for (int x = 0; x < _width - 1; x++)
                {
                    var detectorClone1 = Instantiate(_matchDetector,transform);
                    detectorClone1.transform.position = CalculateDetectorPosition(x, y);

                    var horizontalOffset = 0.28f;

                    if (x % 2 == 0)
                    {
                        horizontalOffset *= -1;
                    }

                    var detectorClone2 = Instantiate(_matchDetector,transform);
                    detectorClone2.transform.position = detectorClone1.transform.position + new Vector3(horizontalOffset, verticalOffset);
                }
            }
        }

        private Vector2 CalculateDetectorPosition(int x, int y)
        {
            Vector2 position;
                    
            if (x % 2 == 0)
            {
                position = (_gameTiles[x, y].transform.position + _gameTiles[x + 1, y].transform.position +
                       _gameTiles[x + 1, y + 1].transform.position) / 3;
            }

            else
            {
                position = (_gameTiles[x, y].transform.position + _gameTiles[x , y + 1].transform.position +
                       _gameTiles[x + 1, y].transform.position) / 3;
            }

            return position;
        }

        private void SpawnNewTileSpawner()
        {
            for (int x = 0; x < _width; x++)
            {
                GameObject tileSpawner = Instantiate(_newTileSpawner,transform);
                tileSpawner.transform.position = CalculateNewTileSpawnerPosition(x);
            }
        }

        private Vector2 CalculateNewTileSpawnerPosition(int x)
        {
            var position = _gameTiles[x, 0].transform.position + Vector3.down;

            return position;
        }
    }
}
