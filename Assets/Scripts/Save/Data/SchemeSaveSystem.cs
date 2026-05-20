using System.Collections.Generic;

namespace Save.Data
{
    public class SchemeSaveSystem : ISaveSystem
    {
        public static string LoadKey => "scheme";
        public string SaveKey => LoadKey;

        public Dictionary<string, string> variables;
        
        public SchemeSaveSystem() { }
        
        public SchemeSaveSystem(Dictionary<string, string> variables)
        {
            this.variables = variables;
        }
    }
}