using System.Diagnostics;
using System.IO;
using System.Text;
using System;

namespace lib;
public struct Color
{
	//MEMBERS
	public float r, g, b;

	//COSTRUTTORE DEFAULT
	public Color()
	{
		r = 0f;
		g = 0f;
		b = 0f;
	}

	//COSTRUTTORE 
	public Color(float x, float y, float z)
    {
		r = x;
		g = y;
		b = z;
    }

	//METODI
	public static Color Sum(Color x, Color y)
    {
		var c = new Color(x.r + y.r, x.g + y.b, x.b + y.b);
		return c;
    }

	public static Color SMult(Color x, float a)
    {
		var c = new Color(a*x.r, a*x.g, a*x.b);
		return c;
	}

	public static Color CMult(Color x, Color y)
    {
		var c = new Color(x.r * y.r, x.g * y.b, x.b * y.b);
		return c;
	}

	public float Luminosity()
    {
		return (Math.Max(this.r, Math.Max(this.g, this.b)) + Math.Min(this.r, Math.Min(this.g, this.b))) / 2f;
    }
}

public class HdrImage 	
{
	//MEMBERS
	public int w;
	public int h;

	public Color[] pixels;

	//COSTRUTTORE BASE
	public HdrImage(int a, int b)
    {
		w = a;
		h = b;
		pixels = new Color[a*b];
    }

	//COSTRUTTORE DA PATH
	public HdrImage(string path)
    {
		//apertura stream e inizializzazione
		int offset = 0;
		var fs = new FileStream(path, FileMode.Open);
		var sr = new StreamReader(fs);

		//riga 1
		string magic = sr.ReadLine();
		offset = offset + magic.Length;
		if (magic != "PF")
			Console.WriteLine("il file non e' PFM");
        
		//riga2
		string dim = sr.ReadLine();
		offset = offset + dim.Length;

		//riga3
		string endianness_string = sr.ReadLine();
		offset = offset + endianness_string.Length;
		bool endianness;
		if (endianness_string == "1.0")
			endianness = true;
		else
			endianness = false;

		//istanziazione membri
		w = HdrImage.StringToDim(dim)[0];
		h = HdrImage.StringToDim(dim)[1];
		pixels = new Color[h * w];

		//lettura bytes
		HdrImage.MyByteReader(w, h, endianness, offset, fs, ref pixels);
		Array.Reverse(pixels);
		HdrImage.FlipImg(w, h, ref pixels);

		//chiusura 
		sr.Close();
		fs.Close();
    }

	//METODI
	public Color GetPixel(int row, int col)
    {
		return pixels[w*row + col];
    }
	
	public void SetPixel(Color val, int row, int col)
    {
		pixels[w * row + col] = val;
	}

	public void SaveHdrImage(string path)
    {
		//prep
		float endianness_value = 1.0f;
		if (BitConverter.IsLittleEndian == true)
			endianness_value = -1.0f;
		HdrImage.FlipImg(w, h, ref pixels);
		Array.Reverse(pixels);
		var header = Encoding.ASCII.GetBytes($"PF\n{w} {h}\n{endianness_value}\n");
		var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
		
		//scrittura
		fs.Write(header, 0, header.Length);
        for (int i = 0; i < w * h; i++)
        {
			HdrImage.WriteFloat(fs, pixels[i].r);
			HdrImage.WriteFloat(fs, pixels[i].g);
			HdrImage.WriteFloat(fs, pixels[i].b);
		}

		//chiusura
		fs.Close();
	}

	public float Avg_Lum()
    {
		float delta = 1e-10f;
		float sum = 0.0f;
		for (int i = 0; i < this.pixels.Length; i++)
			sum = sum + (float)Math.Log10(delta + this.pixels[i].Luminosity());
		sum = sum / ((float)this.pixels.Length);
		return (float)Math.Pow(10, sum);
    }

	public void NormalizeImg(float a, float l)
    {
		for (int i = 0; i < this.pixels.Length; i++)
        {
			this.pixels[i].r = a* this.pixels[i].r / l;
			this.pixels[i].g = a * this.pixels[i].g / l;
			this.pixels[i].b = a * this.pixels[i].b / l;
		}

	}

	public void ClampImg()
    {
		for (int i = 0; i < this.pixels.Length; i++)
		{
			this.pixels[i].r = HdrImage.Clamp(this.pixels[i].r);
			this.pixels[i].g = HdrImage.Clamp(this.pixels[i].g);
			this.pixels[i].b = HdrImage.Clamp(this.pixels[i].b);
		}
	}

	//METODI PRIVATI
	private static int[] StringToDim(string stringa)
    {
		var array = new int[2];
		int j = 0;
		while (stringa[j] != ' ')
			j++;

		var heightchars = new char[j + 1];
		for (int i = 0; i < heightchars.Length; i++)
			heightchars[i] = stringa[i];

		var widthchars = new char[stringa.Length - j];
		for (int i = 0; i < widthchars.Length; i++)
			widthchars[i] = stringa[j + i];

		string heightstring = new string(heightchars);
		string widthstring = new string(widthchars);

		array[0] = Int32.Parse(heightstring);
		array[1] = Int32.Parse(widthstring);

		return array;
	}
	private static void MyByteReader(int width, int height, bool end, int off, FileStream stream, ref Color[] pix)
	{
		stream.Seek(off + 3, SeekOrigin.Begin);
		byte[] tone = new byte[4];

		for (int i = 0; i < width * height; i++)
		{
			for (int j = 0; j < 4; j++)
				tone[j] = Convert.ToByte(stream.ReadByte());
			if (end != BitConverter.IsLittleEndian)
				Array.Reverse(tone);
			pix[i].r = BitConverter.ToSingle(tone, 0); 

			for (int j = 0; j < 4; j++)
				tone[j] = Convert.ToByte(stream.ReadByte());
			if (end != BitConverter.IsLittleEndian)
				Array.Reverse(tone);
			pix[i].g = BitConverter.ToSingle(tone, 0);

			for (int j = 0; j < 4; j++)
				tone[j] = Convert.ToByte(stream.ReadByte());
			if (end != BitConverter.IsLittleEndian)
				Array.Reverse(tone);
			pix[i].b = BitConverter.ToSingle(tone, 0);
		}
	}
	private static void FlipImg(int width, int height, ref Color[] pix)
    {
		var pix_copy = new Color[pix.Length];
		for (int i = 0; i < pix.Length; i++)
        {
			pix_copy[i].r = pix[i].r;
			pix_copy[i].g = pix[i].g;
			pix_copy[i].b = pix[i].b;
        }
		
		for (int j = 0; j < height; j++)
		{
			for (int i = 0; i < width; i++)
			{
				pix[i + j * width].r = pix_copy[width*(j+1) - i - 1].r;
				pix[i + j * width].g = pix_copy[width*(j+1) - i - 1].g;
				pix[i + j * width].b = pix_copy[width*(j+1) - i - 1].b;
			}
		}
	}
	private static void WriteFloat(FileStream output, float val)
	{
		byte[] seq = BitConverter.GetBytes(val);
		output.Write(seq, 0, seq.Length);
	}
	private static float Clamp(float a)
    {
		return a / (a + 1f);
	}
}



/*
	for (int i = 0; i < pixels.Length; i++)
	{
		Console.WriteLine("rosso {0}", pixels[i].r);
		Console.WriteLine("verde {0}", pixels[i].g);
		Console.WriteLine("blu {0}", pixels[i].b);
		Console.WriteLine(" ");
	}
*/


/*
private static void MyByteReader(int width, int height, bool end, int off, FileStream stream, ref Color[] pix)
    {
		stream.Seek(off + 3, SeekOrigin.Begin);
		byte[] tone = new byte[4];

		for (int i = 0; i < width * height; i++)
		{
			for (int j = 0; j < 4; j++)
				tone[j] = Convert.ToByte(stream.ReadByte());
			if(end == true)
				Array.Reverse(tone);
			pix[i].r = BitConverter.ToSingle(tone, 0);

			for (int j = 0; j < 4; j++)
				tone[j] = Convert.ToByte(stream.ReadByte());
			if (end == true)
				Array.Reverse(tone);
			pix[i].g = BitConverter.ToSingle(tone, 0);

			for (int j = 0; j < 4; j++)
				tone[j] = Convert.ToByte(stream.ReadByte());
			if (end == true)
				Array.Reverse(tone);
			pix[i].b = BitConverter.ToSingle(tone, 0);
		}

		//Array.Reverse(pix);
		//HdrImage.FlipImg(width, height, ref pix);
	}
*/
