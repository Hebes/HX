using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BuffData1 //: IBuffData
{
    private uint _id = 1;

    public string buffName => "1";

    public string describe => "1";

    public uint ID { get => _id; set => _id = value; }
}

