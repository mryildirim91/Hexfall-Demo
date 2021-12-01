using System.Collections.Generic;
using UnityEngine;

namespace Mryildirim.CoreGame
{
    public static class Tiles
    {
        public static bool TilesSetByGrid { get; set; }
        public static readonly List<SelectableTile> SelectedTiles = new List<SelectableTile>();
        
        public static void SetSelectedTilesParent(Transform parent)
        {
            foreach (var tile in SelectedTiles)
            {
                if(tile)
                    tile.transform.SetParent(parent);
            }
        }
        public static Vector2 CalculateSelectedTilesCenter()
        {
            var position = Vector2.zero;

            foreach (var tile in SelectedTiles)
            {
                if (tile)
                {
                    Vector2 tilePosition = tile.transform.position;
                    position += tilePosition / 3;
                }
            }

            return position;
        }
    }
}
