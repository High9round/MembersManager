using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Consumer
{
    class Screen
    {
        private int number;//타석 번호
        private int turn;//순번
        private int playtime;//이용시간(이용 가능한 총 시간)

        public Screen()
        {
 
        }
        public Screen(int n, int t, int p)
        {
            this.number=n;
            this.turn=t;
            this.playtime=p;
        }

        public void set_number(int i)
        {
            number = i;
        }
        public void set_turn(int i)
        {
            turn = i;

        }
        public void set_playtime(int i)
        {
            playtime = i;
        }

        public int get_number()
        {
            return number;
        }
        public int get_turn()
        {
            return turn;
        }
        public int get_playtime()
        {
            return playtime;
        }
    }
}
