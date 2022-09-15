using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asos.CodeTest.Repository.Interface
{
    public  interface IFailoverRepository
    {
        List<FailoverEntry> GetFailOverEntries();
    }
}
