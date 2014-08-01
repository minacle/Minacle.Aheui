using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Minacle.Aheui.AheuiInterpreter aheui = new Minacle.Aheui.AheuiInterpreter("밤밣따빠밣밟따빠맣받밪밬따딴박다닥빠맣밠당빠빱맣맣받닫빠맣파빨받밤따다맣맣빠빠밣다맣맣빠밬다맣밬탕빠맣밣타맣발다밤타맣박발땋맣희");
            aheui.Run();
            Console.ReadKey(true);
        }
    }
}
