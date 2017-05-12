using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Media;



namespace MusicApp
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\1691713\Downloads\Compressed\MusicApp\MusicApp\MusicAlbum.mdf;Integrated Security=True");
        string[] doc, path;
        int currenttrack=0;
        int list_count = 0;
        int count_file ;

         
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ListTracks_SelectedIndexChanged(object ListThatGotClicked, EventArgs e)
        {
            
            try
            {
                ListBox currentSelectedList = (ListBox)ListThatGotClicked;
                //MessageBox.Show(currentSelectedList.SelectedItem.ToString());
                //WMD.URL = @path[currentSelectedList.SelectedIndex];
                WMD.URL = currentSelectedList.SelectedItem.ToString();
            }
            catch (IndexOutOfRangeException r)
            {

                
            }
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ListBox.ObjectCollection list = ListTracks.Items;
            Random random = new Random();
            int w = list.Count;
            ListTracks.BeginUpdate();
            while (w>1)
            {
                w--;
                int u = random.Next(w + 1);
                object value = list[u];
                list[u] = list[w];
                list[w] = value;
            }
            ListTracks.EndUpdate();
            ListTracks.Invalidate();


        }

        private void butclose(object sender, EventArgs e)
        {
            this.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(openfile.Text)) // checks the file
                {

                    FileStream file = new FileStream(openfile.Text, FileMode.Open);
                    byte[] songdata = new byte[file.Length];
                    string path=file.Name;
                    path = "'" + path + "'";
                    string category="'"+Cat.Text+"'";
                    file.Close();

                    
                    con.Open();
                    // Executing the command to insert song
                    SqlCommand cmd = new SqlCommand("INSERT INTO Song (Songname,category) VALUES(" + path + ","+category+")", con);
                    //cmd.Parameters.AddWithValue("@song", songdata);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Inserted Successfully.");
                    con.Close();
                }
                else
                {
                    MessageBox.Show("Please choose the file first.");
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
            }
        
    }

        public void AddSong(string filePath)
        {
            try
            {
                string path = filePath;
                path = "'" + path + "'";

                con.Open();
                // Executing the command to insert song
                SqlCommand cmd = new SqlCommand("INSERT INTO Song (Songname,category) VALUES(" + path + "," + "'Rap'" + ")", con);
                //cmd.Parameters.AddWithValue("@song", songdata);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Inserted Successfully.");
                con.Close();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
            }
        }

        private void WMD_Enter(object sender, EventArgs e)
        {

        }

        private void PlayList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                WMD.URL = path[PlayList.SelectedIndex];

            }
            catch (IndexOutOfRangeException r)
            {

                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection sqlCon = null;
            string connection = "";
            try
            {
                connection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\1691713\Downloads\Compressed\MusicApp\MusicApp\MusicAlbum.mdf;Integrated Security=True";
                con = new SqlConnection(connection);
                sqlCon.Open();

                //***READ FROM THE DATABASE***//
                string temp = PlayList.SelectedItem.ToString();
                string query2 = "DELETE FROM Song WHERE Songname = '" + temp + "'";
                SqlCommand view = new SqlCommand(query2, sqlCon);
                SqlDataReader dataReader = view.ExecuteReader();
                //PlayList.Items.Remove(PlayList.SelectedItem);
                //PlayList.SelectedIndex = 0;
            }
            catch (Exception)
            {

                //ListTracks.Items.Remove(ListTracks.SelectedItem);
                //ListTracks.SelectedIndex = -1;
            }
        }

        private void Nexttrack(object sender, EventArgs e)
        {
           //try
           //{
           //    count_file = PlayList.Items.Count;
           //    if (count_file <= 0)
           //    {
           //        MessageBox.Show("No file to select. ", "Message", MessageBoxButtons.OK);
           //    }
           //    else
           //    {
           //        if (currenttrack >= list_count - 1)
           //        {
             //           currenttrack = 0;
             //           ListTracks.SelectedIndex = currenttrack;
             //       }
             //       else
             //       {
             //           currenttrack += 1;
             //
             //           ListTracks.SelectedIndex = currenttrack;
             //       }
               // }

           // }
           // catch (Exception ea)
           // {
           //
           //     
           // }
           // 
            //WMD.Ctlcontrols.next();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Mp3 Files|*.mp3|Avi Files|*.avi|Wav Files|*.wav";
            if (open.ShowDialog() == DialogResult.OK)
            {
                openfile.Text = open.FileName;
            }
        }

        private void Ref_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\1691713\Downloads\Compressed\MusicApp\MusicApp\MusicAlbum.mdf;Integrated Security=True");
            try     //TRY Some code and if about to crash because of error,... do something
            {
                
                con.Open();
                

                //***READ FROM THE DATABASE***//

                string query2 = "SELECT * FROM Song";
                SqlCommand view = new SqlCommand(query2, con);
                SqlDataReader dataReader = view.ExecuteReader();

                PlayList.Items.Clear();
                while (dataReader.Read())   //Advance to next row
                {
                    PlayList.Items.Add(dataReader.GetValue(1).ToString());
                }
            }
            catch (Exception ee)     //Caught an error
            {
            }
            finally     //Well, either way(error or not) do this
            {
                con.Close();    //Close connection so others can use the database
            }
        }

        private void openfile_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
           // try
           // {
           //     count_file = PlayList.Items.Count;
           //     if (count_file <= 0)
           //     {
           //         MessageBox.Show("No file to select. ", "Message", MessageBoxButtons.OK);
           //     }
           //     else
           //     {
           //         if (currenttrack <=0)
           //         {
           //             currenttrack = list_count-1;
           //             ListTracks.SelectedIndex = currenttrack;
           //         }
           //         else
           //         {
           //             currenttrack -= 1;
           //
         //              ListTracks.SelectedIndex = currenttrack;
         //          }
         //      }
         //  }
         //  catch (Exception ea)
         //  {
         //
         //      
         //  }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = true;
            open.Filter = "Mp3 Files|*.mp3|Avi Files|*.avi|Wav Files|*.wav";
            if (open.ShowDialog() == DialogResult.OK)
            {
                doc = open.SafeFileNames;
                path = open.FileNames;
                for (int i = 0; i < doc.Length; i++)
                {
                    PlayList.Items.Add(doc[i]);
                    AddSong(doc[i]);
                    //list_count = ListTracks.Items.Count;
                    //currenttrack = 0;
                    //PlayList.SelectedIndex = currenttrack;
                }
            }
            
        }
    }
}
