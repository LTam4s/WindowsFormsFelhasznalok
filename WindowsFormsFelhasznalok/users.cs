using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsFelhasznalok
{
    internal class users
    {
        int id;
        string name;
        DateTime date;
        string pfp;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public DateTime Date { get => date; set => date = value; }
        public string Pfp { get => pfp; set => pfp = value; }

        public users(int id, string name, DateTime date, string pfp)
        {
            Id = id;
            Name = name;
            Date = date;
            Pfp = pfp;
        }

        public override string ToString()
        {
            return id + " | " + name;
        }
    }
}
