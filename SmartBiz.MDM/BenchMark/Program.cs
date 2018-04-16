using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchMark.ServiceReference1;
using System.Diagnostics;

namespace BenchMark
{
    class Program
    {
        static void Main(string[] args)
        {
            
            MdmServiceClient client = new MdmServiceClient();
            LastTransactionInfoQuery query = new LastTransactionInfoQuery();
            Console.WriteLine("Press any key to start benchmark");
            Console.ReadKey();
            Console.WriteLine("Benchmark running...");
            Stopwatch st = Stopwatch.StartNew();
            for(int i =0 ;i<10;i++){
             
                var results =client.QueryLastTransactionInfo(query,5,1);
                Console.WriteLine("Service call "+ (i+1) + " Running: "+ st.ElapsedMilliseconds.ToString()+"ms");
            }
            Console.WriteLine("Complete");
            Console.ReadKey();
        }
    }
}
