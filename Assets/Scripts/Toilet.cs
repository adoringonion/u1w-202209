using UnityEngine;

public class Toilet : MonoBehaviour
{
        [SerializeField] private Transform landPos;
        [SerializeField] private Transform banishPos;

        public Vector3 LandPos => landPos.position;
        public Vector3 BanishPos => banishPos.position;
}