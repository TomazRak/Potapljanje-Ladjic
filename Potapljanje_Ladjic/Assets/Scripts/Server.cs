using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.IO;


public class Server : MonoBehaviour {

	public int port = 1234;

	private List<Povezava> povezave;
	private List<Povezava> odstranjeni;
	public int[,] matrikaA = new int[10, 10];
	public int[,] matrikaB = new int[10, 10];
	int SteviloPovezav = 0;

	private TcpListener server;
	private bool started;


	public void init(){

		DontDestroyOnLoad (gameObject);
		povezave = new List<Povezava> ();
		odstranjeni = new List<Povezava> ();

		try {
			//IPAddress ip=IPAddress.Parse("127.0.0.1");
			server=new TcpListener(IPAddress.Any, 1234);
			server.Start();
			poslusaj();
			started=true;

		}

		catch (Exception ex) {
			Debug.Log (ex.Message);
		}

	}

	private void Update(){

		if (started == false) {
			return;
		}

		foreach (Povezava pov in povezave) {
			if (!jePovezan (pov.tcp)) {
				pov.tcp.Close ();
				odstranjeni.Add (pov);
				continue;
			} else {
				NetworkStream ns = pov.tcp.GetStream ();
				if (ns.DataAvailable) {
					StreamReader reader = new StreamReader (ns, true);
					string msg = reader.ReadLine ();

					if (msg != null) {
						Sprejmi (pov, msg);
					}
				}
			}
		}

		for (int i = 0; i < odstranjeni.Count - 1; i++) {
			povezave.Remove (odstranjeni [i]);
			odstranjeni.RemoveAt (i);
		}
	}

	private void Broadcast(string msg, List<Povezava> pl){
		foreach (Povezava pove in pl) {
			try{
				StreamWriter writer=new StreamWriter(pove.tcp.GetStream());
				writer.WriteLine(msg);
				writer.Flush();
			}

			catch (Exception ex){
				Debug.Log (ex.Message);
			}
		}
	}

	private void Sprejmi(Povezava tc, string nekis){
		//Debug.Log (nekis);
		Broadcast (nekis, povezave);
		//tc.isHost = (nekis == "0" ? false : true);
	}

	private void poslusaj(){
					server.BeginAcceptTcpClient (AcceptTcpClient, server);
	}

	private void AcceptTcpClient(IAsyncResult ar){
		TcpListener li = (TcpListener)ar.AsyncState;
		Povezava po = new Povezava (li.EndAcceptTcpClient (ar));
		povezave.Add (po);

		poslusaj();

		Debug.Log("Povezava je uspela");
		SteviloPovezav++;
		if (SteviloPovezav == 2) {
			MatrikeDefault ();
			NapolniMatirkeRandom1 ();
			NapolniMatirkeRandom2 ();
		}


	}

	private bool jePovezan(TcpClient c){
		try
		{
			if(c!=null && c.Client!=null && c.Client.Connected){
				if (c.Client.Poll(0,SelectMode.SelectRead)){
					return !(c.Client.Receive(new byte[1], SocketFlags.Peek)==0);
				}

				return true;
			}

			else {
				return false;
			}
		}
		catch {
			return false;
		}
	}


	public void MatrikeDefault() {
		for (int i = 0; i < 10; i++) {
			for(int j = 0; j < 10; j++)
			{
				matrikaA[i, j] = 0;
				matrikaB[i, j] = 0;
			}
		}

		//player1.Matrika [0, 0] = 1;
		//player2.Matrika [0, 0] = 1;
	}

	public void NapolniMatirkeRandom1() {
		// recimo imaš ladjice naslednjih dolžin, ki jih moraš razporediti v polje
		var ladjice = new List<int> { 2, 2, 2, 2, 3, 3, 3, 4, 4, 5 };

		// tvoj random number generator
		var r = new System.Random();

		// razporedimo vsako ladjo
		foreach (int l in ladjice)
		{
			bool vstavljeno = false;
			do
			{

				// določi začetne koordinate randomly
				int x = r.Next(0, matrikaA.GetLength(0) - 1);
				int y = r.Next(0, matrikaA.GetLength(1) - 1);

				// če je lokacija x,y zasedena, še enkrat določi točke
				if (matrikaA[x, y] != 0) continue;

				// določi začetno smer randomly
				int smer = r.Next(0, 3);

				// preveri če lahko vstaviš to ladjo, sicer ponovi postopek
				int dolzina = l - 1;
				bool jeProsto = true;
				switch (smer)
				{
				case 0:
					// levo

					// preveri meje
					if (x - dolzina < 0) continue;

					// preveri prostost vseh lokacij levo od začetne lokacije
					for (int i = x; i >= x - dolzina; i--)
					{
						// če najdeš katerokoli lokacijo, ki ni prosta, treba ponoviti vse skupaj
						if (matrikaA[i, y] != 0)
						{
							jeProsto = false;
							break;
						}
					}
					if (jeProsto == false) continue;

					// vstavi v polje
					for (int i = x; i >= x - dolzina; i--)
						matrikaA[i, y] = l;
					break;
				case 1:
					// dol
					if (y + dolzina >= matrikaA.GetLength(1)) continue;

					for (int i = y; i <= y + dolzina; i++)
						if (matrikaA[x, i] != 0)
						{
							jeProsto = false;
							break;
						}
					if (jeProsto == false) continue;
					for (int i = y; i <= y + dolzina; i++)
						matrikaA[x, i] = l;
					break;
				case 2:
					// desno
					if (x + dolzina >= matrikaA.GetLength(0)) continue;

					for (int i = x; i <= x + dolzina; i++)
						if (matrikaA[i, y] != 0)
						{
							jeProsto = false;
							break;
						}
					if (jeProsto == false) continue;
					for (int i = x; i <= x + dolzina; i++)
						matrikaA[i, y] = l;
					break;
				case 3:
					// gor
					if (y - dolzina < 0) continue;

					for (int i = y; i >= y - dolzina; i--)
						if (matrikaA[x, i] != 0)
						{
							jeProsto = false;
							break;
						}
					if (jeProsto == false) continue;
					for (int i = y; i >= y - dolzina; i--)
						matrikaA[x, i] = l;

					break;
				}

				vstavljeno = true;             
			}
			while (vstavljeno == false);
		}


		for (int i = 0; i < 10; i++) {
			for (int j = 0; j < 10; j++) {
				string posljiMA = "M1|" + i + "|" + j + "|" + matrikaA [i, j];
				Broadcast (posljiMA, povezave);
			}
		}


	}

	public void NapolniMatirkeRandom2()
	{
		// recimo imaš ladjice naslednjih dolžin, ki jih moraš razporediti v polje
		var ladjice = new List<int> { 2, 2, 2, 2, 3, 3, 3, 4, 4, 5 };

		// tvoj random number generator
		var r = new System.Random();

		// razporedimo vsako ladjo
		foreach (int l in ladjice)
		{
			bool vstavljeno = false;
			do
			{

				// določi začetne koordinate randomly
				int x = r.Next(0, matrikaB.GetLength(0) - 1);
				int y = r.Next(0, matrikaB.GetLength(0) - 1);

				// če je lokacija x,y zasedena, še enkrat določi točke
				if (matrikaB[x, y] != 0) continue;

				// določi začetno smer randomly
				int smer = r.Next(0, 3);

				// preveri če lahko vstaviš to ladjo, sicer ponovi postopek
				int dolzina = l - 1;
				bool jeProsto = true;
				switch (smer)
				{
				case 0:
					// levo

					// preveri meje
					if (x - dolzina < 0) continue;

					// preveri prostost vseh lokacij levo od začetne lokacije
					for (int i = x; i >= x - dolzina; i--)
					{
						// če najdeš katerokoli lokacijo, ki ni prosta, treba ponoviti vse skupaj
						if (matrikaB[i, y] != 0)
						{
							jeProsto = false;
							break;
						}
					}
					if (jeProsto == false) continue;

					// vstavi v polje
					for (int i = x; i >= x - dolzina; i--)
						matrikaB[i, y] = l;
					break;
				case 1:
					// dol
					if (y + dolzina >= matrikaB.GetLength(1)) continue;

					for (int i = y; i <= y + dolzina; i++)
						if (matrikaB[x, i] != 0)
						{
							jeProsto = false;
							break;
						}
					if (jeProsto == false) continue;
					for (int i = y; i <= y + dolzina; i++)
						matrikaB[x, i] = l;
					break;
				case 2:
					// desno
					if (x + dolzina >= matrikaB.GetLength(0)) continue;

					for (int i = x; i <= x + dolzina; i++)
						if (matrikaB[i, y] != 0)
						{
							jeProsto = false;
							break;
						}
					if (jeProsto == false) continue;
					for (int i = x; i <= x + dolzina; i++)
						matrikaB[i, y] = l;
					break;
				case 3:
					// gor
					if (y - dolzina < 0) continue;

					for (int i = y; i >= y - dolzina; i--)
						if (matrikaB[x, i] != 0)
						{
							jeProsto = false;
							break;
						}
					if (jeProsto == false) continue;
					for (int i = y; i >= y - dolzina; i--)
						matrikaB[x, i] = l;

					break;
				}

				vstavljeno = true;
			}
			while (vstavljeno == false);
		}


		for (int i = 0; i < 10; i++) {
			for (int j = 0; j < 10; j++) {
				string posljiMB = "M2|" + i + "|" + j + "|" + matrikaA [i, j];
				Broadcast (posljiMB, povezave);
			}
		}



	}
    public void reset_matrike()
    {

        if (SteviloPovezav == 2)
        {
            MatrikeDefault();
            NapolniMatirkeRandom1();
            NapolniMatirkeRandom2();
        }
    }

}


public class Povezava {
	public bool isHost;
	public TcpClient tcp;

	public Povezava(TcpClient t){
		this.tcp = t;
	}
}