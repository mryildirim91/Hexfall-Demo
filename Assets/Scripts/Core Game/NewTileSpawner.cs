using System.Collections;
using DG.Tweening;
using Mryildirim.ScriptableObjects;
using Mryildirim.Utilities;
using UnityEngine;

namespace Mryildirim.CoreGame
{
    public class NewTileSpawner : MonoBehaviour
    {
        private RaycastHit2D[] _hits;
        [SerializeField] private LayerMask _gameTileLayer;
        [SerializeField] private GameObject _gameTile;
        [SerializeField] private GridData _gridData;

        private void Start()
        {
            _hits = new RaycastHit2D[_gridData.Height];

            EventManager.OnTilesDestroyed += CountTilesInColumn;
        }
        
        private void OnDestroy()
        {
            EventManager.OnTilesDestroyed -= CountTilesInColumn;
        }
        
       
        private void CountTilesInColumn()
        {
            StartCoroutine(CountTilesInColumnRoutine());
        }
        
        //Counts the number of tiles in each column by casting ray from the bottom
        private IEnumerator CountTilesInColumnRoutine()
        {
            var hitSize = Physics2D.RaycastNonAlloc(transform.position, Vector2.up, _hits, Mathf.Infinity,_gameTileLayer);
            
            if (hitSize < _gridData.Height)
            {
               StartCoroutine(MoveTilesRoutine(hitSize));
                
               yield return new WaitForSeconds(0.3f);
               
                for (int i = 0; i < _gridData.Height - hitSize; i++)
                {
                    SpawnNewGameTile(i);
                }
                yield return new WaitForSeconds(0.2f);
                hitSize = Physics2D.RaycastNonAlloc(transform.position, Vector2.up, _hits, Mathf.Infinity,_gameTileLayer);;
                StartCoroutine(MoveTilesRoutine(hitSize));
            }
        }
        
        private void SpawnNewGameTile(int y)
        {
            var gameTile = ObjectPool.Instance.GetObject(_gameTile);
            gameTile.transform.position = transform.position + Vector3.up * 20 * (y+1);
        }
        
        //Moves tiles down if there is gap between them
        private IEnumerator MoveTilesRoutine(int size)
        {
            for (int i = 0; i < size - 1; i++)
            {
                if (Vector2.Distance(_hits[0].transform.position, transform.position) > 1)
                {
                    MoveTilesDown(transform.position,0,0f);
                }
                
                yield return new WaitForSeconds(0.05f);
                
                if (Vector2.Distance(_hits[i + 1].transform.position, _hits[i].transform.position) > 1)
                {
                    MoveTilesDown(_hits[i].transform.position,i + 1, 0f);
                }
            }
        }

        private void MoveTilesDown(Vector3 position, int index, float duration)
        {
            PolygonCollider2D col = _hits[index].transform.GetComponent<PolygonCollider2D>();
            col.enabled = false;
            _hits[index].transform.DOMove(position + Vector3.up, duration).SetEase(Ease.Linear).OnComplete(()=>col.enabled = true);
        }
    }
}
