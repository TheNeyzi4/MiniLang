using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace NLang
{
    class MiniLang
    {
        private readonly Dictionary<string, object> variables = [];

        public void Run(string code)
        {
            var lines = code.Split('\n');
            foreach (var raw in lines)
            {
                string line = raw.Trim();
                if (string.IsNullOrEmpty(line)) continue;

                if (line.StartsWith("print"))
                {
                    string expr = line[5..].Trim(' ', '(', ')');
                    Console.WriteLine(Eval(expr));
                } 
                
                else if (line.Contains('='))
                {
                    var parts = line.Split('=');
                    string name = parts[0].Trim();
                    string expr = parts[1].Trim();
                    variables[name] = Eval(expr);
                } 
                
                else if (line.StartsWith("if"))
                {
                    var condition = line.Substring(3, 5).Trim();
                    var command = line.Substring(8).Trim();

                    bool result = false;

                    if (condition.Contains('>'))
                    {
                        var parts = condition.Split(">");

                        int left = (int)(variables.ContainsKey(parts[0].Trim()) 
                            ? variables[parts[0].Trim()] : int.Parse(parts[0].Trim()));
                        int right = (int)(variables.ContainsKey(parts[1].Trim()) 
                            ? variables[parts[1].Trim()] : int.Parse(parts[1].Trim()));

                        result = left > right;
                    }
                    else if (condition.Contains('<'))
                    {
                        var parts = condition.Split("<");
                        int left = (int)(variables.ContainsKey(parts[0].Trim()) ? variables[parts[0].Trim()] : int.Parse(parts[0].Trim()));
                        int right = (int)(variables.ContainsKey(parts[1].Trim()) ? variables[parts[1].Trim()] : int.Parse(parts[1].Trim()));
                        result = left < right;
                    }

                    if (result) Run(command);
                }
            }
        }

        private int Eval(string expr)
        {
            expr = expr.Replace(" ", "");

            int result = 0;
            int current = 0;
            int sign = 1;
            char lastOp = '+';
            string token = "";

            for (int i = 0; i < expr.Length; i++)
            {
                char c = expr[i];

                if (char.IsLetterOrDigit(c))
                {
                    token += c;
                }

                if (!char.IsLetterOrDigit(c) || i == expr.Length - 1)
                {
                    if (token != "")
                    {
                        int number;
                        if (variables.TryGetValue(token, out object? value))
                            number = Convert.ToInt32(value);
                        else
                            number = int.Parse(token);
                        token = "";

                        if (lastOp == '*') current *= number;
                        else if (lastOp == '/') current /= number;
                        else current = number;
                    }

                    if (c == '+' || c == '-' || i == expr.Length - 1)
                    {
                        result += sign * current;

                        sign = c == '-' ? -1 : 1;
                        current = 0;
                    }

                    lastOp = c;
                }
            }

            return result;
        }
    }
}
