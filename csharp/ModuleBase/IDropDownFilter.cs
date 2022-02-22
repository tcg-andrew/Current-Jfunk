using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleBase
{
    public interface IDropDownFilter
    {
        Dictionary<string, string> SelectedFilter { get; set; }
        Dictionary<string, Dictionary<string, string>> Filters { get; }
    }
}
