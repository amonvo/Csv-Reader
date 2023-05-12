/*
 * Created by SharpDevelop.
 * User: amonv
 * Date: 11.08.2022
 * Time: 19:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace statecky
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class Stat
	{
		private string nazev;
		private string kontinent;
		private string zkratka;
		private string hlavniMesto;
		private string pocetObyvatel;
		private string rozloha;
		
		public Stat(string n, string k, string z, string hM, string pO, string r)
		{
			nazev = n;
			kontinent = k;
			zkratka = z;
			hlavniMesto = hM;
			pocetObyvatel = pO;
			rozloha = r;
		}
		
		public string Nazev
		{
			get { return nazev; }
    		set { nazev = value; }
		}
	
		public string Kontinent
		{
			get { return kontinent; }
    		set { kontinent = value; }
		}		
		
		public string Zkratka
		{
			get { return zkratka; }
    		set { zkratka = value; }
		}	
		
		public string HlavniMesto
		{
			get { return hlavniMesto; }
    		set { hlavniMesto = value; }
		}	
			
		public string PocetObyvatel
		{
			get { return pocetObyvatel; }
    		set { pocetObyvatel = value; }
		}	
		
		public string Rozloha
		{
			get { return rozloha; }
    		set { rozloha = value; }
		}	
		
	}
}
