using System;

namespace Mryildirim.Utilities
{
    public static class EventManager
    {
        public static Action OnTileSelected;
        public static Action OnTileMatch;
        public static Action OnTilesDestroyed;

        public static void TriggerTileSelected()
        {
            OnTileSelected?.Invoke();
        }

        public static void TriggerTileMatch()
        {
            OnTileMatch?.Invoke();
        }

        public static void TriggerTilesDestroyed()
        {
            OnTilesDestroyed?.Invoke();
        }
    }
}
