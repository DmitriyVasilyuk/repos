using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ConsoleApp3
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var test = "fldp=Test%20Project%201%2FFolder%201%2Ftest2&fldp=Test%20Project%201%2FFolder%201%2Ftest6&fldp=Test%20Project%201%2FFolder%201%2Ftest6%2Flastchild_folder&fldp=Test%20Project%201%2Ftest3&fldp=Test%20Project%201%2Ftest5&fldp=Société%20Ａ＄＄ԱԲԳԴԵԶԷԸ%20ԹԺԻԼԽԾԿ&fldp=ñ";

            Console.WriteLine(HttpUtility.UrlDecode(test));

        }
    }
}