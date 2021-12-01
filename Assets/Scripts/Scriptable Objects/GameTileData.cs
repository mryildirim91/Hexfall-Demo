using UnityEngine;

namespace Mryildirim.ScriptableObjects
{
    [CreateAssetMenu(menuName = "New Tile/Game Tile", fileName = "Game Tile")]
    public class GameTileData : ScriptableObject
    {
        [SerializeField] private GameTileType _gameTileType;
        [SerializeField] private Color _tileColor;
    
        public GameTileType GameTileType => _gameTileType;
        public Color TileColor => _tileColor;
    }

    public enum GameTileType
    {
        Blue,
        Cyan,
        Green,
        Magenta,
        Orange,
        Red,
        Yellow
    }
}