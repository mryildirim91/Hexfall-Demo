using Mryildirim.Utilities;
using UnityEngine;

namespace Mryildirim.CoreGame
{
    public class Outline : MonoBehaviour
    {
        [SerializeField]private GameObject[] _outlines;
        
        private void OnEnable()
        {
            EventManager.OnTileSelected += SetOutlines;
        }

        private void OnDisable()
        {
            EventManager.OnTileSelected -= SetOutlines;
        }

        private void SetOutlines()
        {
            transform.position = Tiles.CalculateSelectedTilesCenter();
            
            ToggleOutlines(true);
            
            for (int i = 0; i < _outlines.Length; i++)
            {
                _outlines[i].transform.position = Tiles.SelectedTiles[i].transform.position;
            }
            
            Tiles.SetSelectedTilesParent(transform);
        }

        public void ToggleOutlines(bool enable)
        {
            foreach (var t in _outlines)
            {
                t.SetActive(enable);
            }
        }
        
    }
}
