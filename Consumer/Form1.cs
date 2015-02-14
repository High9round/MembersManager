using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;


using System.Net;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Consumer
{
    

    public partial class Form1 : Form
    {
      
        private const int WM_KEYDOWN = 0x100;
        private const int WM_SYSKEYDOWN = 0x104;

        List<Member> member = new List<Member>();
        List<Member> Del = new List<Member>();
        List<Member> Upd = new List<Member>();
        List<Member> New = new List<Member>();

        List<Screen> screen = new List<Screen>();
        List<Locker> locker = new List<Locker>();

        static bool saved;
        
        //list view sort
        class ListViewItemComparer : IComparer
        {
            private int col;
            public string sort = "asc";
            public ListViewItemComparer()
            {
                col = 0;
            }
            /// <summary>
            /// 컬럼과 정렬 기준(asc, desc)을 사용하여 정렬 함.
            /// </summary>
            /// <param name="column">몇 번째 컬럼인지를 나타냄.</param>
            /// <param name="sort">정렬 방법을 나타냄. Ex) asc, desc</param>
            public ListViewItemComparer(int column, string sort)
            {
                col = column;
                this.sort = sort;
            }
            public int Compare(object x, object y)
            {
                if (sort == "asc")
                    return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
                else
                    return String.Compare(((ListViewItem)y).SubItems[col].Text, ((ListViewItem)x).SubItems[col].Text);
            }
        }


        int select_index;
        
        public Form1()
        {
            InitializeComponent();
        }
        struct list_mem
        {
            public string name;
            public string sex;
            public string type;
            public string etc;
            public int index;
        }

        
        
        private void list_refresh()//리스트뷰 새로고침
        {
            list_mem temp;
            listView_mem.Items.Clear();
           
            for (int i = 0; i < member.Count; i++)
            {
                temp.name = member[i].get_name();
                temp.sex = member[i].get_sex();
                temp.type = member[i].get_type();
                temp.etc = member[i].get_etc();
                temp.index = i;
                listView_mem.Items.Add(new ListViewItem(new string[] { temp.name, temp.sex, temp.type, temp.etc }));
            }
        }

        public void Load_Json()
        {
            try
            {
                using (StreamReader r = new StreamReader("Member_list.json"))
                {
                    string json = r.ReadToEnd();
                    member = JsonConvert.DeserializeObject<List<Member>>(json);
                    r.Close();
                }

            }
            catch
            {

            }                               
            
        }

        public void Save_Json()
        {
            try
            {
                using (FileStream fs = File.Open("Member_list.json", FileMode.Create))
                using (StreamWriter sw = new StreamWriter(fs))
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;

                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jw, member);

                }
            }
            catch
            {
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            saved = false;
            FileInfo f_info = new FileInfo("Member_list.json");
            if (f_info.Exists)
            {
                //json load
                Load_Json();

            }
            else//for windows xp user
            {
                using (FileStream fs = File.Open("Member_list.json", FileMode.Create))
                using (StreamWriter sw = new StreamWriter(fs))
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;

                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jw, member);


                }
             }
           
           

            //리스트뷰 새로고침
            try
            {
                list_refresh();
                list_locker_refrash();
            }
            catch
            {
            }
            //현재시간 표시
            timer1.Interval=500;
            timer1.Start();

            //combobox default value
            comboBox_mtype.SelectedItem = "정기회원";
            comboBox_sex.SelectedItem = "남자";
            comboBox_type.SelectedItem = "정기회원";
            comboBox_sep.SelectedItem = "이름";
                                   
            //기타 설정
            listView_mem.FullRowSelect = true;
        }



        //회원 관리
        private void button_add_Click(object sender, EventArgs e)
        {
            Member temp=new Member();

            try
            {
                temp.set_address((string)textBox_add.Text);
                temp.set_birth(dateTimePicker_birth.Value);
                temp.set_car((string)textBox_car.Text);

                if (string.IsNullOrEmpty(textBox_card.Text))
                {
                    temp.set_card(0);
                }
                else
                {
                    temp.set_card(System.Convert.ToInt32(textBox_card.Text));
                }
                if (string.IsNullOrEmpty(textBox_cash.Text))
                {
                    temp.set_cash(0);
                }
                else
                {
                    temp.set_cash(System.Convert.ToInt32(textBox_cash.Text));
                }
                if (textBox_phone.Text.Length > 11)
                {
                    MessageBox.Show("전화번호가 너무 깁니다");
                    return ;
                }
                else if (textBox_phone.Text.Length < 8)
                {
                    MessageBox.Show("전화번호가 너무 짧습니다");
                    return;
                }
                temp.set_cphone((string)textBox_phone.Text);
                
                if (textBox_phone.Text.Length == 10)
                {
                    string pass;
                    pass = temp.c_phone[6].ToString() + temp.c_phone[7].ToString() + temp.c_phone[8].ToString() + temp.c_phone[9].ToString();
                    
                    temp.set_password(pass);
                    
                }
                else if (textBox_phone.Text.Length == 11)
                {
                    string pass;
                    pass = temp.c_phone[7].ToString() + temp.c_phone[8].ToString() + temp.c_phone[9].ToString() + temp.c_phone[10].ToString();
                    temp.set_password(pass);
                }

                temp.set_etc((string)textBox_etc.Text);


                int check=Convert.ToInt32(textBox_id.Text);
                for (int i = 0; i < member.Count; i++)
                {
                    if (check == member[i].id_number)
                    {
                        MessageBox.Show("회원번호는 중복될수 없습니다");
                        return;
                    }
                }
                temp.set_id(System.Convert.ToInt32(textBox_id.Text));


                temp.set_joindate(dateTimePicker_start.Value);
                temp.set_limit(System.Convert.ToInt32(textBox_limit.Text));
               
                temp.set_name((string)textBox_name.Text);
                temp.set_joinsite("본점");
                int numb = 0;
                string strTmp;
                int numTep;
                for (int i = 0; i < member.Count; i++)
                {
                    strTmp = Regex.Replace(member[i].o_number, @"\D", "");
                    numTep = Convert.ToInt32(strTmp);
                    if (numb <= numTep)
                    {
                        numb = numTep;
                    }
                }
                numb++;
                temp.set_o_num(temp.get_joinsite()+"_"+numb.ToString());
                
                temp.set_phone((string)textBox_tel.Text);
                temp.set_sex(comboBox_sex.SelectedItem.ToString());
                temp.set_sold(temp.card+temp.cash);
                temp.set_term_e(dateTimePicker_end.Value);
                temp.set_term_s(dateTimePicker_start.Value);
                temp.set_type(comboBox_mtype.SelectedItem.ToString());
                temp.set_um(checkBox_um.Checked);
                temp.set_weding(dateTimePicker_wed.Value);
                temp.set_zip((string)textBox_zip.Text);
               
                temp.set_email((string)textBox_email.Text);

                if (string.IsNullOrEmpty(textBox_car.Text))
                {
                    temp.set_car("");
                }
            
                if (string.IsNullOrEmpty(textBox_etc.Text))
                {
                    temp.set_etc("");
                }
                if (string.IsNullOrEmpty(textBox_locker.Text))
                {
                    temp.set_lock(-1);
                }
                if (string.IsNullOrEmpty(textBox_tel.Text))
                {
                    temp.set_phone("");
                }
                if (string.IsNullOrEmpty(textBox_zip.Text))
                {
                    temp.set_zip("");
                }
                if (string.IsNullOrEmpty(textBox_email.Text))
                {
                    temp.set_email("");
                }

                New.Add(temp);
                New[New.Count - 1].sync_type = "new";

                member.Add(temp);
                list_refresh();
                
            }
            catch
            {
                
                                
                if(string.IsNullOrEmpty(textBox_add.Text))
                {
                    MessageBox.Show("주소를 입력하십시오");
                }
               
                if (string.IsNullOrEmpty(textBox_id.Text))
                {
                    MessageBox.Show("회원번호를 입력하시오");
                }
                if (string.IsNullOrEmpty(textBox_limit.Text))
                {
                    MessageBox.Show("개수 제한을 입력하시오");
                }
               
                if (string.IsNullOrEmpty(textBox_name.Text))
                {
                    MessageBox.Show("이름을 입력하시오");
                }
                
                if (string.IsNullOrEmpty(textBox_phone.Text))
                {
                    MessageBox.Show("휴대전화 번호를 입력하시오");
                }
                
               
            }
            
           
        }

        private void button_edit_Click(object sender, EventArgs e)
        {
            try
            {
                
                Member temp = new Member();

                temp.set_address((string)textBox_add.Text);
                temp.set_birth(dateTimePicker_birth.Value);
                temp.set_car((string)textBox_car.Text);
                temp.set_card(System.Convert.ToInt32(textBox_card.Text));
                temp.set_cash(System.Convert.ToInt32(textBox_cash.Text));
                temp.set_cphone((string)textBox_phone.Text);
                temp.set_etc((string)textBox_etc.Text);
                temp.set_id(System.Convert.ToInt32(textBox_id.Text));
                temp.set_joindate(dateTimePicker_start.Value);
                temp.set_limit(System.Convert.ToInt32(textBox_limit.Text));
               
                temp.set_name((string)textBox_name.Text);
                temp.set_phone((string)textBox_tel.Text);
                temp.set_sex(comboBox_sex.SelectedItem.ToString());
                temp.set_sold(temp.card + temp.cash);
                temp.set_term_e(dateTimePicker_end.Value);
                temp.set_term_s(dateTimePicker_start.Value);
                temp.set_type(comboBox_mtype.SelectedItem.ToString());
                temp.set_um(checkBox_um.Checked);
                temp.set_weding(dateTimePicker_wed.Value);
                temp.set_zip((string)textBox_zip.Text);
                temp.set_joinsite("본점");
                temp.set_email((string)textBox_email.Text);

                if (string.IsNullOrEmpty(textBox_car.Text))
                {
                    temp.set_car("");
                }
                if (string.IsNullOrEmpty(textBox_card.Text))
                {
                    temp.set_card(0);
                }
                if (string.IsNullOrEmpty(textBox_cash.Text))
                {
                    temp.set_cash(0);
                }
                if (string.IsNullOrEmpty(textBox_etc.Text))
                {
                    temp.set_etc("");
                }
                if (string.IsNullOrEmpty(textBox_locker.Text))
                {
                    temp.set_lock(-1);
                }
                if (string.IsNullOrEmpty(textBox_tel.Text))
                {
                    temp.set_phone("");
                }
                if (string.IsNullOrEmpty(textBox_zip.Text))
                {
                    temp.set_zip("");
                }
                if (string.IsNullOrEmpty(textBox_email.Text))
                {
                    temp.set_email("");
                }
                Upd.Add(temp);
                Upd[Upd.Count - 1].sync_type = "update";
                member[select_index] = temp;
            }
            catch 
            {
                if (string.IsNullOrEmpty(textBox_add.Text))
                {
                    MessageBox.Show("주소를 입력하십시오");
                }

                if (string.IsNullOrEmpty(textBox_id.Text))
                {
                    MessageBox.Show("회원번호를 입력하시오");
                }
                if (string.IsNullOrEmpty(textBox_limit.Text))
                {
                    MessageBox.Show("개수 제한을 입력하시오");
                }

                if (string.IsNullOrEmpty(textBox_name.Text))
                {
                    MessageBox.Show("이름을 입력하시오");
                }
                
                if (string.IsNullOrEmpty(textBox_phone.Text))
                {
                    MessageBox.Show("휴대전화 번호를 입력하시오");
                }
            }
            list_refresh();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            label_onum.Text="";
            Del.Add(member[select_index]);
            Del[Del.Count - 1].sync_type = "delete";
            member.RemoveAt(select_index);
            list_refresh();
        }

        private void listview_mem_select(object sender, EventArgs e)
        {
            try
            {
                if (listView_mem.SelectedIndices.Count <= 0)
                {
                    return;
                }

                for (int i = 0; i < member.Count; i++)
                {
                    if (listView_mem.FocusedItem.SubItems[0].Text == member[i].get_name())
                    {
                        if (listView_mem.FocusedItem.SubItems[1].Text == member[i].get_sex())
                        {
                            if (listView_mem.FocusedItem.SubItems[2].Text == member[i].get_type())
                            {
                                if (listView_mem.FocusedItem.SubItems[3].Text == member[i].get_etc())
                                {
                                    select_index = i;

                                }
                            }
                        }

                    }

                }
                if (select_index >= 0)
                {

                    this.listView_mem.Items[listView_mem.SelectedIndices[0]].Focused = true;
                    this.listView_mem.Items[listView_mem.SelectedIndices[0]].Selected = true;

                    textBox_add.Text = member[select_index].get_address();
                    textBox_car.Text = member[select_index].get_car();
                    textBox_card.Text = member[select_index].get_card().ToString();
                    textBox_cash.Text = member[select_index].get_cash().ToString();
                    textBox_etc.Text = member[select_index].get_etc();
                    textBox_id.Text = member[select_index].get_id().ToString();
                    textBox_limit.Text = member[select_index].get_limit().ToString();
                    if (member[select_index].get_lock() == -1)
                    {
                        textBox_locker.Text = "사용안함";
                    }
                    else
                    {
                        textBox_locker.Text = member[select_index].get_lock().ToString();
                    }
                    textBox_name.Text = member[select_index].get_name();
                    label_onum.Text = member[select_index].get_o_num().ToString();
                    textBox_phone.Text = member[select_index].get_cphone().ToString();
                    label_joinsite.Text = member[select_index].get_joinsite();
                    textBox_email.Text = member[select_index].get_email();
                    label_Sold.Text = member[select_index].get_sold().ToString();
                    textBox_tel.Text = member[select_index].get_phone().ToString();
                    textBox_zip.Text = member[select_index].get_zip();

                    checkBox_um.Checked = member[select_index].get_um();

                    dateTimePicker_birth.Value = member[select_index].get_birth();
                    dateTimePicker_wed.Value = member[select_index].get_weding();
                    dateTimePicker_start.Value = member[select_index].get_term_s();
                    dateTimePicker_end.Value = member[select_index].get_term_e();

                    comboBox_sex.SelectedItem = member[select_index].get_sex();
                    comboBox_mtype.SelectedItem = member[select_index].get_type();
                }

            }
            catch
            { }
           
        }

        private void button_all_Click(object sender, EventArgs e)
        {
            list_refresh();
        }

        private void button_scerch_Click(object sender, EventArgs e)
        {
            listView_mem.Items.Clear();

            int i;
            list_mem temp;
                       
            for (i = 0; i < member.Count; i++)
            {
                if ((string)comboBox_type.SelectedItem == member[i].get_type())
                {
                    if ((string)comboBox_sep.SelectedItem == "이름")
                    {
                        if(textBox_find.Text==member[i].get_name())
                        {
                            temp.type = member[i].get_type();
                            temp.name = member[i].get_name();
                            temp.sex = member[i].get_sex();
                            temp.etc = member[i].get_etc();

                            listView_mem.Items.Add(new ListViewItem(new string[] { temp.name, temp.sex, temp.type, temp.etc }));
                        }
                    }
                    else if ((string)comboBox_sep.SelectedItem == "성별")
                    {
                        if (textBox_find.Text == member[i].get_sex())
                        {
                            temp.type = member[i].get_type();
                            temp.name = member[i].get_name();
                            temp.sex = member[i].get_sex();
                            temp.etc = member[i].get_etc();

                            listView_mem.Items.Add(new ListViewItem(new string[] { temp.name, temp.sex, temp.type, temp.etc }));
 
                        }
 
                    }
                    else if ((string)comboBox_sep.SelectedItem== "비고")
                    {
                        if (textBox_find.Text == member[i].get_etc())
                        {
                            temp.type = member[i].get_type();
                            temp.name = member[i].get_name();
                            temp.sex = member[i].get_sex();
                            temp.etc = member[i].get_etc();

                            listView_mem.Items.Add(new ListViewItem(new string[] { temp.name, temp.sex, temp.type, temp.etc }));
                         }
                    }
                }
       
                
            }
            
        }

        private void listView_mem_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //정렬을 위하여 사용 됨.
            try
            {
                if (this.listView_mem.Sorting == SortOrder.Ascending || listView_mem.Sorting == SortOrder.None)
                {
                    this.listView_mem.ListViewItemSorter = new ListViewItemComparer(e.Column, "desc");
                    listView_mem.Sorting = SortOrder.Descending;//가끔씩 에러 뜸
                }
                else
                {
                    this.listView_mem.ListViewItemSorter = new ListViewItemComparer(e.Column, "asc");
                    listView_mem.Sorting = SortOrder.Ascending;//가끔씩 에러 뜸
                }

                listView_mem.Sort();
            }
            catch
            { }
        }

        private void button_save_xls_Click(object sender, EventArgs e)
        {
            
            new xls().save_xls(member,this.Cursor);
        }

        private void button_Load_xls_Click(object sender, EventArgs e)
        {
            new xls().Load_xls(member,this.Cursor);
            list_refresh();
            //oledb 등록 오류 발생시 http://h5bak.tistory.com/139 로 갈것(이 문제는 OS상의 문제임)

        }

       
        private void timer1_Tick(object sender, EventArgs e)//현재시간 실시간 표시
        {
            label_currenttime.Text = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            
        }

        //타석관리
        private void label_screen1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1번 타석 선택");
        }

        private void label_screen2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("2번 타석 선택");
        }

        private void label_screen3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("3번 타석 선택");
        }

        private void label_screen4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("4번 타석 선택");
        }

        private void label_screen5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("5번 타석 선택");
        }

        private void label_screen6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("6번 타석 선택");
        }

        private void label_screen7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("7번 타석 선택");
        }

        private void label_screen8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("8번 타석 선택");
        }

        private void label_screen9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("9번 타석 선택");
        }

        private void label_screen10_Click(object sender, EventArgs e)
        {
            MessageBox.Show("10번 타석 선택");
        }

        private void label_screen11_Click(object sender, EventArgs e)
        {
            MessageBox.Show("11번 타석 선택");
        }

        private void label_screen12_Click(object sender, EventArgs e)
        {
            MessageBox.Show("12번 타석 선택");
        }

        private void label_screen13_Click(object sender, EventArgs e)
        {
            MessageBox.Show("13번 타석 선택");
        }

        private void label_screen14_Click(object sender, EventArgs e)
        {
            MessageBox.Show("14번 타석 선택");
        }

        private void label_screen15_Click(object sender, EventArgs e)
        {
            MessageBox.Show("15번 타석 선택");
        }

        private void label_screen16_Click(object sender, EventArgs e)
        {
            MessageBox.Show("16번 타석 선택");
        }

        private void label_screen17_Click(object sender, EventArgs e)
        {
            MessageBox.Show("17번 타석 선택");
        }

        private void label_screen18_Click(object sender, EventArgs e)
        {
            MessageBox.Show("18번 타석 선택");
        }

        private void label_screen19_Click(object sender, EventArgs e)
        {
            MessageBox.Show("19번 타석 선택");
        }

        private void label_screen20_Click(object sender, EventArgs e)
        {
            MessageBox.Show("20번 타석 선택");
        }

        private void label_screen21_Click(object sender, EventArgs e)
        {
            MessageBox.Show("21번 타석 선택");
        }

        private void label_screen22_Click(object sender, EventArgs e)
        {
            MessageBox.Show("22번 타석 선택");
        }

        private void label_screen23_Click(object sender, EventArgs e)
        {
            MessageBox.Show("23번 타석 선택");
        }

        private void label_screen24_Click(object sender, EventArgs e)
        {
            MessageBox.Show("24번 타석 선택");
        }

        private void label_screen25_Click(object sender, EventArgs e)
        {
            MessageBox.Show("25번 타석 선택");
        }

        private void label_screen26_Click(object sender, EventArgs e)
        {
            MessageBox.Show("26번 타석 선택");
        }

        private void label_screen27_Click(object sender, EventArgs e)
        {
            MessageBox.Show("27번 타석 선택");
        }

        private void label_screen28_Click(object sender, EventArgs e)
        {
            MessageBox.Show("28번 타석 선택");
        }

        private void label_screen29_Click(object sender, EventArgs e)
        {
            MessageBox.Show("29번 타석 선택");
        }

        private void label_screen30_Click(object sender, EventArgs e)
        {
            MessageBox.Show("30번 타석 선택");
        }
        //락커 관리
        private void button_locksubmit_Click(object sender, EventArgs e)
        {
            try
            {
                locker_add(Convert.ToInt32(textBox_locker.Text));
                tabControl1.SelectedTab = tabPage_lock;
            }
            catch
            {
                MessageBox.Show("이용자를 선택해 주십시오");
            }
            
        }
        public void locker_add(int input)
        {

            for(int i=0;i<locker.Count;i++)
            {
                if (locker[i].get_number() == input)
                {
                    MessageBox.Show("이미 사용중인 락커 입니다.");
                    return;    
                }
            }
           
            Locker temp = new Locker();
            
            try
            {
                member[select_index].set_lock(input);
                temp.set_user(member[select_index]);
                temp.set_number(input);
                temp.set_desposit(0);

                temp.set_start(temp.user.get_term_s());
                temp.set_end(temp.user.get_term_e());


                TimeSpan ts = temp.user.get_term_e() - temp.get_start();
                temp.set_limit(ts.Days);

                locker.Add(temp);
                list_locker_refrash();
            }
            catch
            {
                MessageBox.Show("이용자를 선택해 주십시오");
                return;            
            }
            
           
            
            
        }
        struct list_lock1
        {
            public string name;
            public int num;
        }

        struct list_lock2
        {
            public int num;
            public string usertype;
            public string name;
            public string sex;
            public DateTime start;
            public DateTime end;
            public int remain;
            public int desposit;
        }

        public void list_locker_refrash()
        {
            listView_locker1.Items.Clear();
            listView_locker2.Items.Clear();
            
            //view 1
            list_lock1 temp1;
            for (int i = 0; i < locker.Count; i++)
            {
                temp1.name = locker[i].get_username();
                temp1.num = locker[i].get_number();
                listView_locker1.Items.Add(new ListViewItem(new string[] {temp1.name, temp1.num.ToString() }));
            }
            //view 2
            list_lock2 temp2;
            for (int i = 0; i < locker.Count; i++)
            {
                temp2.num = locker[i].get_number();
                temp2.usertype = locker[i].get_usertype();
                temp2.name = locker[i].get_username();
                temp2.sex = locker[i].get_usersex();
                temp2.start = locker[i].get_start();
                temp2.end = locker[i].get_end();
                temp2.remain = locker[i].get_limit();
                temp2.desposit = locker[i].get_desposit();
                listView_locker2.Items.Add(new ListViewItem(new string[] {temp2.num.ToString(), temp2.usertype, temp2.name, temp2.sex, temp2.start.ToString("d"), temp2.end.ToString("d"), temp2.remain.ToString(), temp2.desposit.ToString() }));
            }
 
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //sync();
            if (!saved)
            {
                DialogResult r = MessageBox.Show("저장하고 마치시겠습니까?", "저장안됨", MessageBoxButtons.YesNoCancel);
                if ( r == DialogResult.Yes)
                {
                    Save_Json();
                }
                else if (r == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
            else
            { }
            
         }
        private void sync()
        {
            
            try
            {
                
                //del
                using (FileStream fs = File.Open("Member_del.json", FileMode.Create))
                using (StreamWriter sw = new StreamWriter(fs))
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;

                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jw, Del);

                }
                

                //new
                using (FileStream fs = File.Open("Member_new.json", FileMode.Create))
                using (StreamWriter sw = new StreamWriter(fs))
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;

                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jw, New);

                }
                //update
                using (FileStream fs = File.Open("Member_update.json", FileMode.Create))
                using (StreamWriter sw = new StreamWriter(fs))
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;

                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jw, Upd);

                }
                
            }
            catch
            {
            }
        }
        private void button_server_save_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < member.Count; i++)
            {
                member[i].sync_type = "insert";
 
            }
            Save_Json();
                      
            new Web().upload(this.Cursor);
            
        }
               
        private void button_server_load_Click(object sender, EventArgs e)
        {
            //var down = new Web();
            //down.download(member);
            string r;
            r=new Web().download(member,this.Cursor);
            convert(r);
            list_refresh();
            
        }
        public void convert(string s)
        {

            member.Clear();
            member = JsonConvert.DeserializeObject<List<Member>>(s);
            

            //값이 -1이면 빈값이므로 디폴트값 넣어라
            
            for (int i = 0; i < member.Count; i++)
            {

                member[i].sold = member[i].card + member[i].cash;

            }
            
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            Save_Json();
            saved = true;
            MessageBox.Show("saved!");
        }

        private void button_sync_Click(object sender, EventArgs e)
        {
            List<Member> sync_list=new List<Member>();

            sync_list.AddRange(New);
            sync_list.AddRange(Upd);
            sync_list.AddRange(Del);

            new Web().sync(sync_list, this.Cursor);


            New.Clear();
            Upd.Clear();
            Del.Clear();

        }

        
   }
}
