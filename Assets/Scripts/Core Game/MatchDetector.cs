using Mryildirim.Utilities;
using UnityEngine;

namespace Mryildirim.CoreGame
{
    public class MatchDetector : MonoBehaviour
    {
        private const int MatchNumber = 3;
        private bool _detected;
        private readonly GameTile[] _gameTiles = new GameTile[3];
        private readonly RaycastHit2D[] _hits = new RaycastHit2D[3];
        [SerializeField] private LayerMask _gameTileLayer;

        private void Update()
        {
            DetectTileMatch();
        }

        private void DetectTileMatch()
        {
            var size = Physics2D.CircleCastNonAlloc(transform.position, 0.25f, Vector2.one, _hits, 0.25f,_gameTileLayer);

            if (size != MatchNumber)
            {
                _detected = false;
                return;
            }

            if (_detected) return;
        
            _detected = true;

            for (int i = 0; i < size; i++)
            {
                _gameTiles[i] = _hits[i].transform.GetComponent<GameTile>();
            }
            for (int i = 0; i < MatchNumber - 2; i++)
            {
                if (_gameTiles[i].GameTileType == _gameTiles[i + 1].GameTileType && _gameTiles[i].GameTileType == _gameTiles[i + 2].GameTileType)
                {
                    EventManager.TriggerTileMatch();
                    Invoke(nameof(DestroyMatchedTiles), 0.5f);
                }
            }
        }

        private void DestroyMatchedTiles()
        {
            for (int i = 0; i < MatchNumber; i++)
            {
                if(_gameTiles[i].transform.parent)
                    _gameTiles[i].transform.SetParent(null);
                
                ObjectPool.Instance.ReturnGameObject(_gameTiles[i].gameObject);
            }

            Tiles.SetSelectedTilesParent(null);
            if (Tiles.SelectedTiles.Count > 0)
                Tiles.SelectedTiles.Clear();
            
            Invoke(nameof(DelayTriggerTilesDestroyed),0.1f);
        }

        private void DelayTriggerTilesDestroyed()
        {
            EventManager.TriggerTilesDestroyed();
        }
    }
}
