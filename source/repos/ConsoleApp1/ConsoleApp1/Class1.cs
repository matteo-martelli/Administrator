using System.Text;
namespace metodiImmagini;

public class metodi
{
    public StreamReader Read = new StreamReader("reference_le.pfm");
    public bool magic()
    {
        if(Read.Readline() == "PF")
            return true;
        else 
            return false;   
    }
    public int[] res()
    {
        string temp_resolution = Read.Readline();
        string[] r =line.Split(' ');
        int[] resolution = new int[2];
        resolution[0] = int.Parse(r[0]);
        resolution[1] = int.Parse(r[1]);
        return resolution;
    }
}
