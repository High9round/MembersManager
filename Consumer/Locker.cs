using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Consumer
{
    class Locker
    {
        public int number;//락커 번호
        public int limit;//남은 날
        public DateTime start;//시작일
        public DateTime end;//종료일
        public int desposit;//보증금
        public Member user;//이용자

        public Locker()
        { }
        public Locker(int n, int l, DateTime s, DateTime e, int d, Member u)
        {
            number = n;
            limit = l;
            start = s;
            end = e;
            desposit = d;
            user = u;
        }

        //set
        public void set_number(int i)
        {
            number = i;
        }
        public void set_limit(int i)
        {
            limit = i;
        }
        public void set_start(DateTime s)
        {
            start = s;
        }
        public void set_end(DateTime e)
        {
            end = e;
        }
        public void set_desposit(int i)
        {
            desposit = i;
        }
        public void set_user(Member m)
        {
            user = m;
        }
        
        //get
        public int get_number()
        {
            return number;
        }
        public int get_limit()
        {
            return limit;
        }
        public DateTime get_start()
        {
            return start;
        }
        public DateTime get_end()
        {
            return end;
        }
        public int get_desposit()
        {
            return desposit;
        }
        public string get_username()
        {
            return user.get_name();
        }
        public string get_usersex()
        {
            return user.get_sex();
        }
        public string get_usertype()
        {
            return user.get_type();
        }
    }
}
