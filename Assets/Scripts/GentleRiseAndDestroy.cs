using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class GentleRiseAndDestroy : MonoBehaviour
    {
        public float LifeSpan = 1f;
        public float Speed = 0.05f;
        private Transform _transform;

        void Start()
        {
            _transform = transform;
            Destroy(gameObject, LifeSpan);
        }

        void Update()
        {
            var move = Vector3.up * Time.deltaTime * Speed;
            _transform.position += move;
        }

    }
}
