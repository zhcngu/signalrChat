using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SignalRClientApp
{
    public partial class Form1 : Form
    {
        HubConnection connection;
        public Form1()
        {
            InitializeComponent();
            var host = ConfigurationManager.AppSettings["signalrHost"];
            connection = new HubConnectionBuilder()
            .WithUrl($"{host}/StronglyTypedChatHub", ops => { ops.AccessTokenProvider = async () => { return await Task.FromResult(token); }; })
            .Build();
            connection.Closed += async (error) =>
            {
                //await connection.DisposeAsync();
                // connection=null;

                //await Task.Delay(new Random().Next(0, 5) * 1000);
                //await connection.StartAsync();
                this.Invoke(new Action(() => {
                    
                    btnConn.Enabled = true;
                    txtMsgs.Text += "服务端断开链接\r\n";
                }));
                await Task.CompletedTask;
            };
            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                this.Invoke(new Action(() =>
                {
                    var newMessage = $"{DateTime.Now.ToString()} {user}: {message}";
                    txtMsgs.Text += newMessage + "\r\n";
                }));
            });

            connection.On<string, string>("Notification", (user, message) =>
            {
                this.Invoke(new Action(() =>
                {
                    var newMessage = $"{DateTime.Now.ToString()} : {message}";
                    txtMsgs.Text += newMessage + "\r\n";
                }));
            });
        }
        string token = "";
        private async void btnlogin_Click(object sender, EventArgs e)
        {

            try
            {
                var host = ConfigurationManager.AppSettings["signalrHost"];
                var url = $"{host}/home/login?user={txtUser.Text}";
                HttpClient client = new HttpClient()
                {
                };
                token = await client.GetStringAsync(url);
                txtMsgs.Text += token + "\r\n";
                btnConn.Enabled = true;
                btnlogin.Enabled = false;
            }
            catch (Exception ex)
            {
                txtMsgs.Text += "登录失败" + ex.Message + "\r\n";
            }
        }

        private async void btnConn_Click(object sender, EventArgs e)
        {
            try
            {
                await connection.StartAsync();
                txtMsgs.Text += "Connection started" + "\r\n";
                await connection.InvokeAsync("Join",
                txtUser.Text, "winform");
                btnConn.Enabled = false;
                btnSend.Enabled = true;
            }
            catch (Exception ex)
            {
                txtMsgs.Text += ex.Message + "\r\n";
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                await connection.InvokeAsync("SendMessage",
                    txtUser.Text, txtUserMsg.Text);
            }
            catch (Exception ex)
            {
                txtMsgs.Text += ex.Message + "\r\n";
            }
        }
    }
}
