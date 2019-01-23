using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Caesar
{
    public class Cypher
    {
        string _alphabetrus = "абвгдежзийклмнопрстуфхцчшщъыьэюя";
        string _alphabeteng = "abcdefghijklmnopqrstuvwxyz";
        string _alphabetrusup = "АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

        string _alphabetengup = "ABCDEFGHIJKLMNOPQRSTUVWXXYZ";

        //  double[] freqeng = {.0796,.016,.0284,.0401,.1286,.0262,.0199,.0539,.0777,.0016,.0041,.0351,
        //                      .0243,.0751,.0662,.0181,.0017,.0683,.0662,.0972,.0248,.0115,.018,.0017,
        //                                                                               .0152,.0005 };
        //  double[] freqrus = {.0629,.0114,.0355,.0083,.0265,.0732,.0079,.0133,.0577,.0125,.0302,.0299,
        //                      .0275,.049,.0764,.033,.0459,.0404,.0549,.0222,.0036,.0048,.0021,.0094,
        //                                             .0026,.0042,.0003,.0143,.0138,.0023,.0081,.0153};
        string _symbols = "!@#$%^&*()_+{}[]:;'//,.?№ ";

        public SortedDictionary<char, double> Alphabetrusdict = new SortedDictionary<char, double>
        {
            ['а'] = .0629, ['б'] = .0114, ['в'] = .0355,
            ['г'] = .0083, ['д'] = .0265, ['е'] = .0732,
            ['ж'] = .0079, ['з'] = .0133, ['и'] = .0577,
            ['й'] = .0125, ['к'] = .0302, ['л'] = .0299,
            ['м'] = .0275, ['н'] = .049, ['о'] = .0764,
            ['п'] = .033, ['р'] = .0459, ['с'] = .0404,
            ['т'] = .0549, ['у'] = .0222, ['ф'] = .0036,
            ['х'] = .0048, ['ц'] = .0021, ['ч'] = .0094,
            ['ш'] = .0026, ['щ'] = .0042, ['ъ'] = .0003,
            ['ы'] = .0143, ['ь'] = .0138, ['э'] = .0023,
            ['ю'] = .0081, ['я'] = .0153
        };



        public SortedDictionary<char, double> MakeDict(string textin)
        {
            string text = textin.ToLower();

            var x = text.GroupBy(c => c)
                .Select(g => new {g, count = g.Count()})
                .OrderBy(t => t.g.Key)
                .Select(t => new {Value = t.g.Key, Count = t.count});

            var xdict = x.ToDictionary(c => c.Value, c => c.Count);

            //SortedDictionary<char, double>[] moves = new []
            //{
            //    new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),new SortedDictionary<char, double>(),
            //};
            var move = new SortedDictionary<char, double>();
            for (int i = 0; i < 32; i++)
            {
                double val = 0.00;
                char ky = Alphabetrusdict.ElementAt(i).Key;

                if (xdict.ContainsKey(ky))
                {
                    val = (double) xdict[ky] / (double) text.Length;

                    move.Add(Alphabetrusdict.ElementAt(i).Key,
                        Math.Abs(val - Alphabetrusdict[ky]));
                }
                else
                {
                    move.Add(Alphabetrusdict.ElementAt(i).Key, val);
                }

            }

            return move;

        }

        public string MovedText(string inText, int key)
        {
            string IN = inText;
            string buf = "";

            for (int i = 0; i < IN.Length; i++)
            {
                foreach (var item in _symbols)
                {
                    if (IN[i] == item)
                    {
                        buf += item;
                    }
                }

                for (int j = 0; j < _alphabetrus.Length; j++)
                {
                    if (IN[i] == _alphabetrus[j])
                    {
                        int move = j + key;
                        while (move >= _alphabetrus.Length)
                        {
                            move -= _alphabetrus.Length;
                        }

                        if (move < 0) move = _alphabetrus.Length + move;
                        buf = buf + _alphabetrus[move];
                    }
                }

                for (int j = 0; j < _alphabetrusup.Length; j++)
                {
                    if (IN[i] == _alphabetrusup[j])
                    {
                        int move = j + key;
                        if (move < 0) move = _alphabetrusup.Length + move;
                        while (move >= _alphabetrusup.Length)
                        {
                            move -= _alphabetrusup.Length;
                        }

                        buf = buf + _alphabetrusup[move];
                    }
                }

                for (int j = 0; j < _alphabeteng.Length; j++)
                {
                    if (IN[i] == _alphabeteng[j])
                    {
                        int move = j + key;
                        if (move < 0) move = _alphabeteng.Length + move;
                        while (move >= _alphabeteng.Length)
                        {
                            move -= _alphabeteng.Length;
                        }

                        buf = buf + _alphabeteng[move];
                    }
                }

                for (int j = 0; j < _alphabetengup.Length; j++)
                {
                    if (IN[i] == _alphabetengup[j])
                    {
                        int move = j + key;
                        if (move < 0) move = _alphabetengup.Length + move;
                        while (move >= _alphabetengup.Length)
                        {
                            move -= _alphabetengup.Length;
                        }

                        buf = buf + _alphabetengup[move];
                    }
                }

            }

            string moved = buf;
            return moved;
        }



        public string MakeE(string dtext)
        {
            string text = dtext;
            text.Replace('ё', 'е');
            text.Replace('Ё', 'Е');
            return text;
        }

    }
}