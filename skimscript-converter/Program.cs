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

    public string Parse(string script)
    {
        variableNames = new List<string>();
        string converted = "";
        script = script.Trim();

        foreach (var l in Regex.Split(script, @"(?<=[.])"))
        {
            string line = l.Trim();

            if (line.StartsWith("let"))
            {
                converted += ConvertLetStatement(line) + "\n";
            }
        }

        return converted;
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

        return line;
    }
}