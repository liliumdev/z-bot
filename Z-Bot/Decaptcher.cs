using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace Z_Bot
{
    class Decaptcher
    {
        [DllImport("decaptcher.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int DecaptcherInit();
       
        [DllImport("decaptcher.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int CCprotoInit();
        
        [DllImport("decaptcher.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int CCprotoLogin(int id, string hostname, int port, string login, int login_size, string pwd, int pwd_size);
               
        [DllImport("decaptcher.dll", CallingConvention = CallingConvention.Winapi)]
        unsafe public static extern int CCprotoBalance(int id, float* balance);
                       
        [DllImport("decaptcher.dll", CallingConvention = CallingConvention.Winapi)]
        unsafe public static extern int CCprotoPicture(int id, byte* pict, int pict_size, byte* text);

        [DllImport("decaptcher.dll", CallingConvention = CallingConvention.Winapi)]
        unsafe public static extern int CCprotoPicture2(int id, byte* pict, int pict_size, int* p_pict_to, int* p_pict_type, byte* text, int size_buf, int* major_id, int* minor_id);
              
        [DllImport("decaptcher.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int CCprotoPictureBad(int id);

        [DllImport("decaptcher.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int CCprotoPictureBad2(int id, int major_id, int minor_id);

        [DllImport("decaptcher.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int CCprotoClose(int id);

        [DllImport("decaptcher.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int CCprotoDestroy(int id);

        [DllImport("decaptcher.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int DecaptcherDestroy(int id);

        public static int id;
        public static int ret;
        public static int pic_size;

        public static int[] major_id;
        public static int[] minor_id;

        public static float[] balance;

        [STAThread]
        public unsafe bool Init()
        {
            if (DecaptcherInit() == -1) return false;

            id = CCprotoInit();

            if (id == -1) return false;

            ret = CCprotoLogin(id, "api.decaptcher.com", 18640, "mobster1930", 11, "123654987", 9);

            if (ret < 0) return false;

            balance = new float[1];
            fixed (float* balance1 = &balance[0])

            ret = CCprotoBalance(id, balance1);

            if (ret < 0) return false;

            return true;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public unsafe string DecodeCaptcha(string pathToPicture)
        {           
            int[] p_pict_to;
            int[] p_pict_type;
            int size_buf;            

            FileStream fs = new FileStream(pathToPicture, FileMode.Open);

            byte[] buffer = new byte[fs.Length];
            pic_size = (int)fs.Length;
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            
            byte[] captcha1 = new byte[256];
            p_pict_to = new int[1];
            p_pict_type = new int[1];

            major_id = new int[1];
            minor_id = new int[1];

            p_pict_to[0] = 0;
            p_pict_type[0] = 0;
            size_buf = 255;

            fixed (int* p_pict_to1 = &p_pict_to[0])
            fixed (int* p_pict_type1 = &p_pict_type[0])
            fixed (int* major_id1 = &major_id[0])
            fixed (int* minor_id1 = &minor_id[0])
            fixed (byte* captcha = &captcha1[0])
            fixed (byte* bufPass = &buffer[0])
            {
                //ret = CCprotoPicture(id, bufPass, pic_size, captcha);
                ret = CCprotoPicture2(id, bufPass, pic_size, p_pict_to1, p_pict_type1, captcha, size_buf, major_id1, minor_id1);
            }

            if (ret < 0)
                return "Something's wrong with decaptcha service at the moment :(";

            int g = 0;
            for (int i = 0; i < 256; i++)
            {
                if (captcha1[i] != 0x00)
                    g++;
            }

            byte[] captcha_real = new byte[g];

            for(int i = 0; i < g; i++)
            {
                captcha_real[i] = captcha1[i];
            }

            string s = new string(Encoding.ASCII.GetChars(captcha_real));
            
            // Return the captcha
            return s;

        }

        [STAThread]
        public unsafe void IncorrectCaptcha()
        {
            CCprotoPictureBad2(id, major_id[0], minor_id[0]);
        }

        [STAThread]
        public unsafe void Close()
        {
            CCprotoClose(id);
        }
    }
}
