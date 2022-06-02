using System;
using System.Text;
using System.Web;
using System.Drawing;
using System.Security.Cryptography;
/// dependency: OtpNet
using OtpNet;
/// dependency: QRCoder
using QRCoder;

namespace jlib {

	class Crypto
	{
		public static string ComputeMd5(string s)
		{
			MD5 md5 = MD5.Create();
			byte[] bs = md5.ComputeHash(Encoding.UTF8.GetBytes(s));
			return Encrypt.ToString(bs);
		}

        public static string Hex(byte n)
        {
            return string.Format("{0:X2}", n);
        }
	}

	class Encrypt
	{
		public static string ToString(byte[] bs)
		{
			string _r = "";
			for (int i = 0; i < bs.Length; i++)
			{
				_r += bs[i].ToString("X");
			}
			return _r;
		}

		public static byte[] HexToBytes(string hex)
		{
			if (hex.Length % 2 == 1)
			{
				throw new Exception($"Bytes sequence error: {hex}");
			}
			byte[] _r = new byte[hex.Length / 2];
			for (int i = 0; i < hex.Length; i = i + 2)
			{
				_r[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
			}
			return _r;
		}

		public static string EncodeUtf8(string s)
		{
			return ToString(Encoding.UTF8.GetBytes(s));
		}

		public static string DecodeUtf8(string s)
		{
			return Encoding.UTF8.GetString(HexToBytes(s));
		}

		public static string ComputeJustKey(string secret)
		{
			return secret + jlib.Platform.GetCpuSeq() + jlib.Time.Format(jlib.Time.GetMilliSeconds());
		}

		public static bool JustValidate(string justKey, string dynamicKey)
		{
			long ms = jlib.Time.Parse(justKey.Substring(justKey.Length - 19, 19));
			string calculated = new Totp(Encoding.UTF8.GetBytes(justKey), 30, OtpHashMode.Sha1, 6)
				.ComputeTotp(jlib.Time.Convert(ms / 1000 * 1000));
			return calculated.Equals(dynamicKey);
		}

		public static string ComputeJustKey(string justKey, int beginHours, int interval)
		{
			string stdTime = jlib.Time.Format(jlib.Time.GetMilliSeconds());
			long ms = jlib.Time.Parse(stdTime) + (8 - beginHours) * 60 * 60 * 1000;
			return new Totp(Encoding.UTF8.GetBytes(justKey), interval * 60 * 60, OtpHashMode.Sha1, 6).ComputeTotp(jlib.Time.Convert(ms));
		}

		private static QRCodeGenerator qrGenerator = new QRCodeGenerator();

		public static void EncodeQRCoder(string code, string path)
		{
			QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
			QRCode qrcode = new QRCode(qrCodeData);
			Bitmap qrCodeImage = qrcode.GetGraphic(5, System.Drawing.Color.Black, System.Drawing.Color.White, null, 15, 3);
			qrCodeImage.Save(path, System.Drawing.Imaging.ImageFormat.Png);
		}
	}

}