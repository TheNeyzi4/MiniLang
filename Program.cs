using System;
using System.Collections.Generic;

namespace NLang
{ 
    class Program
    {
        static void Main()
        {
            var miniLang = new MiniLang();
            miniLang.Run(@"
x = 10
y = 10
z = 25
print(z + x * y)
print(x - 5 + 28 * 156)
if x > 5 print(100 + 10)
");
            
        }
    }
}