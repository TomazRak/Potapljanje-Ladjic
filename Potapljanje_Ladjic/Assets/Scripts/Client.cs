using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.IO;

public class Client : MonoBehaviour {
	private bool obstaja;
	//private TcpClient vticnica=new TcpClient();
	private TcpClient vticnica;
	private NetworkStream ns;
	private StreamWriter writer;
	private StreamReader reader;

	public bool isHost=false;

	private static Client instance;
	public static Client Instance{
		get {
			if ( instance == null)
			{
				instance = FindObjectOfType<Client>();
				#if UNITY_EDITOR
				if (FindObjectsOfType<Client>().Length > 1)
				{
					Debug.LogError("There is more than one game controller in the scene");
				}
				#endif
			}
			return instance;
		}
	}


	public bool povezan(string host){
		if (obstaja) {
			return false;
			//nocemo se povezat 2x
		}

		try {

			//Debug.Log(host+" "+port);
			//IPAddress ip=IPAddress.Parse(host);
			vticnica=new TcpClient(host, 1234);
			//vticnica.Connect(host, 1234);
			ns=vticnica.GetStream();
			writer=new StreamWriter(ns);
			reader=new StreamReader(ns);
			obstaja=true;
		}

		catch (Exception ex){
			Debug.Log (ex.Message);

		}

		return obstaja;
	}

	private void Update(){
		if (obstaja) {
			if (ns.DataAvailable) {
				string msg = reader.ReadLine ();
				if (msg != null) {
					Beri (msg);
				}
			}
		}
	}

	public void Poslji(string msg){
		if (!obstaja) {
			return;
		}

		writer.WriteLine (msg);
		writer.Flush ();
	}

	private void Beri(string msg){
		//Poslji (((isHost) ? 1 : 0).ToString ());
		var gc=FindObjectOfType<GameController>();
		//Debug.Log ("client");
		//Debug.Log (msg);

		gc.Sprejmi (msg);

	}

	private void OnApplicationQuit(){
		ZapriVticnico ();

	}

	private void OnDisable(){
		ZapriVticnico ();
	}

	private void ZapriVticnico(){
		if (!obstaja) {
			return;
		}

		writer.Close ();
		reader.Close ();
		vticnica.Close ();
		obstaja = false;
	}
}

public class uporabnik {
	public string ime;
	public bool isHost;
	//tukaj lahko damo tudi borde za igralca
}
