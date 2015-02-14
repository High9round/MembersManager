using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Consumer
{
   
    
    class Member
    {
        public string sync_type;
        public string o_number;//고유번호 필수 
        public int id_number;//회원번호 필수
        public string name;//이름 필수
        public string type;//회원구분 필수
        public int lim;//개수제한 필수
        public DateTime term_s;//사용기간 필수
        public DateTime term_e;
        public DateTime joindate;//가입일 필수
        public int sold;//판매액 필수
        public int cash;//현금 필수
        public int card;//카드 필수
        public string sex;//성별 필수 
        
        public int locker;//락커 번호

        public string phone;//전번 
        public string c_phone;//폰전번 필수
        public string car;//차번호
        public string zip_cord;//우편번호
        public string address;//주소 필수
        public string email;
        public string joinsite;//가입지점

        public DateTime birth;//생년월일
        public bool um;//음력1/양력0

        public DateTime weding;//결혼기념일
        public string etc;//비고

        public string pass;

        public Member()
        {
            sync_type = "insert";
            o_number = "본점_0";//고유번호
            id_number = 0;//회원번호
            name = "";//이름
            type = "정기회원";//회원구분 0정기 1단기
            lim = 0;//개수제한
            term_s = DateTime.Now;//사용기간
            term_e = DateTime.Now;
            joindate = DateTime.Now;//가입일
            sold = 0;//판매액
            cash = 0;//현금
            card = 0;//카드
            sex = "남자";//성별 0 남자 1 여자

            locker = -1;//락커 번호

            phone = "00000000000";//전번
            c_phone = "00000000000";//폰전번
            car = "";//차번호
            zip_cord = "";//우편번호
            address = "";//주소
            joinsite = "";
            email = "";

            birth = DateTime.Now;//생년월일
            um = false;//음력1/양력0

            weding = DateTime.Now;//결혼기념일
            etc = "";//비고

            pass = "0000";
        }
        public Member(Member m)
        {
            sync_type = "insert";
            o_number=m.o_number;//고유번호
            id_number=m.id_number;//회원번호
            name=m.name;//이름
            type=m.type;//회원구분 0정기 1단기
            lim=m.lim;//개수제한
            term_s=m.term_s;//사용기간
            term_e=m.term_e;
            joindate=m.joindate;//가입일
            sold=m.sold;//판매액
            cash=m.cash;//현금
            card=m.card;//카드
            sex=m.sex;//성별 0 남자 1 여자

            locker=m.locker;//락커 번호

            phone=m.phone;//전번
            c_phone=m.c_phone;//폰전번
            car=m.car;//차번호
            zip_cord=m.zip_cord;//우편번호
            address=m.address;//주소
            joinsite = m.joinsite;
            email = m.email;

            birth=m.birth;//생년월일
            um=m.um;//음력1/양력0

            weding=m.weding;//결혼기념일
            etc=m.etc;//비고

            pass = m.pass;
        }
       
        //set
        public void set_o_num(string o)
        {
            o_number=o;
        }
        public void set_id(int i)
        {
            id_number = i;
        }
        public void set_name(string n)
        {
            name = n;
        }
        public void set_type(string t)
        {
            type = t;
        }
        public void set_limit(int l)
        {
            lim = l;
        }
        public void set_term_s(DateTime ts)
        {
            term_s = ts;
        }
        public void set_term_e(DateTime te)
        {
            term_e = te;
        }
        public void set_joindate(DateTime j)
        {
            joindate = j;
        }
        public void set_sold(int s)
        {
            sold = s;
        }
        public void set_cash(int c)
        {
            cash = c;
        }
        public void set_card(int c)
        {
            card = c;
        }
        public void set_lock(int l)
        {
            locker = l;
        }
        public void set_phone(string p)
        {
            phone = p;
        }
        public void set_cphone(string p)
        {
            c_phone = p;
        }
        public void set_car(string c)
        {
            car = c;
        }
        public void set_zip(string z)
        {
            zip_cord = z;
        }
        public void set_address(string a)
        {
            address = a;
        }
        public void set_birth(DateTime b)
        {
            birth = b;
        }
        public void set_um(bool u)
        {
            um = u;
        }
        public void set_weding(DateTime w)
        {
            weding = w;
        }
        public void set_etc(string e)
        {
            etc = e;
        }
        public void set_sex(string s)
        {
            sex = s;
        }
        public void set_email(string s)
        {
            email = s;
        }
        public void set_joinsite(string j)
        {
            joinsite = j;
        }
        public void set_password(string s)
        {
            pass = s;
        }
        //get
        public string get_o_num()
        {
            return o_number;
        }
        public int get_id()
        {
            return id_number;
        }
        public string get_name()
        {
            return name;
        }
        public string get_type()
        {
            return type;
        }
        public int get_limit()
        {
            return lim;
        }
        public DateTime get_term_s()
        {
            return term_s;
        }
        public DateTime get_term_e()
        {
            return term_e;
        }
        public DateTime get_joindate()
        {
            return joindate;
        }
        public int get_sold()
        {
            return sold;
        }
        public int get_cash()
        {
            return cash;
        }
        public int get_card()
        {
            return card;
        }
        public int get_lock()
        {
            return locker;
        }
        public string get_phone()
        {
            return phone;
        }
        public string get_cphone()
        {
            return c_phone;
        }
        public string get_car()
        {
            return car;
        }
        public string get_zip()
        {
            return zip_cord;
        }
        public string get_address()
        {
            return address;
        }
        public DateTime get_birth()
        {
            return birth;
        }
        public bool get_um()
        {
            return um;
        }
        public DateTime get_weding()
        {
            return weding;
        }
        public string get_etc()
        {
            return etc;
        }
        public string get_sex()
        {
            return sex;
        }
        public string get_email()
        {
            return email;
        }
        public string get_joinsite()
        {
            return joinsite;
        }
    }
}
