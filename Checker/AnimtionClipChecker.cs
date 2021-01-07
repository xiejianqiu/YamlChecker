using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace YAMLCheckerWin
{
    [Checker("*.anim")]
    class AnimtionClipChecker : BaseChecker
    {
        public AnimtionClipChecker(string _root, string _pattern, bool _printLog=false) : base(_root, _pattern, _printLog)
        {
        }

        public override void Execute()
        {
            for (int index = 0; index < files.Length; index++)
            {
                var allText = File.ReadAllText(files[index]);

                // Setup the input
                var input = new StringReader(allText);

                // Load the stream
                var yaml = new YamlStream();
                try
                {
                    yaml.Load(input);

                    var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

                    if (mapping.Children.ContainsKey("AnimationClip"))
                    {
                        mapping = (YamlMappingNode)mapping.Children[new YamlScalarNode("AnimationClip")];
                        var items = (YamlScalarNode)mapping.Children[new YamlScalarNode("m_Name")];
                        string fileName = Path.GetFileNameWithoutExtension(files[index]);
                        if (fileName != items.ToString())
                        {
                            ProcessLog($"{files[index]}");
                            if (mPrintLog)
                            {
                                Console.WriteLine($"anim {files[index]}");
                            }
                        }
                    }
                }
                catch (YamlDotNet.Core.SyntaxErrorException e)
                {
                    if (mPrintLog)
                    {
                        //Console.WriteLine($"SyntaxErrorException ConflictError:{files[index]}");
                    }
                    continue;
                }
                catch (YamlException e)
                {
                    if (mPrintLog)
                    {
                        //Console.WriteLine($"YamlException ConflictError:{files[index]}");
                    }
                    continue;
                }
                catch (Exception e)
                {
                    if (mPrintLog)
                    {
                        //Console.WriteLine($"Exception ConflictError:{files[index]} {e.ToString()}");
                    }
                    continue;
                }
            }
        }

        
    }
}
