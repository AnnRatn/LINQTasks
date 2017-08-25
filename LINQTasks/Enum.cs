using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQTasks
{
    public enum UserType
    {
        Manager,
        Developer,
        Tester,
    }

    public enum Status
    {
        Created,
        Fixed,
        Closed,
    }

    public enum Priority
    {
        Minor,
        Normal,
        Critilal,
    }
}