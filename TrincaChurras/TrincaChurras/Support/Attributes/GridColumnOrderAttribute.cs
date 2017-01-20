using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrincaChurras.Support.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GridColumnOrderAttribute : Attribute
    {
        public string[] Order;

        public GridColumnOrderAttribute(params string[] pOrder)
        {
            Order = pOrder;
        }
    }
}