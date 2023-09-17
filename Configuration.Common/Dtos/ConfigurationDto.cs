﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Common.Dtos
{
    public class ConfigurationDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public bool IsActive { get; set; }

        public string ApplicationName { get; set; }
    }
}
