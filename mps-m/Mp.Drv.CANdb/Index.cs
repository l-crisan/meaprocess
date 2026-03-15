using System;
using System.Collections.Generic;
using System.Text;

namespace Mp.Drv.CANdb
{
    public class Index
    {
        private int _db = -1;
        private int _msg = -1;
        private int _sig = -1;

        public Index(int db, int msg, int sig)
        {
            _db = db;
            _msg = msg;
            _sig = sig;
        }

        public Index(int db, int msg)
        {
            _db = db;
            _msg = msg;
        }

        public Index(int db)
        {
            _db = db;
        }

        public int DB
        {
            get { return _db; }
        }

        public int MSG
        {
            get { return _msg; }
        }

        public int Sig
        {
            get { return _sig; }
        }
    }
}
