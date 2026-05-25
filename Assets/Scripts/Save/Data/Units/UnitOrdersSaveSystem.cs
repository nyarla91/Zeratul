using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Save.Data.Units
{
    public class UnitOrdersSaveSystem : IUnitSaveSystem
    {
        public static string LoadKey => "orders";
        public string SaveKey => LoadKey;

        public OrderSaveData[] queue;

        public UnitOrdersSaveSystem(OrderSaveData[] queue)
        {
            this.queue = queue;
        }
    }
}