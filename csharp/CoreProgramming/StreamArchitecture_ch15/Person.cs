namespace StreamArchitecture_ch15;

public class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public int Height { get; set; }

    public void SaveData(Stream stream)
    {
        BinaryWriter writer = new BinaryWriter(stream);

        writer.Write(Name);
        writer.Write(Age);
        writer.Write(Height);

        writer.Flush();
    }

    public void LoadData(Stream stream)
    {
        BinaryReader reader = new BinaryReader(stream);

        Name = reader.ReadString() + "test";
        Age = reader.ReadInt32() + 100;
        Height = reader.ReadInt32() + 200;
    }
}
