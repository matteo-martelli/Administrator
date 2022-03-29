/*
using System.Diagnostics;
using System.IO;
using System;

namespace support_functions;

public static int[] DimFromString(string stringa)
{
    var array = new int[2];
    int j = 0;
    while(stringa[j] != ' ')
        j++;
        
    var heightchars = new char[j + 1];
    for (int i = 0; i<heightchars.Length; i++)
        heightchars[i] = stringa[i];


    var widthchars = new char[stringa.Length - j];
    for (int i = 0; i<widthchars.Length; i++)
        widthchars[i] = stringa[j + i];
        
    string heightstring = new string(heightchars);
    string widthstring = new string(widthchars);

    array[0] = Int32.Parse(heightstring);
    array[1] = Int32.Parse(widthstring);

    return array;
}
*/