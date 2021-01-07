using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace YAMLCheckerWin
{
    [Checker("*.meta")]
    public class MetaChecker : BaseChecker
    {
        public MetaChecker(string _root, string _pattern, bool _printLog=false) : base(_root, _pattern, _printLog) { 
        }
        public override void Execute()
        {
            var printlog = false;
            var fildPathDict = new Dictionary<string, string>();
            for (int i = 0; i < files.Length; i++)
            {
                Dictionary<string, object> raw = null;

                var allText = File.ReadAllText(files[i]);

                var input = new StringReader(allText);

                try
                {
                    var deserializer = new DeserializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance)
                        .Build();
                    raw = deserializer.Deserialize<Dictionary<string, object>>(input);
                }
                catch (SyntaxErrorException e)
                {
                    if (printlog)
                    {
                        Console.WriteLine($"SyntaxErrorException ConflictError:{files[i]}");
                    }

                    continue;
                }
                catch (YamlException e)
                {
                    ProcessLog($"文件本身存在冲突没有解决\n{files[i]}");
                    continue;
                }
                catch (Exception e)
                {
                    if (printlog)
                    {
                        Console.WriteLine($"Exception ConflictError:{files[i]} {e.ToString()}");
                    }
                    continue;
                }

                var items = raw["guid"];
                if (fildPathDict.TryGetValue(items.ToString(), out var value))
                {
                    ProcessLog($"{value}\nconfitct with:\n{files[i]}\nguid:{items}\n");
                    if (printlog)
                    {
                        Console.WriteLine($"hasRepeatError:{files[i]}");
                    }
                }
                else
                {
                    fildPathDict.Add(items.ToString(), files[i]);
                }
            }
        }
    }
}
