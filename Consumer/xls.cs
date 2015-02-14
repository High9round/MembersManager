using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.Reflection;
using System.Data;

using System.Threading;

namespace Consumer
{
    class xls
    {
        Thread message=new Thread(new ThreadStart(loading));
        static Form2 mes = new Form2();

        private static Object locker=new Object();

        public xls()
        {
                
        }
        
        private static void loading()
        {
            lock(locker)
            {
                mes.ShowDialog();
            }
        }

        public void save_xls(List<Member> member,Cursor c)
        {
            string path;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "xls File (*.xls)|*.xls";

            
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                c = Cursors.WaitCursor;
                message.Start();

                path = sfd.FileName;

                Excel.Application excelApp = null;
                Excel.Workbook wb = null;
                Excel.Worksheet ws = null;

                try
                {
                    // Excel 첫번째 워크시트 가져오기                
                    excelApp = new Excel.Application();
                    wb = excelApp.Workbooks.Add();
                    ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;

                    //칼럼 
                    ws.Cells[1, 1] = "고유번호";
                    ws.Cells[1, 2] = "회원번호";
                    ws.Cells[1, 3] = "이름";
                    ws.Cells[1, 4] = "회원구분";
                    ws.Cells[1, 5] = "가입지점";
                    ws.Cells[1, 6] = "성별";
                    ws.Cells[1, 7] = "개수 제한";
                    ws.Cells[1, 8] = "사용 기간";
                    ws.Cells[1, 9] = "판매액";
                    ws.Cells[1, 10] = "현금";
                    ws.Cells[1, 11] = "카드";
                    ws.Cells[1, 12] = "락커 번호";
                    ws.Cells[1, 13] = "전화 번호";
                    ws.Cells[1, 14] = "휴대폰 번호";
                    ws.Cells[1, 15] = "차량 번호";
                    ws.Cells[1, 16] = "우편 번호";
                    ws.Cells[1, 17] = "주소";
                    ws.Cells[1, 18] = "생년월일";
                    ws.Cells[1, 19] = "음력";
                    ws.Cells[1, 20] = "결혼 기념일";
                    ws.Cells[1, 21] = "이메일주소";
                    ws.Cells[1, 22] = "비고";

                    //유저 정보 입력
                    for (int i = 0; i < member.Count; i++)
                    {
                        ws.Cells[2 + i, 1] = member[i].get_o_num();
                        ws.Cells[2 + i, 2] = member[i].get_id();
                        ws.Cells[2 + i, 3] = member[i].get_name();
                        ws.Cells[2 + i, 4] = member[i].get_type();
                        ws.Cells[2 + i, 5] = member[i].get_joinsite();
                        ws.Cells[2 + i, 6] = member[i].get_sex();
                        ws.Cells[2 + i, 7] = member[i].get_limit();
                        ws.Cells[2 + i, 8] = member[i].get_term_s().ToString("d") + "~" + member[i].get_term_e().ToString("d");
                        ws.Cells[2 + i, 9] = member[i].get_sold();
                        ws.Cells[2 + i, 10] = member[i].get_cash();
                        ws.Cells[2 + i, 11] = member[i].get_card();
                        ws.Cells[2 + i, 12] = member[i].get_lock();
                        ws.Cells[2 + i, 13] = member[i].get_phone();
                        ws.Cells[2 + i, 14] = member[i].get_cphone();
                        ws.Cells[2 + i, 15] = member[i].get_car();
                        ws.Cells[2 + i, 16] = member[i].get_zip();
                        ws.Cells[2 + i, 17] = member[i].get_address();
                        ws.Cells[2 + i, 18] = member[i].get_birth().ToString("d");
                        if (member[i].get_um())
                        {
                            ws.Cells[2 + i, 19] = "음력";
                        }
                        else
                        {
                            ws.Cells[2 + i, 19] = "양력";
                        }

                        ws.Cells[2 + i, 20] = member[i].get_weding().ToString("d");
                        ws.Cells[2 + i, 21] = member[i].get_email();
                        ws.Cells[2 + i, 22] = member[i].get_etc();
                    }

                    // 엑셀파일 저장
                    try
                    {
                        wb.SaveAs(path, Excel.XlFileFormat.xlWorkbookNormal);
                    }
                    catch
                    {
                    }
                    wb.Close(true);
                    excelApp.Quit();
                }
                finally
                {
                    // Clean up
                    ReleaseExcelObject(ws);
                    ReleaseExcelObject(wb);
                    ReleaseExcelObject(excelApp);

                    c = Cursors.Default;
                    mes.Close();
                    message.Abort();
                    //message
                    MessageBox.Show("save complete");
                }


            }
        }

        public void Load_xls(List<Member> member,Cursor c)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "xls File (*.xls)|*.xls";
            
            if (ofd.ShowDialog() == DialogResult.OK)
            {

                c = Cursors.WaitCursor;
                message.Start();
                string strFileName = ofd.FileName;

                string strConnection = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFileName + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1;MAXSCANROWS=0\"";

                OleDbConnection oleDbCon = new OleDbConnection(strConnection);
                oleDbCon.Open();

                try
                {
                    member.Clear();
                    string strSQL = "SELECT * FROM [Sheet1$]";
                    OleDbCommand dbCommand = new OleDbCommand(strSQL, oleDbCon);
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(dbCommand);

                    DataTable dTable = new DataTable();
                    dataAdapter.Fill(dTable);


                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        try
                        {
                            Member temp = new Member();

                            temp.set_address(dTable.Rows[i][16].ToString());
                            temp.set_birth(Convert.ToDateTime(dTable.Rows[i][17]));
                            temp.set_car(dTable.Rows[i][14].ToString());
                            temp.set_card(Convert.ToInt32(dTable.Rows[i][10]));
                            temp.set_cash(Convert.ToInt32(dTable.Rows[i][9]));
                            temp.set_cphone(dTable.Rows[i][13].ToString());
                            temp.set_etc(dTable.Rows[i][21].ToString());
                            temp.set_id(Convert.ToInt32(dTable.Rows[i][1]));
                            temp.set_joindate(Convert.ToDateTime(sep_datetime_s(dTable.Rows[i][7].ToString())));
                            temp.set_limit(Convert.ToInt32(dTable.Rows[i][6]));
                            temp.set_lock(Convert.ToInt32(dTable.Rows[i][11]));
                            temp.set_name(dTable.Rows[i][2].ToString());
                            temp.set_o_num(dTable.Rows[i][0].ToString());
                            temp.set_phone(dTable.Rows[i][12].ToString());
                            temp.set_sex(dTable.Rows[i][5].ToString());
                            temp.set_sold(temp.get_card() + temp.get_cash());
                            temp.set_term_e(Convert.ToDateTime(sep_datetime_e(dTable.Rows[i][7].ToString())));
                            temp.set_term_s(Convert.ToDateTime(sep_datetime_s(dTable.Rows[i][7].ToString())));
                            temp.set_type(dTable.Rows[i][3].ToString());
                            temp.set_joinsite(dTable.Rows[i][4].ToString());

                            if (dTable.Rows[i][18].ToString() == "음력")
                            {
                                temp.set_um(true);
                            }
                            else if (dTable.Rows[i][18].ToString() == "양력")
                            {
                                temp.set_um(false);
                            }
                            temp.set_weding(Convert.ToDateTime(dTable.Rows[i][19]));
                            temp.set_zip(dTable.Rows[i][15].ToString());
                            temp.set_email(dTable.Rows[i][20].ToString());

                            member.Add(temp);
                        }
                        catch
                        {
                            break;
                        }

                    }

                    // dispose used objects
                    dTable.Dispose();
                    dataAdapter.Dispose();
                    dbCommand.Dispose();

                    oleDbCon.Close();
                    oleDbCon.Dispose();


                }
                catch (Exception ex)
                {

                    string strExec = ex.Message;
                    MessageBox.Show(strExec);
                }
                finally
                {
                    //Form1.list_refresh();
                    oleDbCon.Close();
                    c = Cursors.Default;
                    mes.Close();
                    message.Abort();
                    MessageBox.Show("load complete");
                }


            }

        }

        public string sep_datetime_s(string input)
        {
            string temp = "";
            for (int i = 0; i < input.Length; i++)
            {
                temp += input[i];
                if (input[i + 1] == '~')
                    break;
            }

            return temp;
        }
        public string sep_datetime_e(string input)
        {
            string temp = "";
            bool _in = false;
            for (int i = 0; i < input.Length; i++)
            {
                if (_in == true)
                    temp += input[i];

                if (input[i] == '~')
                    _in = true;


            }

            return temp;
        }

        private static void ReleaseExcelObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
