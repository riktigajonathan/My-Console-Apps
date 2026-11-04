using System.Drawing;
using System.Text.RegularExpressions;

namespace skimscript_converter;

internal class Program
{
    static void Main(string[] args)
    {
        string script = @"
let x be 5.
let name be ""John"".

let greeting be text.

let greeting be ""Hi, there, friend."". 

if name is John, then
let thing be 5,
if name is John, then
let thing be 5,
let thing2 be 10.
let thing2 be 10.

print ""Hello, stranger!"".
";

        Parser parser = new Parser();
        parser.Parse(script);

        Console.WriteLine(parser.result);
    }
}

class Parser
{
    public string result = "";

    public void Parse(string script)
    {
        script = script.Trim();
        result = ParseBlock(ref script);
    }

    private int FindNextTerminator(string text, out char terminator)
    {
        bool inString = false;
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (c == '"' && (i == 0 || text[i - 1] != '\\'))
            {
                inString = !inString;
            }
            else if (!inString && (c == '.' || c == ','))
            {
                terminator = c;
                return i;
            }
        }
        terminator = '\0';
        return -1;
    }

    private string ParseBlock(ref string text)
    {
        string output = "";

        while (text.Length > 0)
        {
            text = text.Trim();
            if (text.Length == 0) break;

            if (text.StartsWith("."))
            {
                text = text.Substring(1);
                continue;
            }
            if (text.StartsWith(","))
            {
                text = text.Substring(1);
                continue;
            }

            if (text.StartsWith("if"))
            {
                output += ParseIfStatement(ref text) + "\n";
            }
            else if (text.StartsWith("let"))
            {
                int idx = FindNextTerminator(text, out char term);
                string stmt;
                if (idx == -1)
                {
                    stmt = text;
                    text = "";
                }
                else
                {
                    stmt = text.Substring(0, idx).Trim();
                    text = text.Substring(idx + 1);
                }

                string parsedLet = ConvertLetStatement(stmt);
                if (!string.IsNullOrEmpty(parsedLet))
                {
                    output += parsedLet + "\n";
                }
            }
            else if (text.StartsWith("print"))
            {
                int idx = FindNextTerminator(text, out char term);
                string stmt;
                if (idx == -1)
                {
                    stmt = text;
                    text = "";
                }
                else
                {
                    stmt = text.Substring(0, idx).Trim();
                    text = text.Substring(idx + 1);
                }

                string parsedPrint = ConvertPrintStatement(stmt);
                if (!string.IsNullOrEmpty(parsedPrint))
                {
                    output += parsedPrint + "\n";
                }
            }
            else
            {
                int idx = FindNextTerminator(text, out char term);
                string stmt;
                if (idx == -1)
                {
                    stmt = text;
                    text = "";
                }
                else
                {
                    stmt = text.Substring(0, idx).Trim();
                    text = text.Substring(idx + 1);
                }

                if (!string.IsNullOrEmpty(stmt))
                {
                    output += "// unrecognized line: " + stmt + "\n";
                }
            }
        }

        return output;
    }

    private string ParseIfStatement(ref string text)
    {
        Match match = Regex.Match(text, @"^if\s+(.*?)\s*,\s*then", RegexOptions.Singleline);
        if (!match.Success)
        {
            return "// unrecognized if statement";
        }

        string condition = match.Groups[1].Value;
        condition = TranslateCondition(condition);

        text = text.Substring(match.Index + match.Length).Trim();

        string statement = "if (" + condition + ") {\n";

        while (text.Length > 0)
        {
            text = text.Trim();
            if (text.StartsWith("."))
            {
                text = text.Substring(1);
                break;
            }
            if (text.StartsWith(","))
            {
                text = text.Substring(1);
                continue;
            }

            if (text.StartsWith("if"))
            {
                string innerIf = ParseIfStatement(ref text);
                statement += "    " + innerIf.Replace("\n", "\n    ").TrimEnd() + "\n";
            }
            else if (text.StartsWith("let"))
            {
                int idx = FindNextTerminator(text, out char term);
                string stmt;
                if (idx == -1)
                {
                    stmt = text;
                    text = "";
                }
                else
                {
                    stmt = text.Substring(0, idx).Trim();
                    text = text.Substring(idx + 1);
                }

                string parsedLet = ConvertLetStatement(stmt);
                if (!string.IsNullOrEmpty(parsedLet))
                {
                    statement += "    " + parsedLet + "\n";
                }

                if (term == '.')
                {
                    break;
                }
            }
            else if (text.StartsWith("print"))
            {
                int idx = FindNextTerminator(text, out char term);
                string stmt;
                if (idx == -1)
                {
                    stmt = text;
                    text = "";
                }
                else
                {
                    stmt = text.Substring(0, idx).Trim();
                    text = text.Substring(idx + 1);
                }

                string parsedPrint = ConvertPrintStatement(stmt);
                if (!string.IsNullOrEmpty(parsedPrint))
                {
                    statement += "    " + parsedPrint + "\n";
                }

                if (term == '.')
                {
                    break;
                }
            }
            else
            {
                int idx = FindNextTerminator(text, out char term);
                string stmt;
                if (idx == -1)
                {
                    stmt = text;
                    text = "";
                }
                else
                {
                    stmt = text.Substring(0, idx).Trim();
                    text = text.Substring(idx + 1);
                }

                if (!string.IsNullOrEmpty(stmt))
                {
                    statement += "    // unrecognized line: " + stmt + "\n";
                }

                if (term == '.')
                {
                    break;
                }
            }
        }

        statement += "}";
        return statement;
    }

    private string TranslateCondition(string condition)
    {
        condition = condition
            .Replace("is equal to", "==")
            .Replace("is not equal to", "!=")
            .Replace("isn't equal to", "!=")
            .Replace("is less than or equal to", "<=")
            .Replace("is less than", "<")
            .Replace("is greater than or equal to", ">=")
            .Replace("is greater than", ">")
            .Replace("not", "!")
            .Replace("binary and", "&")
            .Replace("and", "&&")
            .Replace("exclusive or", "^")
            .Replace("binary or", "|")
            .Replace("or", "||");

        if (Regex.IsMatch(condition, @"\w+\s+is\s+(?!==|!=|<|>)\w+"))
        {
            condition = Regex.Replace(condition, @"(\w+)\s+is\s+(\w+)", matchItem =>
            {
                string varName = matchItem.Groups[1].Value;
                string val = matchItem.Groups[2].Value;
                string formattedVal = int.TryParse(val, out _) || double.TryParse(val, out _) ? val : $"\"{val}\"";
                return $"{varName} == {formattedVal}";
            });
        }

        return condition;
    }

    private string ConvertLetStatement(string line)
    {
        line = line.TrimEnd('.');

        Match match = Regex.Match(line, @"let\s+(\w+)\s+be\s+(.+)");
        if (match.Success)
        {
            string variableName = match.Groups[1].Value;
            string variableType = "var";

            switch (match.Groups[2].Value)
            {
                case "str" or "string" or "text":
                    variableType = "string";
                    break;
                case "int" or "integer":
                    variableType = "int";
                    break;
                case "bool" or "boolean" or "flag":
                    variableType = "bool";
                    break;
                case "float" or "number":
                    variableType = "float";
                    break;
            }

            string value = match.Groups[2].Value;
            if (variableType == "float")
                value += "f";

            if (variableType == "var")
            {
                return $"{variableType} {variableName} = {value};".Trim('.').Trim();
            }
            else
            {
                return $"{variableType} {variableName}".Trim('.').Trim();
            }
        }

        return "// unrecognized line: " + line;
    }

    private string ConvertPrintStatement(string line)
    {
        line = line.TrimEnd('.');
        Match match = Regex.Match(line, @"print\s+(.+)");
        if (match.Success)
        {
            string value = match.Groups[1].Value.Trim();
            return $"Console.WriteLine({value});";
        }

        return "// unrecognized line: " + line;
    }
}