using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.ConnectSdk.Scripts.Unity
{
    class TimedQueue<T>
    {
        private readonly Queue<T> _queue = new Queue<T>();
        public event EventHandler Enqueued;
        public event EventHandler Elapsed;
        private UnityTimer _timer;


        private GameObject _connectGameObject;

        public TimedQueue(float interval = 5f)
        {
            _timer = ConnectGameObject.AddComponent<UnityTimer>();
            _timer.enabled = false;
            _timer.Interval = interval;
            _timer.Elapsed += OnElapsed;
        }

        public List<T> ToList() {
            return new List<T>(_queue);
        }

        public void Clear() {
            _queue.Clear();
        }

        private GameObject ConnectGameObject
        {
            get { return _connectGameObject ?? (_connectGameObject = GameObject.FindWithTag("ConnectTracker")); }
        }

        protected virtual void OnEnqueued()
        {
            if (Enqueued != null)
                Enqueued(this, new EventArgs());
        }

        protected virtual void OnElapsed(object sender, EventArgs eventArgs)
        {
            ConnectLog.Write("Queue timer elapsed");
            if (Elapsed != null)
                Elapsed(this, new EventArgs());
        }

        public bool Enabled
        {
            get { return _timer.enabled; }
            set
            {
                var message = value ? "Enabling" : "Disabling";
                ConnectLog.Write(string.Format("{0} timer", message));
                _timer.enabled = value;
            }
        }

        public int Count
        {
            get { return _queue.Count; }
        }

        public virtual void Enqueue(T item)
        {
            _queue.Enqueue(item);
            OnEnqueued();
        }

        public virtual T Dequeue()
        {
            var item = _queue.Dequeue();
            return item;
        }
    }
}

