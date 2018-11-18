using Newtonsoft.Json;
using System;
using System.IO;

namespace ValidadorJson
{
    public class JsonFormatting
    {
        public static string Ident(string jsonRecebido)
        {
            using (var sr = new StringReader(jsonRecebido))
            using (var sw = new StringWriter())
            {
                var jr = new JsonTextReader(sr);
                var jw = new JsonTextWriter(sw) { Formatting = Formatting.Indented };
                jw.WriteToken(jr);
                return sw.ToString();
            }
        }
    }

}

