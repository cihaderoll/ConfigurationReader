using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Common.Dtos
{
    public class OptionsDto
    {
        public string ApplicationName { get; set; }

        public string RefreshTimerIntervalInMs { get; set; }
    }
}
