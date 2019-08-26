using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CLF.Common.SecurityHelper
{
    public static class EmbeddedFileHelper
    {
        public static byte[] GetEmbeddedFileBytes(string name)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies.Where(x => x.FullName != null && !x.FullName.StartsWith("Anonymously")))
            {
                try
                {
                    using (Stream stream = assembly.GetManifestResourceStream(name))
                    {
                        byte[] bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, (int)stream.Length);
                        return bytes;
                    }
                }
                catch
                {
                }
            }
            return null;
        }

        public static List<string> GetEmbeddedFileContents(string name)
        {
            List<string> lines = new List<string>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies.Where(x => x.FullName != null && !x.FullName.StartsWith("Anonymously")))
            {
                try
                {
                    using (Stream stream = assembly.GetManifestResourceStream(name))
                    {
                        if (stream == null) continue;
                        StreamReader textStreamReader = new StreamReader(stream, new UTF8Encoding());
                        string line = textStreamReader.ReadLine();
                        while (line != null)
                        {
                            if (line.Trim().Length > 0)
                                lines.Add(line);
                            line = textStreamReader.ReadLine();
                        }
                    }
                }
                catch
                {
                }
            }
            return lines;
        }

        public static Stream GetEmbeddedFileStream(string name)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies.Where(x => x.FullName != null && !x.FullName.StartsWith("Anonymously")))
            {
                try
                {
                    Stream stream = assembly.GetManifestResourceStream(name);
                    return stream;
                }
                catch
                {
                }
            }
            return null;
        }

        public static string GetEmbeddedFileText(string name)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies.Where(x => x.FullName != null && !x.FullName.StartsWith("Anonymously")))
            {
                try
                {
                    using (Stream stream = assembly.GetManifestResourceStream(name))
                    {
                        StreamReader textStreamReader = new StreamReader(stream, new UTF8Encoding());
                        string text = textStreamReader.ReadToEnd();
                        return text;
                    }
                }
                catch
                {
                }
            }
            return null;
        }

        public static List<string> ToStatements(List<string> sqlContentLines)
        {
            StringComparison noCase = StringComparison.OrdinalIgnoreCase;

            List<string> statements = new List<string>();
            StringBuilder statementBuilder = new StringBuilder();
            for (int i = 0, count = sqlContentLines.Count; i < count; i++)
            {
                string line = sqlContentLines[i];
                if (line.Trim().Equals("GO", noCase))
                {
                    statements.Add(statementBuilder.ToString());
                    statementBuilder.Clear();
                }
                else
                {
                    statementBuilder.AppendLine(line);
                }
            }
            if (statementBuilder.Length > 0)
                statements.Add(statementBuilder.ToString());
            return statements;
        }
    }
}
