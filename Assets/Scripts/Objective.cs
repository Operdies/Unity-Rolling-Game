using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public static class Objective
    {
        public static int Collected = 0; 
        public static int TotalPickups => Collectibles.Count;
        public static List<Pickup> Collectibles = new List<Pickup>();
        public static void Register(Pickup p)
        {
            Collectibles.Add(p);
            OnRegister?.Invoke();
        }

        public static void Unregister(Pickup p)
        {
            Collectibles.Remove(p);
            OnRegister?.Invoke();
        }

        public static void PickupCollected(Pickup p)
        {
            Collected += 1;
            OnCollected?.Invoke();
            
            if (Collected >= TotalPickups)
                OnWin?.Invoke();
        }

        public static Action OnWin;
        public static Action OnCollected;
        public static Action OnRegister;

        public static void Reset()
        {
            Collected = 0;
            Collectibles = new List<Pickup>();
            OnWin = null;
            OnCollected = null;
            OnRegister = null;
        }
    }
}