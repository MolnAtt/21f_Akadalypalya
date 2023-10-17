using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace _21f_Akadalypalya
{
	internal class Program
	{
		/// <summary>
		/// Ez a függvény megmondja, hogy az i-edik sor j-edik eleméből milyen más pontok (=számpárok) elérhetők.
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <returns></returns>
		static List<(int, int)> Szomszédai(string[] map, int i, int j) 
		{
			int N = map.Length; // sorok száma
			int M = map[0].Length; // egy tetszőleges sor hossza = oszlopok száma

			List<(int, int)> szomszedok = new List<(int, int)>();

			if (0 < i && map[i - 1][j] != 'X') // É
			{
				szomszedok.Add((i - 1, j));
				//Console.WriteLine(map[i-1][j]);
			}
			if (0 < j && map[i][j-1]!='X')// NY
			{
				szomszedok.Add((i, j - 1));
				//Console.WriteLine(map[i][j-1]);
			}
			if (i < N - 1 && map[i + 1][j] != 'X') // D
			{
				szomszedok.Add((i + 1, j));
				//Console.WriteLine(map[i+ 1][j]);

			}
			if (j < M - 1 && map[i][j + 1] != 'X') // K
			{
				szomszedok.Add((i, j+1));
				//Console.WriteLine(map[i][j + 1]);
            }

			return szomszedok;
		}

		static List<(int, int)> Legrovidebb_ut(string[] map, (int, int) start, (int, int) end)
		{

			int N = map.Length;
			int M = map[0].Length;

			int fehér = 0;  // abszolút érintetlen
			int kék = 1;    // már találkoztam vele csak még nem dolgoztam fel (tennivalók közt ott van)
			int piros = 2;  // feldolgoztam, nincs már vele több munka (tennivalókból kikerült)

			int[,] szin = new int[N,M]; // ennek most mindegyik értéke nulla, azaz FEHÉR.

			Queue<(int, int)> tennivalok = new Queue<(int, int)>();

			tennivalok.Enqueue(start);
			
			szin[start.Item1, start.Item2] = kék;

			(int, int)[,] honnan = new (int, int)[N,M];
			for (int i = 0; i < N; i++)
			{
				for (int j = 0; j < M; j++)
				{
					honnan[i,j] = (-2,-2);
				}
			}
			honnan[start.Item1, start.Item2] = (-1,-1);


			while (tennivalok.Count != 0)
			{
				(int, int) tennivalo = tennivalok.Dequeue();

				// FELDOLGOZÁS

				if (tennivalo == end)
				{
					return Honnan_vektor_felgongyolitese(honnan, end);
				}
				szin[tennivalo.Item1, tennivalo.Item2] = piros;


				// SZOMSZÉDAIVAL FOGLALKOZUNK/TÖLTJÜK A TENNIVALÓKAT

				foreach ((int,int) szomszed in Szomszédai(map, tennivalo.Item1, tennivalo.Item2))
				{
					if (szin[szomszed.Item1, szomszed.Item2] == fehér)
					{
						tennivalok.Enqueue(szomszed);
						szin[szomszed.Item1, szomszed.Item2] = kék;
						honnan[szomszed.Item1, szomszed.Item2] = tennivalo; // kulcsfontosságú elem
					}
				}
			}

			return null;

		}

		static List<(int, int)> Honnan_vektor_felgongyolitese((int,int)[,] honnan, (int,int) end)
		{
			List<(int, int)> result = new List<(int, int)>();

			(int, int) node = end;

			while (node != (-1,-1))
			{
				result.Add(node);
				node = honnan[node.Item1, node.Item2];
			}

			result.Reverse();
			return result;
		}

		static void Kiir(string[] map, List<(int, int)> ut)
		{
			int N = map.Length;
			int M = map[0].Length;

			for (int i = 0; i < N; i++)
			{
				for (int j = 0; j < M; j++)
				{
					if (ut.Contains((i,j)))
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.Write("o");
						Console.ForegroundColor = ConsoleColor.White;
					}
					else
					{
						Console.Write(map[i][j]);
					}
				}
                Console.WriteLine();
            }
		}

		static void Main(string[] args)
		{
			string[] map = File.ReadAllLines("input.txt");

			// ez mutatja meg, hogy a 7. sor 5. oszlopa mi: map[7][5]

			(int, int) S = (7, 5);
			(int, int) G = (11, 16);
			List<(int, int)> ut = Legrovidebb_ut(map, S, G);

            Console.WriteLine(string.Join(" -> ", ut));

			Kiir(map, ut);
        }
	}
}

/*
..........X........
..XXXXXXX.X..X.....
..X.....X.X..X.....
..X.....X.X..X.....
........X....X.X...
........X....X.X...
...X....X....X.X...
...X.S..X....X.X...
.X.X....X....X.XXXX
.X.XXXXXXXXXXX.X...
.X......X......X...
.X......X...X..XG..
.XXXXXXXX...X..XXX.
............X......
............X......
..........XXXXXXXXX
........X..........
........X..........
*/