using System;
using Mryildirim.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mryildirim.CoreGame
{
    public class GameTile : MonoBehaviour
    {
        private GameTileType _gameTileType;
        public GameTileType GameTileType => _gameTileType;

        private SpriteRenderer _spriteRenderer;
        [SerializeField] private GameTileData[] _gameTileDatas;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            if(!Tiles.TilesSetByGrid) return;
                
            var randomTileIndex = Random.Range(0, _gameTileDatas.Length);
            SetTileType(randomTileIndex);
        }

        private void OnDisable()
        {
            UIManager.Instance.UpdateScore(5);
        }

        public  void SetTileType(int randomTileIndex)
        {
            GameTileData data = _gameTileDatas[randomTileIndex];
            _gameTileType = data.GameTileType;
            _spriteRenderer.color = data.TileColor;
        }
    }
}
