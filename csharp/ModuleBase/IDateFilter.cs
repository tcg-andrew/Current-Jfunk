using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleBase
{
    public interface IDateFilter
    {
        DateTime To { get; set; }
        DateTime From { get; set; }
    }
}
