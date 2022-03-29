using System.Diagnostics;
using System.IO;
using System;
using lib;

class Program
{
    static void Main()
    {
        /*
        Console.WriteLine("inserire altezza");
        int height = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("inserire lunghezza");
        int width = Convert.ToInt32(Console.ReadLine());
        var img = new HdrImage(height, width);
        Console.WriteLine("pixel random:");
        Console.WriteLine("{0}, {1}, {2}", img.GetPixel(3, 3).r, img.GetPixel(3, 3).g, img.GetPixel(3, 3).b);

        Console.WriteLine("inserire coordinate pixel da impostare");
        int r = Convert.ToInt32(Console.ReadLine());
        int c = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("inserire valori r, g, b");
        float red = float.Parse(Console.ReadLine());
        float green = float.Parse(Console.ReadLine());
        float blue = float.Parse(Console.ReadLine());
        var colore = new Color(red, green, blue);

        img.SetPixel(colore, r, c);
        Console.WriteLine("pix vale {0}, {1}, {2}", img.GetPixel(r, c).r, img.GetPixel(r, c).g, img.GetPixel(r, c).b);

        //TEST
        var color1 = new Color(1.3f, 2.4f, 5.5f);
        var color2 = new Color(1.7f, 3.6f, 88.5f);

        var color3 = new Color();
        color3 = Color.Sum(color1, color2);
        var color4 = new Color(3.0f, 6.0f, 104.0f);
        //Debug.Assert(color3 == color4);
        */

        string percorso = @"c:\Users\Administrator\Downloads\reference_be.pfm";
        var immagine = new HdrImage(percorso);
        
        string percorso_destinazione = @"c:\Users\Administrator\Downloads\prova.pfm";
        immagine.SaveHdrImage(percorso_destinazione);

        string percorso1 = @"c:\Users\Administrator\Downloads\prova.pfm";
        var immagine1 = new HdrImage(percorso1);

        for (int i = 0; i < immagine1.pixels.Length; i++)
        {
            Console.WriteLine("rosso {0}", immagine1.pixels[i].r);
            Console.WriteLine("verde {0}", immagine1.pixels[i].g);
            Console.WriteLine("blu {0}", immagine1.pixels[i].b);
            Console.WriteLine(" ");
        }

        Console.WriteLine("\n\n\n");
        float l = immagine1.Avg_Lum();
        immagine1.NormalizeImg(0.38f, l);
        immagine1.ClampImg();
        for (int i = 0; i < immagine1.pixels.Length; i++)
        {
            Console.WriteLine("rosso {0}", immagine1.pixels[i].r);
            Console.WriteLine("verde {0}", immagine1.pixels[i].g);
            Console.WriteLine("blu {0}", immagine1.pixels[i].b);
            Console.WriteLine(" ");
        }
        


        /*
        var fs = new FileStream(percorso, FileMode.Open);

        var array1 = new float[6];
        var array2 = new float[6];
        var array3 = new float[6];

        byte[] tone = new byte[4];
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 4; j++)
                tone[j] = Convert.ToByte(fs.ReadByte());
            array1[i] = BitConverter.ToSingle(tone, 0);

            for (int j = 0; j < 4; j++)
                tone[j] = Convert.ToByte(fs.ReadByte());
            array2[i] = BitConverter.ToSingle(tone, 0);

            for (int j = 0; j < 4; j++)
                tone[j] = Convert.ToByte(fs.ReadByte());
            array3[i] = BitConverter.ToSingle(tone, 0);

        } 

        fs.Close();
        */
    }
}

