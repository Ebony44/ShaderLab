using System;
using UnityEngine;

namespace Akki.Nugmets
{
    public class SwipeHandler : MonoBehaviour
    {
        public static bool CanSwipe { get; set; }

        public static event Action<Vector3> OnSwipeStarted;
        public static event Action<Vector3> OnSwipe;
        public static event Action<Vector3> OnSwipeEnded;

        private Vector3 startPosition;

        void Update()
        {
            if (CanSwipe)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    if (OnSwipeStarted != null)
                    {
                        OnSwipeStarted.Invoke(startPosition);
                    }
                }
                else if (Input.GetMouseButton(0))
                {
                    Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 movement = startPosition - currentPosition;

                    if (movement != Vector3.zero)
                    {
                        if (OnSwipe != null)
                        {
                            OnSwipe.Invoke(movement);
                        }
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 movement = startPosition - currentPosition;

                    if (OnSwipeEnded != null)
                    {
                        OnSwipeEnded.Invoke(movement);
                    }
                }
            }
        }
    }
}
