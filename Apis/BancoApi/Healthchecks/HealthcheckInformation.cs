using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoApi.Healthchecks
{
    public class HealthcheckInformation
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Data { get; set; }
        public string Status { get; set; }

        public HealthcheckInformation() { }

    }
}
