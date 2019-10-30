using System;
using System.Text;
using UnityEngine;

namespace Editor
{
    [Serializable]
    public sealed class PackageJson
    {
        public string name;
        public string displayName;
        public string version;
        public string unity;
        public string description;
        public string category;

        public override string ToString()
        {
            var builder = new StringBuilder(JsonUtility.ToJson(this));
            while (true)
            {
                var lastChar = builder[builder.Length - 1];
                builder.Length--;
                if (lastChar == '}')
                {
                    break;
                }
            }
            return builder.Append(",\"dependencies\":{\"nuget.mono-cecil\":\"0.1.6-preview\"}}").ToString();
        }
    }
    
}