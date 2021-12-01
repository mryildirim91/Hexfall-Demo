using System.Collections.Generic;
using Mryildirim.Utilities;
using UnityEngine;

namespace Mryildirim.CoreGame
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class SelectableTile : MonoBehaviour
    {
        [SerializeField]private LayerMask _gameTileLayerMask;
        private PolygonCollider2D _polygonCollider2D;
        private List<SelectableTile> _adjacentTiles = new List<SelectableTile>();

        private void Awake()
        {
            _polygonCollider2D = GetComponent<PolygonCollider2D>();
        }
        
        private void OnMouseDown()
        {
            DetectAdjacentTiles();
            SelectRandomTiles();
            EventManager.TriggerTileSelected();
        }

        private void DetectAdjacentTiles()
        {
            _polygonCollider2D.enabled = false;
            
            if(_adjacentTiles.Count > 0)
                _adjacentTiles.Clear();
            
            const int numOfRayDirections = 6;
            
            float angleInDegrees = 0;
            
            for (int i = 0; i < numOfRayDirections; i++)
            {
                var angleInRadian = Mathf.Deg2Rad * angleInDegrees;
                var direction = new Vector2(Mathf.Sin(angleInRadian), Mathf.Cos(angleInRadian));
                angleInDegrees += 60;
                
                var hit2D = Physics2D.Raycast(transform.position, direction, 1, _gameTileLayerMask);

                if (hit2D)
                {
                    var selectableTile = hit2D.transform.GetComponent<SelectableTile>();
                    _adjacentTiles.Add(selectableTile);
                }

            }
            _polygonCollider2D.enabled = true;
        }

        private void SelectRandomTiles()
        {
            int randomAdjacentTileIndex;
            float adjacentDistance;
            
            do
            {
                randomAdjacentTileIndex = Random.Range(0, _adjacentTiles.Count - 1);
                adjacentDistance = Vector2.Distance(_adjacentTiles[randomAdjacentTileIndex].transform.position, 
                    _adjacentTiles[randomAdjacentTileIndex + 1].transform.position);
            } while (adjacentDistance > 1);

            if (Tiles.SelectedTiles.Count > 0)
            {
                Tiles.SetSelectedTilesParent(null);
                Tiles.SelectedTiles.Clear();
            }
            
            Tiles.SelectedTiles.Add(this);
            
            for (int i = 0; i < 2; i++)
            {
                Tiles.SelectedTiles.Add(_adjacentTiles[randomAdjacentTileIndex + i]);
            }
        }
    }
}
