using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_pokusaj_2
{
    class Cvor
    {
        public int c;
        public List<Cvor> susedi;
        public int dubina;
        public Cvor parent;
        public bool matched;

        public Cvor(int c)
        {
            this.c = c;
            susedi = new List<Cvor>();
            matched = false;
        }

        public Cvor(int c, Cvor p, int d)
        {
            this.c = c;
            this.parent = p;
            this.dubina = d;
            matched = false;
            this.susedi = new List<Cvor>();
        }
    }

    class Grana
    {
        public Cvor a;
        public Cvor b;
        public bool matched;

        public Grana(Cvor a, Cvor b)
        {
            this.a = a;
            this.b = b;
            matched = false;
        }
        public bool contains(Cvor c)
        {
            if (c.c == a.c || c.c == b.c)
            {
                return true;
            }
            return false;
        }
        public Cvor vratidrugi(Cvor c)
        {
            if (this.contains(c))
            {
                if (c.c == a.c)
                {
                    return b;
                }
                else
                {
                    return a;
                }
            }
            return null;
        }
    }

    class Graf
    {
        public List<Cvor> cvorovi;
        public List<Grana> grane;

        public Graf(List<Cvor> cvorovi, List<Grana> grane)
        {
            this.cvorovi = cvorovi;
            this.grane = grane;
            foreach (Cvor v in cvorovi)
            {
                for (int i = 0; i < grane.Count; i++)
                {
                    if (grane[i].contains(v))
                    {
                        v.susedi.Add(grane[i].vratidrugi(v));
                    }
                }
            }
        }

        public Cvor vratiPara(Cvor a)
        {
            foreach (Grana g in grane)
            {
                if (g.contains(a) && g.matched)
                {
                    return g.vratidrugi(a);
                }
            }
            return null;
        }
        public void stampaj()
        {
            foreach (Grana grn in grane)
            {
                if (grn.matched)
                {
                    Console.WriteLine(grn.a.c + " " + grn.b.c);
                }
            }
        }
    }
    class Drvo : Graf
    {
        Stack<Pupoljak> pup;
        public int dubina;
        public Graf g;
        public Put put;
        public Drvo(Graf g) : base(new List<Cvor>(), new List<Grana>())
        {
            dubina = 0;
            this.g = g;
            pup = new Stack<Pupoljak>();
        }
        public bool contains(Cvor v)
        {
            foreach (Cvor c in cvorovi)
            {
                if (c.c == v.c)
                {
                    return true;
                }
            }
            return false;
        }
        public Cvor vrati(Cvor v)
        {
            foreach (Cvor c in cvorovi)
            {
                if (c.c == v.c)
                {
                    return c;
                }
            }
            return null;
        }
        public void Add(Cvor v1)
        {
            foreach (Grana gr in g.grane)
            {
                if (gr.contains(v1) && cvorovi.Contains(gr.vratidrugi(v1)))
                {
                    grane.Add(gr);
                    v1.parent = gr.vratidrugi(v1);
                }
            }
            v1.dubina = dubina;
            cvorovi.Add(v1);
        }
        public Cvor nciklus(Cvor a, Cvor b)
        {
            List<Cvor> r = new List<Cvor>();
            List<Cvor> p1 = new List<Cvor>();
            p1.Add(a);
            List<Cvor> p2 = new List<Cvor>();
            p2.Add(b);
            bool u1 = true;
            bool u2 = true;
            while (u1 || u2)
            {
                if (p1.Last<Cvor>().parent != null)
                {
                    p1.Add(p1.Last<Cvor>().parent);
                }
                else
                {
                    u1 = false;
                }
                if (p2.Last<Cvor>().parent != null)
                {
                    p2.Add(p2.Last<Cvor>().parent);
                }
                else
                {
                    u2 = false;
                }
            }
            List<Cvor> presek = new List<Cvor>();
            foreach (Cvor v1 in p1)
            {
                if (p2.Contains(v1))
                {
                    presek.Add(v1);
                }
            }
            int max = -1;
            Cvor zr = presek[0];
            foreach (Cvor v1 in presek)
            {
                if (max < v1.c)
                {
                    max = v1.c;
                    zr = v1;
                }
            }
            List<Cvor> putt = new List<Cvor>();
            putt.Add(a);
            for (int i = 0; i < p1.Count; i++)
            {
                if (p1[i].dubina > zr.dubina)
                {
                    putt.Add(p1[i]);
                }
            }
            for (int i = p2.Count - 1; i > 0; i++)
            {
                if (p2[i].dubina > zr.dubina)
                {
                    putt.Add(p2[i]);
                }
            }
            putt.Add(b);
            List<Grana> gt1 = new List<Grana>();
            foreach (Grana gr in grane)
            {
                foreach (Cvor cc in putt)
                {
                    if (gr.contains(cc) && !gt1.Contains(gr))
                    {
                        gt1.Add(gr);
                    }
                }
            }
            max = -1;
            foreach (Cvor cc in cvorovi)
            {
                if (cc.c > max)
                {
                    max = cc.c;
                }
            }
            skupi(putt, zr.c, max + 1, gt1);
            return new Cvor(max + 1);
        }
        public void prviPut()
        {
            put = new Put(this.g);
            put.Add(cvorovi.Last<Cvor>());
            while (put.cvorovi.Last<Cvor>().parent != null)
            {
                put.Add(put.cvorovi.Last<Cvor>().parent);
            }
        }
        public void skupi(List<Cvor> ciklus, int koren, int super, List<Grana> gr)
        {
            prviPut();
            pup.Push(new Pupoljak(koren, super, ciklus, gr));
            foreach (Cvor v in ciklus)
            {
                if (cvorovi.Contains(v))
                {
                    cvorovi.Remove(v);
                    g.cvorovi.Remove(v);
                }
            }
            Cvor ss = new Cvor(super);
            cvorovi.Add(ss);
            g.cvorovi.Add(ss);
            foreach (Grana gg in gr)
            {
                foreach (Cvor cc in ciklus)
                {
                    Grana asdf = new Grana(ss, gg.vratidrugi(cc));
                    if (gg.contains(cc) && !grane.Contains(asdf))
                    {
                        grane.Add(asdf);
                        g.grane.Add(asdf);
                    }
                }
            }
            foreach (Grana gg in gr)
            {
                if (grane.Contains(gg))
                {
                    grane.Remove(gg);
                    g.grane.Remove(gg);
                }
            }
        }
        public void procvetaj()
        {
            while (pup.Count > 0)
            {
                Pupoljak temp = pup.Pop();
                List<Cvor> p1 = new List<Cvor>();
                List<Cvor> p2 = new List<Cvor>();
                bool uslov = true;
                foreach (Cvor cc in put.cvorovi)
                {
                    if (cc.c != temp.super)
                    {
                        if (uslov)
                        {
                            p1.Add(cc);
                        }
                        else
                        {
                            p2.Add(cc);
                        }
                    }
                };
                List<Cvor> util = new List<Cvor>();
                foreach (Grana grn in temp.grane)
                {
                    if (grn.contains(p2[0]))
                    {
                        util.Add(grn.vratidrugi(p2[0]));
                    }
                }
                int max = -1;
                Cvor uu = new Cvor(max);
                foreach (Cvor cc in util)
                {
                    if (max < cc.dubina)
                    {
                        max = cc.dubina;
                        uu = cc;
                    }
                }
                int smer = 0;
                foreach (Grana grn in temp.grane)
                {
                    if (grn.contains(uu) && grn.matched)
                    {
                        smer = temp.cvorovi.IndexOf(uu) - temp.cvorovi.IndexOf(grn.vratidrugi(uu));
                    }
                }
                List<Cvor> util2 = new List<Cvor>();
                int i = temp.cvorovi.IndexOf(uu);
                while (uu.c != temp.koren)
                {
                    util2.Add(uu);
                    i += smer;
                    if (i < 0)
                    {
                        i = temp.cvorovi.Count - 1;
                    }
                    else if (i == temp.cvorovi.Count)
                    {
                        i = 0;
                    }
                    uu = temp.cvorovi[i];
                }
                util2.Reverse();
                put.prilagodi(p1, util2, p2);
                put.invert();
            }
        }
    }
    class Put : Drvo
    {
        public Put(Graf g) : base(g) { }
        public void invert()
        {
            foreach (Grana gr in g.grane)
            {
                gr.matched = !gr.matched;
            }
            foreach (Cvor cc in g.cvorovi)
            {
                cc.matched = true;
            }
        }
        public new void Add(Cvor c)
        {
            this.cvorovi.Add(c);
        }
        public void prilagodi(List<Cvor> levo, List<Cvor> srednje, List<Cvor> desno)
        {
            this.cvorovi = new List<Cvor>();
            foreach (Cvor cc in levo)
            {
                cvorovi.Add(cc);
            }
            foreach (Cvor cc in srednje)
            {
                cvorovi.Add(cc);
            }
            foreach (Cvor cc in desno)
            {
                cvorovi.Add(cc);
            }
        }
    }
    class Pupoljak : Graf
    {
        public int koren;
        public int super;
        public Pupoljak(int koren, int super, List<Cvor> cvorovi, List<Grana> grane) : base(cvorovi, grane)
        {
            this.koren = koren;
            this.super = super;
        }
    }
    class Program
    {
        static void Blossom(Graf g)
        {
            List<Cvor> slobodni = g.cvorovi;
            while (slobodni.Count > 0)
            {
                Cvor r = slobodni.First<Cvor>();
                slobodni.Remove(r);
                Queue<Cvor> q = new Queue<Cvor>();
                q.Enqueue(r);
                Drvo t = new Drvo(g);
                while (q.Count > 0)
                {
                    bool uslov = true;
                    Cvor v = q.Dequeue();
                    foreach (Cvor w in v.susedi)
                    {
                        if (!t.contains(w) && w.matched)
                        {
                            t.Add(w);
                            t.Add(t.g.vratiPara(w));
                            q.Enqueue(t.g.vratiPara(w));
                            if (uslov)
                            {
                                t.dubina++;
                                uslov = false;
                            }
                        }
                        else if (t.contains(w) && t.dubina == t.vrati(w).dubina)
                        {
                            q.Enqueue(t.nciklus(w, t.vrati(w)));
                        }
                        else if (slobodni.Contains(w))
                        {
                            t.Add(w);
                            slobodni.Remove(w);
                            t.procvetaj();
                        }
                    }
                }
            }
            g.stampaj();
        }
        static void Main(string[] args)
        {
            List<Cvor> a = new List<Cvor>();
            a.Add(new Cvor(0));
            a.Add(new Cvor(1));
            a.Add(new Cvor(2));
            a.Add(new Cvor(3));
            List<Grana> b = new List<Grana>();
            b.Add(new Grana(a[0], a[1]));
            b.Add(new Grana(a[1], a[2]));
            b.Add(new Grana(a[1], a[3]));
            b.Add(new Grana(a[2], a[3]));

            Graf g = new Graf(a, b);

            Blossom(g);

            Console.ReadLine();
        }
    }
}
