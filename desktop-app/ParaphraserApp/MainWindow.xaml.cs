using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Modules;
using RestSharp;
using RestSharp.Authenticators;

namespace ParaphraserApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
 

    public partial class MainWindow : Window
    {

        RestClient client = new RestClient("http://127.0.0.1:8000/api/predict");

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_paraphrase_Click(object sender, RoutedEventArgs e)
        {
            //check if input exist
            if (!tb_input.Text.Equals(string.Empty))
            {
                // post reuqest to the api
                var request = new RestRequest(Method.POST);

                request.RequestFormat = RestSharp.DataFormat.Json;

                request.AddBody(new {text = tb_input.Text });

                var result = client.Post(request);
                var response = client.Execute(request);

                HttpStatusCode statusCode = response.StatusCode;

                //check that the api request is a success
                if ((int)statusCode == 200)
                {
                    tb_output.Text = string.Empty;
                    using (var reader = new JsonTextReader(new StringReader(response.Content)))
                    {

                        int line_number = 1;
                        while (reader.Read())
                        {
                            if (reader.ValueType == typeof(string))
                            {
                                if (!reader.Value.Equals("response"))
                                {
                                    //write output
                                    tb_output.Text += line_number + "- " + reader.Value + "\n\n";
                                    line_number++;
                                }
                            }
                        }
                    }

                }
                else MessageBox.Show("Please start the Model Service");
            }
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            //clear textboxes
            tb_output.Text = string.Empty;
            tb_input.Text = string.Empty;
        }
    }
}
