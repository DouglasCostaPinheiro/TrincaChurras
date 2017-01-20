using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrincaChurras.Support.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class GridIgnoreAttribute : Attribute
    {
    }
}