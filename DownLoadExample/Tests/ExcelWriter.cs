using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DownLoadExample.Tests
{
    public class ExcelWriter
    {
        private Stream stream;
        private BinaryWriter writer;

        private ushort[] clBegin = { 0x0809, 8, 0, 0x10, 0, 0 };
        private ushort[] clEnd = { 0x0A, 00 };

        private void WriteUshortArray(ushort[] value)
        {
            for (int i = 0; i < value.Length; i++)
                writer.Write(value[i]);
        }
        public ExcelWriter(Stream stream)
        {
            this.stream = stream;
            writer = new BinaryWriter(stream);
        }
        public void WriteCell(int row, int col, string value)
        {
            ushort[] clData = { 0x0204, 0, 0, 0, 0, 0 };
            int iLen = value.Length;
            byte[] plainText = Encoding.ASCII.GetBytes(value);
            clData[1] = (ushort)(8 + iLen);
            clData[2] = (ushort)row;
            clData[3] = (ushort)col;
            clData[5] = (ushort)iLen;
            WriteUshortArray(clData);
            writer.Write(plainText);
        }
        public void WriteCell(int row, int col, int value)
        {
            ushort[] clData = { 0x027E, 10, 0, 0, 0 };
            clData[2] = (ushort)row;
            clData[3] = (ushort)col;
            WriteUshortArray(clData);
            int iValue = (value << 2) | 2;
            writer.Write(iValue);
        }
        public void WriteCell(int row, int col, double value)
        {
            ushort[] clData = { 0x0203, 14, 0, 0, 0 };
            clData[2] = (ushort)row;
            clData[3] = (ushort)col;
            WriteUshortArray(clData);
            writer.Write(value);
        }
        public void WriteCell(int row, int col)
        {
            ushort[] clData = { 0x0201, 6, 0, 0, 0x17 };
            clData[2] = (ushort)row;
            clData[3] = (ushort)col;
            WriteUshortArray(clData);
        }
        public void BeginWrite()
        {
            WriteUshortArray(clBegin);
        }
        public void EndWrite()
        {
            WriteUshortArray(clEnd);
            writer.Flush();
        }
    }
}