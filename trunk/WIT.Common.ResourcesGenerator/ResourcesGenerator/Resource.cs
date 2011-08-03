using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourcesGenerator
{
    public class Resource
    {
        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        private Dictionary<string, string> resources;

        public Dictionary<string, string> Resources
        {
            get { return resources; }
            set { resources = value; }
        }

    }
}
