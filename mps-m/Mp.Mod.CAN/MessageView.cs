using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Mp.Mod.CAN
{
    public partial class MessageView : UserControl
    {
        private List<List<Panel>> _view = new List<List<Panel>>();
        public MessageView()
        {
            InitializeComponent();
            repText.Text = "                    ";

            for (int row = 0; row < 8; ++row)
                _view.Add(new List<Panel>());


            _view[0].Add(p00);
            _view[0].Add(p10);
            _view[0].Add(p20);
            _view[0].Add(p30);
            _view[0].Add(p40);
            _view[0].Add(p50);
            _view[0].Add(p60);
            _view[0].Add(p70);


            _view[1].Add(p01);
            _view[1].Add(p11);
            _view[1].Add(p21);
            _view[1].Add(p31);
            _view[1].Add(p41);
            _view[1].Add(p51);
            _view[1].Add(p61);
            _view[1].Add(p71);

            _view[2].Add(p02);
            _view[2].Add(p12);
            _view[2].Add(p22);
            _view[2].Add(p32);
            _view[2].Add(p42);
            _view[2].Add(p52);
            _view[2].Add(p62);
            _view[2].Add(p72);

            _view[3].Add(p03);
            _view[3].Add(p13);
            _view[3].Add(p23);
            _view[3].Add(p33);
            _view[3].Add(p43);
            _view[3].Add(p53);
            _view[3].Add(p63);
            _view[3].Add(p73);

            _view[4].Add(p04);
            _view[4].Add(p14);
            _view[4].Add(p24);
            _view[4].Add(p34);
            _view[4].Add(p44);
            _view[4].Add(p54);
            _view[4].Add(p64);
            _view[4].Add(p74);

            _view[5].Add(p05);
            _view[5].Add(p15);
            _view[5].Add(p25);
            _view[5].Add(p35);
            _view[5].Add(p45);
            _view[5].Add(p55);
            _view[5].Add(p65);
            _view[5].Add(p75);

            _view[6].Add(p06);
            _view[6].Add(p16);
            _view[6].Add(p26);
            _view[6].Add(p36);
            _view[6].Add(p46);
            _view[6].Add(p56);
            _view[6].Add(p66);
            _view[6].Add(p76);

            _view[7].Add(p07);
            _view[7].Add(p17);
            _view[7].Add(p27);
            _view[7].Add(p37);
            _view[7].Add(p47);
            _view[7].Add(p57);
            _view[7].Add(p67);
            _view[7].Add(p77);
        }


        public void Clear()
        {
            repText.Text = "                    ";
            for (int row = 0; row < 8; ++row)
                for (int col = 0; col < 8; ++col)
                    _view[row][col].BackColor = Color.Olive;
        }

        public void SetBit(int byteIdx, int bitIdx)
        {
            try
            {
                _view[byteIdx][bitIdx].BackColor = Color.YellowGreen;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void SetPivotBit(int bitIdx, bool intel)
        {
            int fromByte = bitIdx / 8;

            try
            {
                _view[fromByte][bitIdx % 8].BackColor = Color.Yellow;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if(intel)
                repText.Text = "Intel Standard";
            else
                repText.Text = "Motorola Forward MSB";
        }

        public void SetRange(int pivot, int count, bool intel)
        {
            if (intel)
            {
                int fromByte = pivot / 8;
                int toByte = ((pivot + count) / 8) + 1;

                int startBit = pivot % 8;
                int lenght = startBit + count;

                for (int i = fromByte; i < toByte; ++i)
                {
                    for (int j = startBit; j < Math.Min(lenght, 8); ++j)
                        SetBit(i, j);

                    lenght -= 8;
                    if (count < 0)
                        return;

                    startBit = 0;
                }
            }
            else
            {
                int msb = pivot / 8;
                int startBitInLsb = pivot % 8 + 1;
                int fill = Math.Min(startBitInLsb, count);

                //fill msb
                for (int i = startBitInLsb - fill; i < startBitInLsb; ++i)
                    SetBit(msb, i);

                int restOfBits = count - startBitInLsb;                
                msb++;
                if (msb >= 8)
                    return;

                int bit = 7;

                for( int j = 0; j < restOfBits; ++j)
                {
                    SetBit(msb, bit);
                    bit--;
                    if( bit == -1)
                    {
                        msb++;
                        if (msb >= 8)
                            return;

                        bit = 7;
                    }                    
                }               
            }
        }
    }
}
