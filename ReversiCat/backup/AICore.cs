using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReversiCat
{
    class AICore
    {
        const int wWeight = 1;
        const int wMobility = 1;


        int Mobility()
        {
            return 0;
        }
        int ComputeWeight()
        {
            return 0;
        }


        int Evaluation()
        {
            return wWeight * ComputeWeight() + wMobility * Mobility();
        }

        int AlphaBeta()
        {
            return 0;
        }
    }
}
