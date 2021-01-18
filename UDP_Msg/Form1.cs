using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;


namespace UDP_Msg
{
	public partial class Form1 : Form
	{
		bool alive = false; // будет ли работать поток для приема
		UdpClient _client;
		UdpClient _encClient;
		const int _localPort = 8001; // порт для приема сообщений
		const int _remotePort = 8001; // порт для отправки сообщений

		const int _remoteEncPort = 8002; // порт для отправки зашифрованных сообщений
		const int _localEncPort = 8002;  // порт для приема зашифрованных сообщений

		const int _ttl = 20;
		const string _host = "235.5.5.1"; // хост для групповой рассылки
		IPAddress _groupAddress; // адрес для групповой рассылки
		string _localIp;  // ip хоста
		string _userName; // имя пользователя в чате

		RSACryptoServiceProvider _rsa_For_Decr = new RSACryptoServiceProvider(); //для дешифровки полученных сообщений
		RSACryptoServiceProvider _rsa_For_Encr = new RSACryptoServiceProvider();  //для шифрования сообщений перед отправкой
		RSAParameters _rsaKeyParameters = new RSAParameters(); // для отобржания параметров ключей

		Dictionary<IPAddress, byte[]> _contacts = new Dictionary<IPAddress, byte[]>();// участники чата

		public Form1()
		{
			InitializeComponent();

			loginButton.Enabled = true; // кнопка входа
			logoutButton.Enabled = false; // кнопка выхода
			sendButton.Enabled = false; // кнопка отправки
			chatTextBox.ReadOnly = true; // поле для сообщений

			_groupAddress = IPAddress.Parse(_host);
			_localIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Last().ToString();
			_rsaKeyParameters = _rsa_For_Decr.ExportParameters(true);

			UnicodeEncoding ByteConverter = new UnicodeEncoding();
			privateKeyTextBox.Text = ByteConverter.GetString(_rsaKeyParameters.D);
			publicKeyTextBox.Text = ByteConverter.GetString(_rsaKeyParameters.Modulus);

		}

		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void loginButton_Click(object sender, EventArgs e)
		{
			_userName = userNameTextBox.Text;
			userNameTextBox.ReadOnly = true;

			try
			{
				_client = new UdpClient(_localPort);
				_encClient = new UdpClient(_localEncPort);
				// присоединяемся к групповой рассылке
				_client.JoinMulticastGroup(_groupAddress, _ttl);

				// запускаем задачу на прием сообщений
				Task receiveTask = new Task(ReceiveMessages);
				receiveTask.Start();

				//Запускаем задачу на прием шифрованных сообщений
				Task receiveTaskenc = new Task(ReceiveMessagesEnc);
				receiveTaskenc.Start();

				// отправляем первое сообщение о входе нового пользователя
				string message = _userName + " вошел в чат";
				byte[] data = Encoding.Unicode.GetBytes(message);
				_client.Send(data, data.Length, _host, _remotePort); // оповещаем участников чата
				SendOpenKey();	//отправляем свой открытый ключ

				loginButton.Enabled = false;
				logoutButton.Enabled = true;
				sendButton.Enabled = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		void SendOpenKey()
		{
			_rsaKeyParameters = _rsa_For_Decr.ExportParameters(false); // только открытые параметры
			List<byte> exponent = new List<byte>(_rsaKeyParameters.Exponent);
			List<byte> moduls = new List<byte>(_rsaKeyParameters.Modulus);
			exponent.AddRange(moduls);
			byte[] data = exponent.ToArray();
			_client.Send(data, data.Length, _host, _remotePort);
		}

		/// <summary>
		/// для получения незашифрованных сообщений
		/// 1) вошел новый участник -> отправил ему открытый ключ
		/// 2) если участник покинул чат -> удаляем его из рассылки
		/// 3) получение открытого ключа -> добавляем контакт <ip,key>
		/// </summary>
		private void ReceiveMessages()
		{
			alive = true;
			try
			{
				while (alive)
				{
					IPEndPoint remoteIp = null;
					byte[] data = _client.Receive(ref remoteIp);
					string message = Encoding.Unicode.GetString(data);

					// добавляем полученное сообщение в текстовое поле
					this.Invoke(new MethodInvoker(() =>
					{
						if(message.EndsWith("вошел в чат"))
						{
							string time = DateTime.Now.ToShortTimeString();
							chatTextBox.AppendText(time + " | " + remoteIp.Address.ToString() + " | " + message + "\r\n");
							if (remoteIp.Address.ToString() != _localIp) // для новых пользователей отправляем свой открытый ключ
								SendOpenKey();
						}
						else if(message.EndsWith("покидает чат"))
						{
							_contacts.Remove(remoteIp.Address);
							string time = DateTime.Now.ToShortTimeString();
							chatTextBox.AppendText(time + " | " + message + "\r\n");
							chatTextBox.AppendText(time + " | " + "Удален ключ для: " + remoteIp.Address.ToString() + "\r\n");
						}
						else
						{
							_contacts[remoteIp.Address] = data;
							string time = DateTime.Now.ToShortTimeString();
							chatTextBox.AppendText(time + " | " + $"Получен ключ({data.Length}) от: " + remoteIp.Address.ToString() + "\r\n");
						}
					}));
				}
			}
			catch (ObjectDisposedException)
			{
				if (!alive)
					return;
				throw;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void ReceiveMessagesEnc()
		{
			alive = true;
			try
			{
				while (alive)
				{
					IPEndPoint remoteIp = null;
					byte[] data = _encClient.Receive(ref remoteIp); // прием зашифрованных сообщений
					string message = Encoding.Unicode.GetString(data);

					this.Invoke(new MethodInvoker(() =>
					{
						//Получаем зашифрованное сообщение, выводим исходное и расшифрованное
						string time = DateTime.Now.ToShortTimeString();

						chatTextBox.AppendText(time + " | " + remoteIp.Address.ToString() + " |Исходное| " + message + "\r\n");

						data = RSADecrypt(data, _rsa_For_Decr.ExportParameters(true), false);
						message = Encoding.Unicode.GetString(data);
						chatTextBox.AppendText(time + " | " + remoteIp.Address.ToString() + " |Расшифрованное| " + message + "\r\n");

					}));

				}
			}
			catch (ObjectDisposedException)
			{
				if (!alive)
					return;
				throw;
			}
			catch (SocketException)
			{
				if (!alive)
					return;
				throw;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		private void sendButton_Click(object sender, EventArgs e)
		{
			try
			{
				
				string message = String.Format("{0}: {1}", _userName, messageTextBox.Text);
				foreach (var item in _contacts)
				{
					Console.WriteLine(item.Key + " - " + item.Value);
					byte[] data = Encoding.Unicode.GetBytes(message);
					_rsaKeyParameters = _rsa_For_Encr.ExportParameters(false);
					_rsaKeyParameters.Exponent = item.Value.Take(3).ToArray();
					_rsaKeyParameters.Modulus = item.Value.Skip(3).ToArray();
					_rsa_For_Encr.ImportParameters(_rsaKeyParameters);
					data = RSAEncrypt(data, _rsa_For_Encr.ExportParameters(false), false);
					_encClient.Send(data, data.Length, item.Key.ToString(), _remoteEncPort);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		// обработчик нажатия кнопки logoutButton
		private void logoutButton_Click(object sender, EventArgs e)
		{
			ExitChat();
		}
		// выход из чата
		private void ExitChat()
		{
			string message = _userName + " покидает чат";
			byte[] data = Encoding.Unicode.GetBytes(message);
			_client.Send(data, data.Length, _host, _remotePort);
			_client.DropMulticastGroup(_groupAddress);

			alive = false;
			_encClient.Close();
			_client.Close();

			loginButton.Enabled = true;
			logoutButton.Enabled = false;
			sendButton.Enabled = false;
		}
		// обработчик события закрытия формы
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (alive)
				ExitChat();
		}

		//Шифрование RSA
		public static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
		{
			try
			{
				byte[] encryptedData;
				//Create a new instance of RSACryptoServiceProvider.
				using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
				{

					//Import the RSA Key information. This only needs
					//toinclude the public key information.
					RSA.ImportParameters(RSAKeyInfo);

					//Encrypt the passed byte array and specify OAEP padding.  
					//OAEP padding is only available on Microsoft Windows XP or
					//later.  
					encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
				}
				return encryptedData;
			}
			//Catch and display a CryptographicException  
			//to the console.
			catch (CryptographicException e)
			{
				MessageBox.Show(e.Message);
				return null;
			}
		}
		public static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
		{
			try
			{
				byte[] decryptedData;
				//Create a new instance of RSACryptoServiceProvider.
				using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
				{
					//Import the RSA Key information. This needs
					//to include the private key information.
					RSA.ImportParameters(RSAKeyInfo);

					//Decrypt the passed byte array and specify OAEP padding.  
					//OAEP padding is only available on Microsoft Windows XP or
					//later.  
					decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
				}
				return decryptedData;
			}
			//Catch and display a CryptographicException  
			//to the console.
			catch (CryptographicException e)
			{
				MessageBox.Show(e.ToString());
				return null;
			}
		}

		private void label2_Click(object sender, EventArgs e)
		{

		}

		private void logoutButton_Click_1(object sender, EventArgs e)
		{
			ExitChat();
		}
	}
}
