using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildTray.Logic.Entities
{
    [Serializable]
    public class BuildInfo
    {
        [NonSerialized]
        private Uri _serverUrl;

        private string _serverUrlString;

        public string BuildName { get; set; }
        public string ProjectName { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public Uri ServerUrl 
        { 
            get 
            { 
                return _serverUrl;
            }
            set
            {
                 _serverUrl = value;
                 _serverUrlString = _serverUrl.AbsoluteUri;
            }
        }

        public string ServerUrlString 
        {
            get { return _serverUrlString; }
            set 
            { 
                _serverUrlString = value;
                _serverUrl = new Uri(value);
            }
        }
    }
}
