using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teach.Models.Interfaces
{
    internal interface IParameterHandler
    {
        void Parse(string[] args);

        string GetValue(string name);

        bool HasParameter(string name);

        string GetDefaultValue(string name);
    }
}
