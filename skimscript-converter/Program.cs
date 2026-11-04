using System.Text.RegularExpressions;

namespace skimscript_converter;

internal class Program
{
    static void Main(string[] args)
    {
        string script =
@"


let x be 5.
let name be ""John"".

let greeting be text.

let greeting be ""Hi, there"".


if name is equal to ""John"", then print ""Hello, John!"", print 5, print ""This is the end of the if statement"".

if not,

print ""Hello, stranger!"".



";

        Parser parser = new Parser();
        var result = parser.Parse(script);

        Console.WriteLine(result);
    }
}

class Parser
{
    List<string> variableNames = new();
    string result = "";

    public string Parse(string script)
    {
        variableNames = new List<string>();
        script = script.Trim();

        var lines = Regex.Split(script, @"(?<=[.])");
        foreach (var line in lines)
        {
            ParseLine(line);
        }

        return result;
    }

    private void ParseLine(string line)
    {
        line = line.Trim();
        if (line.StartsWith("let"))
        {
            result += ConvertLetStatement(line) + "\n";
        }
        else if (line.StartsWith("if"))
        {
            result += ConvertIfStatement(line) + "\n";
        }
    }

    private string ConvertLetStatement(string line)
    {
        Match match;

        match = Regex.Match(line, @"let\s+(\w+)\s+be\s+(.+)\.");
        if (match.Success)
        {
            string variableName = match.Groups[1].Value;

            bool usedName = false;
            if (!variableNames.Contains(variableName))
            {
                variableNames.Add(variableName);
            }
            else 
            {
                usedName = true;
            }

            string variableType = "var";
            if (usedName)
            {
                variableType = "";
            }
            else
            {
                switch (match.Groups[2].Value)
                {
                    case "str" or "string" or "text":
                        variableType = "string";
                        break;
                    case "int" or "integer" or "number":
                        variableType = "int";
                        break;
                }
            }

            if (variableType == "var" || usedName)
            {
                string value = match.Groups[2].Value;
                return $"{variableType} {variableName} = {value};".Trim();
            } 
            else
            {
                return $"{variableType} {variableName};".Trim();
            }
        }

        return "// unrecognized line: " + line + "";
    }

    private string ConvertIfStatement(string line)
    {
        Match match;

        match = Regex.Match(line, @"if\s+(.*?)\s*,\s*then");
        if (match.Success)
        {
            Console.WriteLine(match.Groups[1]);
        }

        return "// unrecognized line: " + line + "";
    }
}