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
		Debug.Log (nekis);
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
}


public class Povezava {
	public string clinetIme;
	public TcpClient tcp;

	public Povezava(TcpClient t){
		this.tcp = t;
	}
}