using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoGambling.Data
{
    public interface IDataContextFactory
    {
        InoGamblingDbContext GetDbContext();
    }
}
