using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Akki.Nugmets
{
    public class TrajectoryHandler : MonoBehaviour
    {
        #region PUBLIC_VARIALBES

        public Transform DotPrefab;
        public int NumberOfTrajectoryDots;
        public float offsetRotation;

        #endregion /PUBLIC_VARIALBES

        #region PRIVATE_VARIABLES

        private List<Transform> trajectoryDots = new List<Transform>();

        #endregion /PRIVATE_VARIABLES

        #region UNITY_CALLBACKS

        private void OnEnable()
        {
            SubscribeToInputManager();
        }

        private void Start()
        {
            for (int i = 0; i < NumberOfTrajectoryDots; i++)
            {
                Transform dot = Instantiate(DotPrefab, transform.position, Quaternion.identity);
                dot.gameObject.SetActive(false);
                trajectoryDots.Add(dot);
            }
        }

        private void OnDisable()
        {
            UnsubscribeToInputManager();
        }

        #endregion /UNITY_CALLBACKS

        #region PRIVATE_METHODS

        private void SubscribeToInputManager()
        {
            SwipeHandler.CanSwipe = true;
            SwipeHandler.OnSwipeStarted += StartFiring;
            SwipeHandler.OnSwipe += Fire;
            SwipeHandler.OnSwipeEnded += EndFiring;
        }

        private void UnsubscribeToInputManager()
        {
            SwipeHandler.CanSwipe = false;
            SwipeHandler.OnSwipeStarted -= StartFiring;
            SwipeHandler.OnSwipe -= Fire;
            SwipeHandler.OnSwipeEnded -= EndFiring;
        }

        private void StartFiring(Vector3 startPos)
        {
            foreach (Transform dot in trajectoryDots)
            {
                dot.position = transform.position;
                dot.gameObject.SetActive(true);
            }
        }

        private void Fire(Vector3 movement)
        {
            if (movement == Vector3.zero)
            {
                return;
            }

            if (movement.magnitude < 0.25f)
            {
                return;
            }

            SetTrajectoryPoints(transform.position, movement);
        }

        private void EndFiring(Vector3 movement)
        {
            foreach (Transform dot in trajectoryDots)
            {
                dot.gameObject.SetActive(false);
            }

            if (Vector3.Angle(movement, Vector3.up) < 90 - offsetRotation)
            {
                return;
            }

            if (movement == Vector3.zero)
            {
                return;
            }

            if (movement.magnitude < 0.25f)
            {
                return;
            }

            if (EventSystem.current.IsPointerOverGameObject(0))
            {
                return;
            }

            FootballSpawner.SpawnPlayerEvent(movement);
        }

        void SetTrajectoryPoints(Vector3 posStart, Vector2 direction)
        {
            if (Vector3.Angle(direction, Vector3.up) < 90 - offsetRotation)
            {
                foreach (Transform dot in trajectoryDots)
                {
                    dot.gameObject.SetActive(false);
                }
                return;
            }

            float velocity = Mathf.Sqrt((direction.x * direction.x) + (direction.y * direction.y));

            float angle = Mathf.Rad2Deg * (Mathf.Atan2(direction.y, direction.x));

            float fTime = 0;

            fTime += 0.1f;

            foreach (Transform dot in trajectoryDots)
            {
                float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
                float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * fTime * fTime / 2.0f);
                Vector3 pos = new Vector3(posStart.x - dx, posStart.y - dy, 0);
                dot.position = pos;
                dot.gameObject.SetActive(true);
                dot.eulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y - (Physics.gravity.magnitude) * fTime, direction.x) * Mathf.Rad2Deg);
                fTime += 0.1f;
            }
        }

        #endregion /PRIVATE_METHODS
    }
}
