using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class DataItem : IData
    {
        public int itemID;
        public string name;
        public int itemType;
        public string itemIconPackage;
        public string itemIcon;
        public string itemOnWorldPackage;
        public string itemOnWorldSprite;
        public string itemDescription;
        public int itemUseRadiue;
        public bool canPickedup;
        public bool canDropped;
        public bool canCarried;
        public int itemPrice;
        public float sellPercentage;

        public int GetId()
        {
            return itemID;
        }
    }
