/*
 Copyright 2021 Fusen
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using FastColoredTextBoxNS;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;

namespace CNote
{

    public class CNoteUtils
    {

        //Python Custom HighLight Colors
        public TextStyle greenstyle = new TextStyle(Brushes.Green, null, FontStyle.Bold);
        public TextStyle graystyle = new TextStyle(Brushes.Gray, null, FontStyle.Bold);
        public TextStyle pyPurple = new TextStyle(Brushes.MediumPurple, null, FontStyle.Bold);
        public TextStyle pyOrange = new TextStyle(Brushes.Orange, null, FontStyle.Bold);
        public TextStyle pyOrangeRed = new TextStyle(Brushes.OrangeRed, null, FontStyle.Bold);
        public TextStyle pyBlue = new TextStyle(Brushes.CornflowerBlue, null, FontStyle.Bold);
        public TextStyle pyLightGreen = new TextStyle(Brushes.DarkCyan, null, FontStyle.Bold);
        public TextStyle pyclassorange = new TextStyle(Brushes.Orange, null, FontStyle.Bold);
        public TextStyle pymultiGreen = new TextStyle(Brushes.Green, null, FontStyle.Bold);
        public TextStyle pyfuncParam = new TextStyle(Brushes.SandyBrown, null, FontStyle.Bold);
        public TextStyle pynums = new TextStyle(Brushes.DarkOrange, null, FontStyle.Bold);
        public TextStyle pyops = new TextStyle(Brushes.Coral, null, FontStyle.Bold);
        // .................................///

        public string allFileFilter = "Any File|*.*| Python File |*.py|CSharp File |*.cs| HTML File |*.html | PHP File |*.php|XML File |*.xml|SQL File |*.sql|LUA File |*.lua|JavaScript File |*.js*";

        //Python Custom HighLight Regex
        public string pyHLmains = @"\b(del|from|not|while|as|elif|or|with|assert|else|if|pass|yield|break|except|import|print|class|raise|continue|finally|return|def|for|lambda|try|as|globals|dict|list|sorted|format|filter|float|int|exec|except|bool|ord|all|as|)\b";
        public string pyHLSecOrange = @"\b(and|is|in|or)\b";
        public string pyHLSecOrangeRed = @"\b(True|False|global|self(\.?))\b";
        public string pyHLparams = @"(?<=\()[a-zA-Z_0-9, \n.'"":+-/%*=\{\}\[\]]+?(?=\))";
        public string pyHLnums = @"[0-9]+";
        public string pyHLops = @"[=+/%*-]+";
        public string pyHLSecFuncs = @"\b(abs|pow|ord|open|eval|repr|reversed|round|set|setattr|format|slice|sorted|object|staticmethod|str|sum|tuple|str|bin|bytes|bytearray|hex|input|min|max|next|zip|len|map|types|vars|range|enumerate|round|compile|memroyview|del|group|super|__(\w+)__)\b";
        public string pyHLstr = "(\'(.*?)\'|\"(.*?)\")"; // py capturign string regex
        // ...................................///


        //C++ Custom HighLight Regex
     
        public string CHLmains = @"\b(enum|while|with|else|if|yield|break|catch|printf|class|throw|continue|finally|return|void|for|try|list|string|uint|double|float|int|char|bool|extern|long|wchar_t|using|namespace|std)\b";
        public string CHLSecOrange = @"\b(and)\b";
        public string CHLSecOrangeRed = @"\b(true|false)\b";
        public string CHLparams = @"(?<=\()[a-zA-Z_0-9, \n.'"":+-/%*=\{\}\[\]]+?(?=\))";
        public string CHLnums = @"[0-9]+";
        public string CHLops = @"[=+/%*-]+";
        public string CHLSecFuncs = @"\b(\w+\(\));?";
        public string CHLinclues = @"#[a-zA-Z_<>\""]+";
        public string CHLstr = "(\'(.*?)\'|\"(.*?)\")"; // c++/c capturign string regex
        // ...................................///




        //CSharp Autocomplete items
        public string[] cshapr_items = {"class", "private", "public", "protected", "var", "string", "int", "float", "double", "Double", "uint", "int32", "byte", "sbyte", "short", "ushort", "long", "ulong", "decimal", "char", "bool", "DateTime", "void", "return", "for", "foreach", "while", "null", "IDictionary", "Dictionary", "Stack", "Hashtable", "delegate", "static", "Program", "Stream", "FileStream", "MemoryStream", "NetworkStream", "PipeStream", "CryptoStream", "StreamWriter", "StreamReader", "BinaryWriter",
                                     "Environment", "Append", "remove", "Open", "ReadAllLines", "Replace", "File", "Directory", "Copy", "try", "catch", "namespace", "Array", "using", "System", "List", "args", "interface", "Enum", "async", "await", "Console", "Write", "decimal", "EventArgs", "object", "WriteLine", "Read", "ReadLine", "ReadKey();", "Error", "Clear", "in", "ToString();", "List<>", "string[]", "int[]", "float[]", "ForEach", "ToUpper();", "ToLower();", "Trim", "ToCharArray",
                                    "Substring", "StartsWith", "Split", "EndsWith", "Square", "new", "object", "true", "false", "Vector", "Main", "Boolean", "Byte", "Char", "Double", "Int16", "Int32", "Int64", "IntPtr", "SByte", "Single", "UInt16", "UInt32", "UInt64", "UIntPtr", "bool", "byte", "char", "double", "short", "long", "IntPtr", "sbyte", "float", "ushort", "uint", "ulong", "ForEach", "Console.WriteLine();", "using System.Linq;", "using System","using System.Threading;",
                                    "using System.IO;", "using System.Collections.Generic;", "using System.Collections;", "using System.Diagnostics;", "using System.Drawing;", "using System.Windows", "Regex", "switch", "case", "default", "break", "Process", "forr", "cmain"};



        public string[] cpp_items = {"include <>","define", "error" ,"import", "pragma", "elif", "if", "undef", "else", "ifdef", "line", "endif", "ifndef" , "extern", "class", "private", "public", "protected","string", "int", "float", "double","wchar_t", "unsigned", "signed", "uint","short", "ushort", "long", "typedef", "decimal", "char", "bool", "void", "return", "for", "while", "static", "try", "catch", "namespace", "using", "argc","argv","argv[]", "enum", "list", "string[]", "int[]", "float[]",
                                     "new", "true", "false", "Main", "Boolean", "Byte", "bool", "byte", "char", "double", "short", "long", "float", "ushort", "uint","switch", "case", "default", "break;", "iostream", "std","std::", "std::cout << << std::endl;", "cout << << endl;", "cin >>", "std::cin >>", ""};

        

        //Python Autocomplete items
        public string[] python_items = { "import", "from", "if", "else", "while", "True", "False", "pass", "global", "as", "not", "del", "break", "print", "class", "continue", "raise", "return", "finally", "def", "lambda", "try", "except", "yield", "assert", "with", "open", "as", "in", "and", "or",
                                        "abs()","bytes","classmethod","chr","dict","compile","dir","eval","enumerate","exec","filter","float","format","forzenset","bytearray()","bool","bin","ascii","any","all","getattr","globals","hasattr","hex","input","int","isinstance","list","len()","iter","locals","map","max",
                                        "memroyview","min","next()","object","oct","ord","pow","math","property","range","repr","reversed","round","set","setattr","slice","sorted()","staticmethod","str","sum","super","tuple","types","vars","zip","__init__(self)","init","__repr__","__doc__","__getitem__","__module__","__name__","__file__","__dict__","__package__","__annotations__",
                                         "__slots__","__hash__","__nonzero__","__bool__","__next__","__qualname__","__defaults__","__code__","__globals__","__closure__","__kwdefaults__","__new__","__del__","__str__","","__bytes__","__format__","__lt__","__le__","__eq__","__ne__","__gt__","__ge__","__add__","__sub__","__mul__","__matmul__","__truediv__","__floordiv__","__abs__","__float__",
                                          "__complex__","__round__","__trunc__","__ceil__","__floor__","__len__","__setitem__","__delitem__","__iter__","__floordiv__","__mod__","__truediv__","__pow__","__lshift__","__rshift__","__and__","__xor__","__or__","__iadd__","__isub__","__imul__","__idiv__","__ifloordiv__","__imod__","__ipow__","__iand__","__neg__","__pos__","__invert__","__int__","__long__","__float__","__oct__","__hex__","__main__","ifmain",
                                           "match", "case", "re", "sys", "os","forrange"};



        //HTML Autocomplete items
        public string[] html_items = { "head", "div" , "html","img", "src=", "rel", "html", "class=\"\"", "!DOCTYPE" ,"a","adress","applet","area","article","aside","audio","embed","object","audio","b","base","basefont","bdi","bdo","big","blockquote","body","br","button","canvas","caption","center","cite","code",
                                        "col=","colgroup","data=","datalist","dd","del","details","dfn","dialog","dir","ul","dt","em","fieldset","figcaption","figure","font","footer","form","frame","frameset","h1","h2","h3","h4","h5","h6","header","hr","i","iframe","input","ins","kbd","label","legend","li",
                                         "link","main","map","mark","meta","nav","noframes","noscript","ol","optgroup","option","output","p","param","picture","pre","progress","q","rp","rt","ruby","s","samp","script","section","select","small","source","span","strike","strong","style=","sub","svg","table","tbody",
                                        "td","template","textarea","tfoot","thread","title","tr","track","tt","u","ul","var","video","wbr","alt=","width=","height=","ismap","longdesc","loading","sizes","srcset","usemap=","autoplay","controls","loop","muted","poster","preload","autofocus","disabled","formaction","formenctype",
                                         "formmethod","formnovalidate","formtarget","name=","type=","value=","id=\"\"","for=","method=","action=\"\"","checked=","autocomplete=","row=","colspan=","size=","wrap=","hard","href=","cords=","shape=","step=","start=","target=","sandbox","rowspan="};


        public void init()
        {

        }



        //ADD or Update settings
        public void AOUSettings(string key, string value)
        {

            try
            {
           
                
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;

                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

      
        //Get Settings
        public string GetSettings(string key)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            if (settings[key] == null)
            {
                return null;
            }
            else
            {
                return settings[key].Value.ToString();
            }
        }

        //Get Python path if installed
        public string GetPythonPath(string requiredVersion = "", string maxVersion = "")
        {
            string[] possiblePythonLocations = new string[3] {
                @"HKLM\SOFTWARE\Python\PythonCore\",
                @"HKCU\SOFTWARE\Python\PythonCore\",
                @"HKLM\SOFTWARE\Wow6432Node\Python\PythonCore\"
            };

            //Version number, install path
            Dictionary<string, string> pythonLocations = new Dictionary<string, string>();

            foreach (string possibleLocation in possiblePythonLocations)
            {
                string regKey = possibleLocation.Substring(0, 4), actualPath = possibleLocation.Substring(5);
                RegistryKey theKey = (regKey == "HKLM" ? Registry.LocalMachine : Registry.CurrentUser);
                RegistryKey theValue = theKey.OpenSubKey(actualPath);

                if (theValue != null)
                {
                    foreach (var v in theValue.GetSubKeyNames())
                    {
                        RegistryKey productKey = theValue.OpenSubKey(v);
                        if (productKey != null)
                        {
                            try
                            {
                                string pythonExePath = productKey.OpenSubKey("InstallPath").GetValue("ExecutablePath").ToString();
                                if (pythonExePath != null && pythonExePath != "")
                                {
                                    //Console.WriteLine("Got python version; " + v + " at path; " + pythonExePath);
                                    pythonLocations.Add(v.ToString(), pythonExePath);
                                }
                            }
                            catch
                            {
                                //Install path doesn't exist
                            }
                        }
                    }
                }
                


            }

            if (pythonLocations.Count > 0)
            {
                System.Version desiredVersion = new System.Version(requiredVersion == "" ? "0.0.1" : requiredVersion),
                    maxPVersion = new System.Version(maxVersion == "" ? "999.999.999" : maxVersion);

                string highestVersion = "", highestVersionPath = "";

                foreach (KeyValuePair<string, string> pVersion in pythonLocations)
                {
                    //TODO; if on 64-bit machine, prefer the 64 bit version over 32 and vice versa
                    int index = pVersion.Key.IndexOf("-"); //For x-32 and x-64 in version numbers
                    string formattedVersion = index > 0 ? pVersion.Key.Substring(0, index) : pVersion.Key;

                    System.Version thisVersion = new System.Version(formattedVersion);
                    int comparison = desiredVersion.CompareTo(thisVersion),
                        maxComparison = maxPVersion.CompareTo(thisVersion);

                    if (comparison <= 0)
                    {
                        //Version is greater or equal
                        if (maxComparison >= 0)
                        {
                            desiredVersion = thisVersion;

                            highestVersion = pVersion.Key;
                            highestVersionPath = pVersion.Value;
                        }
                        else
                        {
                            //Console.WriteLine("Version is too high; " + maxComparison.ToString());
                        }
                    }
                    else
                    {
                        //Console.WriteLine("Version (" + pVersion.Key + ") is not within the spectrum.");
                    }
                }

                //Console.WriteLine(highestVersion);
                //Console.WriteLine(highestVersionPath);
                return highestVersionPath;
            }

            return "";
        }

    }
}
