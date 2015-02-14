using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

using System.Threading;

namespace Consumer
{
    class Web
    {
        
        static Form2 mes = new Form2();
        
        public void upload(Cursor c)
        {
            try
            {
                c = Cursors.WaitCursor;

                
                WebClient client = new WebClient();
                string myfile = "Member_list.json";//파일경로
                client.Credentials = CredentialCache.DefaultCredentials;
                Uri url = new Uri(@"http://220.67.140.148:9007/auto_upload");
                client.UploadFileCompleted += new UploadFileCompletedEventHandler(client_UploadFileCompleted);
                client.UploadFileAsync(url, "post", myfile);//업로드
                mes.ShowDialog();
                //MessageBox.Show("upload success");
                c = Cursors.Default;
                client.Dispose();


            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            finally
            {
               
                                
 
            }

        }

        void client_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            mes.Close();
            MessageBox.Show("upload complete");
        }

        public void sync(List<Member> list, Cursor c)
        {
            
            //220.67.140.148:9007/key?key=

            try
            {
                c = Cursors.WaitCursor;
                string json = JsonConvert.SerializeObject(list);

                WebClient client = new WebClient();
                client.Encoding = ASCIIEncoding.UTF8;

                string url = "http://220.67.140.148:9007/key?key=" + json;
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
                client.DownloadStringAsync(new Uri(url));
                              
                mes.ShowDialog();

                c = Cursors.Default;
                client.Dispose();

                list.Clear();
            }
            catch (Exception err)
            {
                MessageBox.Show("server error");
            }
            finally
            {
                
            }
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            mes.Close();
            MessageBox.Show("sync complete");
           
        }

        
      
        public string download(List<Member>member,Cursor c)
        {
            /*
            try
            {
                string url = "http://localhost:3000/";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stReadData = response.GetResponseStream();
                StreamReader srReadData = new StreamReader(stReadData, Encoding.UTF8);

                string strResult = srReadData.ReadToEnd();
                convert(strResult);

                MessageBox.Show("Load Complete!");
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            */
            string result;
            try
            {
                c = Cursors.WaitCursor;

                //mes.ShowDialog();
                string url = "http://220.67.140.148:9007/showdata";
                var task = MakeAsyncRequest(url, "application/json");

                result = task.Result;
                
                //convert(task.Result, member);
                MessageBox.Show("download complete");

               
            }
            catch (Exception err)
            {
                MessageBox.Show("Server error");
                result = null;
                
            }
            finally
            {
                c = Cursors.Default;
                //mes.Close();

                
            }

            return result;
        }
        public static Task<string> MakeAsyncRequest(string url, string contentType)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = contentType;
            request.Method = WebRequestMethods.Http.Post;
            request.Timeout = 20000;
            request.Proxy = null;

            Task<WebResponse> task = Task.Factory.FromAsync(
                request.BeginGetResponse,
                asyncResult => request.EndGetResponse(asyncResult),
                (object)null);
            
            return task.ContinueWith(t => ReadStreamFromResponse(t.Result));//task async 종료
        }

        private static string ReadStreamFromResponse(WebResponse response)
        {
            string strContent;
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader sr = new StreamReader(responseStream))
            {
                //Need to return this response 
               strContent = sr.ReadToEnd();
                
                
            }
            
            return strContent;
        }
        
       
    }
}
/*
           try
           {
                
               foreach (Member temp in member)
               {
                   string o_number = temp.get_o_num().ToString();
                   string id_number = temp.get_id().ToString();
                   string name = temp.get_name();//이름 필수
                   string type = temp.get_type();//회원구분 필수
                   string lim = temp.get_limit().ToString();//개수제한 필수
                   string term_s = temp.term_s.ToString("yyyy/MM/dd HH:mm:ss");//사용기간 필수
                   string term_e = temp.term_e.ToString("yyyy/MM/dd HH:mm:ss");
                   string joindate = temp.joindate.ToString("yyyy/MM/dd HH:mm:ss");//가입일 필수
                   string sold = temp.sold.ToString();//판매액 필수
                   string cash = temp.cash.ToString();//현금 필수
                   string card = temp.card.ToString();//카드 필수
                   string sex = temp.sex;//성별 필수 

                   string locker = temp.locker.ToString();//락커 번호

                   string phone = temp.phone;//전번 
                   string c_phone = temp.c_phone;//폰전번 필수
                   string car = temp.car;//차번호
                   string zip_cord = temp.zip_cord;//우편번호
                   string address = temp.address;//주소 필수

                   string birth = temp.birth.ToString("yyyy/MM/dd HH:mm:ss");//생년월일
                   string um = temp.um.ToString();//음력1/양력0

                   string weding = temp.weding.ToString("yyyy/MM/dd HH:mm:ss");//결혼기념일
                   string etc = temp.etc;//비고

                   //string url = "http://localhost:3000/input?" + "num=" + num + "&name=" + name;
                   string url = "http://localhost:3000/input?" + "o_number=" + o_number + "&id_number=" + id_number + "&name=" + name + "&type=" + type + "&lim=" + lim + "&term_s=" + term_s + "&term_e=" + term_e + "&joindate=" + joindate + "&sold=" + sold + "&cash=" + cash + "&card=" + card + "&sex=" + sex + "&locker=" + locker + "&phone=" + phone + "&c_phone=" + c_phone + "&car=" + car + "&zip_cord=" + zip_cord + "&address=" + address + "&birth=" + birth + "&um=" + um + "&weding=" + weding + "&etc=" + etc;
                    
                   WebClient client = new WebClient();
                   client.Encoding = ASCIIEncoding.UTF8;
                   Uri u = new Uri(url);
                   //client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(server_save_message);
                   client.DownloadStringAsync(u);
                   client.Dispose();
                   
               }
               MessageBox.Show("server save complete!");
               

           }
           catch (Exception err)
           {
               MessageBox.Show(err.Message);
           }*/