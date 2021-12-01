using UnityEngine;

namespace Mryildirim.ScriptableObjects
{
    [CreateAssetMenu(menuName = "New Grid", fileName = "Grid")]
    public class GridData : ScriptableObject
    {
        [SerializeField]private int _width, _height;

        public int Width => _width;
        public int Height => _height;
    }
}
