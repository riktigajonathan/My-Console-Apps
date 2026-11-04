namespace skimscript_converter;

using System.Text.RegularExpressions;

internal class Program
{
    static void Main(string[] args)
    {
        string script =
@"


let x be 5.
let name be ""John"".

let greeting be text.

if name is ""John"", then

print ""Hello, John!"",
print 5,
print ""This is the end of the if statement"".

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
    List<string> variableNames;

    public string Parse(string script)
    {
        variableNames = new List<string>();
        string converted = "";
        script = script.Trim();

        foreach (var l in Regex.Split(script, @"(?<=[.,])"))
        {
            string line = l.Trim();

            if (line.StartsWith("let"))
            {
                converted += ConvertLetStatement(line) + "\n";
            }
        }

        return converted;
    }

    private static string ConvertLetStatement(string line)
    {
        Match match;

        match = Regex.Match(line, @"let\s+(\w+)\s+be\s+(.+)\.");
        if (match.Success)
        {
            string variableName = match.Groups[1].Value;

            string variableType = "var";
            switch (match.Groups[2].Value)
            {
                case "str" or "string" or "text":
                    variableType = "string";
                    break;
                case "int" or "integer" or "number":
                    variableType = "int";
                    break;
            }

            if (variableType == "var")
            {
                string value = match.Groups[2].Value;
                return $"{variableType} {variableName} = {value};";
            } 
            else
            {
                return $"{variableType} {variableName};";
            }
        }

        return line;
    }
}