using System;
using UnityEngine;

namespace Akki.Nugmets
{
    public class FootballSpawner : MonoBehaviour
    {
        #region PUBLIC_VARIABLES

        public static event Action<Vector3> OnSpawnPlayer;

        public FootballHandler PlayerPrefab;
        public float PlayerSpeed;

        #endregion /PUBLIC_VARIABLES

        #region UNITY_CALLBACKS

        private void OnEnable()
        {
            OnSpawnPlayer += SpawnPlayer;
        }

        private void OnDisable()
        {
            OnSpawnPlayer -= SpawnPlayer;
        }

        #endregion /UNITY_CALLBACKS

        #region PUBLIC_METHODS

        public static void SpawnPlayerEvent(Vector3 direction)
        {
            if (OnSpawnPlayer != null)
            {
                OnSpawnPlayer.Invoke(direction);
            }
        }

        #endregion /PUBLIC_METHODS


        #region PRIVATE_METHODS

        private void SpawnPlayer(Vector3 direction)
        {
            FootballHandler handler = Instantiate(PlayerPrefab, transform.position, Quaternion.identity);

            handler.Direction = direction;
            handler.Speed = PlayerSpeed;
        }

        #endregion /PRIVATE_METHODS
    }
}
