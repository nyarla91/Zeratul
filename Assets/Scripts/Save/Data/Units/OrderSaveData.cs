using System;
using Extentions;

namespace Save.Data.Units
{
    [Serializable]
    public class OrderSaveData
    {
        public string orderType;
        public int targetUnit;
        public SerializableVector2 targetPoint;

        public OrderSaveData(string orderType, int targetUnit, SerializableVector2 targetPoint)
        {
            this.orderType = orderType;
            this.targetUnit = targetUnit;
            this.targetPoint = targetPoint;
        }
    }
}