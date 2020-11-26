using UnityEngine;

namespace Akki.Nugmets
{
    public class FootballHandler : MonoBehaviour
    {
        #region PUBLIC_VARIABLES

        public Vector3 Direction { get; set; }

        public float Speed { get; set; }

        #endregion /PUBLIC_VARIABLES

        #region PRIVATE_VARIABLES

        private Vector3 startPosition;
        private bool move = true;

        #endregion /PRIVATE_VARIABLES

        #region UNITY_CALLBACKS

        private void Start()
        {
            startPosition = transform.position;
            Destroy(gameObject, 2f);
        }

        private void Update()
        {
            if (move)
            {
                SetFootballDirection();
            }
            else
            {
                transform.position = GetDeviateBallPosition();
            }
        }

        #endregion /UNITY_CALLBACKS

        #region PUBLIC_METHODS

        public void DeviateBall()
        {
            Direction = -Direction;
            move = false;
        }

        float dTime = 0;

        private Vector3 GetDeviateBallPosition()
        {
            float velocity = Mathf.Sqrt((Direction.x * Direction.x) + (Direction.y * Direction.y));

            velocity = velocity / 5;

            float angle = Mathf.Rad2Deg * (Mathf.Atan2(Direction.y, Direction.x));

            dTime += Time.deltaTime;

            float dx = velocity * dTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = velocity * dTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * dTime * dTime / 2.0f);
            Vector3 pos = new Vector3(transform.position.x + dx, transform.position.y + dy, -dy);

            transform.localScale += Vector3.one * 0.7f * Time.deltaTime / Speed;

            return pos;
        }

        #endregion /PUBLIC_METHODS  

        #region PRIVATE_METHODS

        bool scored;

        private void SetFootballDirection()
        {
            transform.position = GetCurvePosition();
        }

        float fTime = 0;

        private Vector3 GetCurvePosition()
        {
            float velocity = Mathf.Sqrt((Direction.x * Direction.x) + (Direction.y * Direction.y));

            float angle = Mathf.Rad2Deg * (Mathf.Atan2(Direction.y, Direction.x));

            fTime += Time.deltaTime * Speed;

            float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * fTime * fTime / 2.0f);
            Vector3 pos = new Vector3(startPosition.x - dx, startPosition.y - dy, startPosition.y - dy);

            //transform.localScale -= Vector3.one * Time.deltaTime / Speed * 1.5f;

            return pos;
        }

        #endregion /PRIVATE_METHODS
    }
}
