using System;
using CsExtensions;
using UnityEngine;

namespace Unity.Utils {
    public class DisposableBehaviour : MonoBehaviour, IDisposableCollection {
        private readonly DisposableCollection _disposableCollection = new DisposableCollection();

        public void Retain(Action dispose) {
            _disposableCollection.Retain(dispose);
        }

        protected virtual void OnDestroy() {
            Dispose();
        }

        public void Dispose() {
            _disposableCollection.Dispose();
        }
    }
}