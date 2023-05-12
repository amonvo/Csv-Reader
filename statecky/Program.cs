/*
 * Created by SharpDevelop.
 * User: amonv
 * Date: 11.08.2022
 * Time: 19:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;  
using System.Globalization;
using System.Collections.Generic;
using System.Text;

namespace statecky
{
	class Program
	{
		const int consoleWidth  = 150;
		const int consoleHeight = 40;
		const int naStranku     = consoleHeight - 7;
		
		static ConsoleColor fColorText   = ConsoleColor.White;
		static ConsoleColor fColorLine   = ConsoleColor.Red;
		static ConsoleColor fColorHeader = ConsoleColor.Cyan;
		static ConsoleColor fColorFooter = ConsoleColor.Cyan;
		static ConsoleColor fColorSelected = ConsoleColor.White;
		
		public static string[] hlavicka;
		public static int[] sirkySloupcu = new int[7];
		
		public static string[] odbocky = {"┌", "┬", "┐", "├", "┼", "┤", "└", "┴", "┘"};	
		
		public static string[] napoveda = {" 00/00 ",
										   " F1-help ",
										   " F2-filtr kontinent ", 
										   " F3-filtr název ", 
										   " PgUp-předchozí stránka ", 
										   " PgDown-další stránka ", 
										   " Esc-zruš filtry "};
		
		public static string[] helpPage = {" F1-help ",
										   " F2-filtr kontinent - písmenem zvolíte kontinent", 
										   " F3-filtr název - filtrujte napsáním prvních několika písmen názvu", 
										   " PgUp-předchozí stránka ", 
										   " PgDown-další stránka ", 
										   " Esc-zruš filtry - skoč do původního zobrazení států"};
		
		public static string[] kontinenty = {" 00/00 ",
										     " F1-help ",
										     " A-Afrika ",
										     " E-Evropa ", 
										     " I-Asie ",
										     " J-Jižní Amerika ",
											 " O-Oceánie ",										     
										     " S-Severní Amerika ", 
										   	 " PgUp-zpět ", 
										     " PgDown-dále ", 
										     " Esc-zruš"};
		
		public static string[] nazvy = {" 00/00 ",
									     " F1-help ",
									     " PgUp-zpět ", 
									     " PgDown-dále ", 
									     " Esc-zruš"};
		
		public static List<Stat> staty = new List<Stat>();
		public static List<Stat> statySort = new List<Stat>();	
		public static int zemi;
		public static int stranek;
		public static int stranka;
		public static int mod;
		public static string filtr;
		public static bool radit = true;
		public static int celkove;
		
		public static void Main(string[] args)
		{			
			createLists();
			mod = 0; //0 - standardní seznam, 1 - help stránka, 2 - filtr kontinentů, 3 - filtr názvů
			filtr = "";
			
			while(true)
			{
				prepareConsole();
				
				switch(mod) 
				{
					case 0:
					case 2:
					case 3:
						initiateData();
						filterData();
						orderData();
							
						printHeader();
						printList();
	    		    	break;	
    		    	case 1:
	    		    	printHelpPage();
	    		    	break;	
				}	
				
				// patička
				switch(mod) 
				{
					case 2:
						printFooter("kontinent");
	    		    	break;	
    		    	case 3:
	    		    	printFooter("nazev");
	    		    	break;
	    		    default:
	    		    	printFooter();	
	    		    	break;
				}				
				
				// ovládání
				checkKeys(mod);
				
				Console.Clear();
			}			
		}
		
		public static void checkKeys(int mode)
		{
			//char docasny;
			
			switch(mode) 
			{
				case 0:
					switch(Console.ReadKey().Key) 
					{
					  case ConsoleKey.PageDown:
						if (stranka < stranek) stranka++;				    
					    break;
					  case ConsoleKey.PageUp:
					    if (stranka > 1) stranka--;
					    break;
					  case ConsoleKey.F1:
					    mod = 1;
					    break;
					  case ConsoleKey.F2:
					    mod = 2;
					    break;
					  case ConsoleKey.F3:
					    mod = 3;
					    break;
					  case ConsoleKey.F4:
					    if (radit) radit = false;
					    	else radit = true;					    
					    break;
				      case ConsoleKey.Escape:				    
					    mod = 0;
					    filtr = "";	
						stranka = 1;
					    break;
					}
    		    	break;	
		    	case 1:
    		    	switch(Console.ReadKey().Key) 
					{
					  case ConsoleKey.F2:
					    mod = 2;					    
					    break;
					  case ConsoleKey.F3:
					    mod = 3;
					    break;
				      case ConsoleKey.Escape:				    
					    mod = 0;
						filtr = "";	
						stranka = 1;					    
					    break;
					}
    		    	break;
    		    case 2:
    		    	switch(Console.ReadKey().Key) 
					{
					  case ConsoleKey.PageDown:
						if (stranka < stranek) stranka++;				    
					    break;
					  case ConsoleKey.PageUp:
					    if (stranka > 1) stranka--;
					    break;
					  case ConsoleKey.F1:
					    mod = 1;
					    break;
					  case ConsoleKey.F4:
					    if (radit) radit = false;
					    	else radit = true;					    
					    break;					    
				      case ConsoleKey.Escape:				    
					    mod = 0;
						filtr = "";	
						stranka = 1;						
					    break;
				      case ConsoleKey.A:				    
					    filtr = "Afrika";					    
					    break;
					    case ConsoleKey.E:				    
					    filtr = "Evropa";					    
					    break;
					    case ConsoleKey.I:				    
					    filtr = "Asie";					    
					    break;
					    case ConsoleKey.J:				    
					    filtr = "Jižní Amerika";					    
					    break;
					    case ConsoleKey.O:				    
					    filtr = "Oceánie";					    
					    break;
					    case ConsoleKey.S:				    
					    filtr = "Severní Amerika";					    
					    break;					    
					}
    		    	break;
    		    case 3:
    		    	var character = Console.ReadKey();
    		    	switch(character.Key) 
					{
					  case ConsoleKey.PageDown:
						if (stranka < stranek) stranka++;				    
					    break;
					  case ConsoleKey.PageUp:
					    if (stranka > 1) stranka--;
					    break;
					  case ConsoleKey.F1:
					    mod = 1;
					    break;
					  case ConsoleKey.F4:
					    if (radit) radit = false;
					    	else radit = true;					    
					    break;					    
				      case ConsoleKey.Escape:				    
					    mod = 0;
					    filtr = "";
					    if (nazvy.Length == 6) {
    		    			Array.Resize(ref nazvy, 5);    		    			
    		    		}
					    stranka = 1;
					    break;
					}
    		    	if (Char.IsLetter(character.KeyChar)) filtr = filtr + character.KeyChar.ToString();
    		    	if (filtr.Length > 0) {
    		    		if (nazvy.Length < 6) {
    		    			Array.Resize(ref nazvy, 6);
    		    			nazvy[5] = " Hledáš: " + filtr + " ";
    		    		} else nazvy[5] = " Hledáš: " + filtr + " ";
    		    	}
    		    	break;
			}			
		}
		
		public static void createLists()
		{
			//load csv			
		    string line;
		    string[] row;
		    
		    StreamReader sr = new StreamReader("staty_sveta.csv");	    
		        
			line = sr.ReadLine();
			hlavicka = line.Split(';');	
			for(int i = 0; i <= 6; i++)
			{
				sirkySloupcu[i] = hlavicka[i].Length;
			}			
			
		    while(true)
			{
			    line = sr.ReadLine();
			    if(line == null) break;
			    
		    	row = line.Split(';');	
				for(int i = 0; i <= 6; i++)
				{
					if (row[i].Length > sirkySloupcu[i]) {
						sirkySloupcu[i] = row[i].Length;
					}					
				}
				
		    	staty.Add(new Stat(row[0], row[1], row[2], row[4], row[5], row[6]));
		    	statySort.Add(new Stat(row[0], row[1], row[2], row[4], row[5], row[6]));
		    	++zemi;
			}	
		    
		    stranka = 1;
		    stranek = (int) Math.Ceiling((decimal) zemi/((decimal) naStranku));
		}
		
		public static void prepareConsole()
		{
			Console.WindowWidth  = consoleWidth;
			Console.WindowHeight = consoleHeight;
		}
		
		// position: 0 = top, 1 = middle, 2 = bottom
		public static void hLine(int position, int vertikala)
		{
			int xPozice = 0;
			int yPozice = vertikala;
			int mezery = 4;	
			
			Console.SetCursorPosition(xPozice, yPozice);
			printLine(odbocky[position * 3 + 0]);
			
			for(int i = 0; i <= 6; i++)
			{
				if(i == 3) continue;
				
				for(int j = 0; j < sirkySloupcu[i] + mezery; j++)
					printLine("─");				
				
				xPozice += sirkySloupcu[i];
				
				if (i == 0) {
					mezery = 4;					
				} else {
					mezery = 3;
				}
				xPozice += mezery;						
				
				Console.SetCursorPosition(xPozice, yPozice);	
				
				if (i < 6) printLine(odbocky[position * 3 + 1]);							
			}
			printLine(odbocky[position * 3 + 2]);
		}
		
    	public static void printHeader()
		{
			int xPozice = 0;
			int yPozice = 1;			
			int mezery = 4;			

			hLine(0, 0);
			
			Console.SetCursorPosition(xPozice, yPozice);
			printLine("│");
			
			for(int i = 0; i <= 6; i++)
			{
				if(i == 3) continue;
				
				Console.Write(" ");					
					
				Console.ForegroundColor = fColorHeader;					
				Console.Write(hlavicka[i]);				
				Console.ForegroundColor = fColorText;
				
				xPozice += sirkySloupcu[i];
				
				if (i == 0) {
					mezery = 4;					
				} else {
					mezery = 3;
				}
				xPozice += mezery;					

				Console.SetCursorPosition(xPozice, yPozice);	
				
				printLine("│");				
			}	
						
			hLine(1, 2);	
		}
    	public static void initiateData()
    	{    		
    		statySort.Clear();
    		
    		foreach (var stat in staty) {
    			statySort.Add(new Stat(stat.Nazev, stat.Kontinent, stat.Zkratka, stat.HlavniMesto, stat.PocetObyvatel, stat.Rozloha));
    		}
    	}
    	
    	public static void filterData()
    	{
			int i = 0; 
			int j = 0;
			int[] toRemove = new int[200];
    				
    		switch(mod) {
    			case 2:    				
    				if (filtr != "") {
	    				foreach(var stat in statySort) {
	    					if(stat.Kontinent != filtr) {
	    						toRemove[j] = i;
	    						j++;
	    					}
	    					i++;
	    				}    					
    				}
    				
    				Array.Resize(ref toRemove, j);    				
    				
    				for (int k=(toRemove.Length-1); k >= 0; k-- ) {
    					statySort.RemoveAt(toRemove[k]);
    				}
    				break;
    				
    			case 3:
    				if (filtr != "") {
	    				foreach(var stat in statySort) {
    						if(!stat.Nazev.ToLower().StartsWith(filtr.ToLower())) {
	    						toRemove[j] = i;
	    						j++;
	    					}
	    					i++;
	    				}    					
    				}  
    				
    				Array.Resize(ref toRemove, j);    				
    				
    				for (int k=(toRemove.Length-1); k >= 0; k-- ) {
    					statySort.RemoveAt(toRemove[k]);
    				}
    				break;
    		}
    		stranek = (int) Math.Ceiling((decimal) statySort.Count/((decimal) naStranku));    
    	}
    	
    	public static void orderData()
    	{
    		int i; 
			int moved;			
			string pomocny;			
			
			if(radit) {
				celkove = 0;
				do {
					moved = 0;
					for (i = 0; i < statySort.Count-1; i++) {
						if(String.Compare(statySort[i].Nazev, statySort[i+1].Nazev) > 0) {
							pomocny = statySort[i].Nazev;
							statySort[i].Nazev = statySort[i+1].Nazev;
							statySort[i+1].Nazev = pomocny;
							
							pomocny = statySort[i].Kontinent;
							statySort[i].Kontinent = statySort[i+1].Kontinent;
							statySort[i+1].Kontinent = pomocny;
							
							pomocny = statySort[i].Zkratka;
							statySort[i].Zkratka = statySort[i+1].Zkratka;
							statySort[i+1].Zkratka = pomocny;
							
							pomocny = statySort[i].HlavniMesto;
							statySort[i].HlavniMesto = statySort[i+1].HlavniMesto;
							statySort[i+1].HlavniMesto = pomocny;
							
							pomocny = statySort[i].PocetObyvatel;
							statySort[i].PocetObyvatel = statySort[i+1].PocetObyvatel;
							statySort[i+1].PocetObyvatel = pomocny;
							
							pomocny = statySort[i].Rozloha;
							statySort[i].Rozloha = statySort[i+1].Rozloha;
							statySort[i+1].Rozloha = pomocny;

							moved++;
							celkove++;
						}
					}
				} while(moved > 0);				
			}
    	}
    	
    	public static void printList()		
		{		
			int xPozice = 0;
			int yPozice = 3;
			int poradi = 0;
			int start;
			int end;
			
			start = (stranka-1) * naStranku;
			end = stranka * naStranku - 1;
			
			Console.SetCursorPosition(xPozice, yPozice);			
			
			foreach(var stat in statySort)
			{		
				if((poradi >= start) && (poradi <= end)) {
					printLine("│");

					Console.Write(" " + stat.Nazev);
					xPozice += sirkySloupcu[0] + 4;
					Console.SetCursorPosition(xPozice, yPozice);
					printLine("│");
					
					Console.Write(" " + stat.Kontinent);
					xPozice += sirkySloupcu[1] + 3;
					Console.SetCursorPosition(xPozice, yPozice);
					printLine("│");
					
					Console.Write(" " + stat.Zkratka);
					xPozice += sirkySloupcu[2] + 3;
					Console.SetCursorPosition(xPozice, yPozice);
					printLine("│");
					
					Console.Write(" " + stat.HlavniMesto);
					xPozice += sirkySloupcu[4] + 3;
					Console.SetCursorPosition(xPozice, yPozice);
					printLine("│");
					
					Console.Write(String.Format(" {0," + sirkySloupcu[5] + "}", stat.PocetObyvatel));
					xPozice += sirkySloupcu[5] + 3;
					Console.SetCursorPosition(xPozice, yPozice);
					printLine("│");
					
					Console.Write(String.Format(" {0," + sirkySloupcu[6] + "}", stat.Rozloha));
					xPozice += sirkySloupcu[6] + 3;
					Console.SetCursorPosition(xPozice, yPozice);
					printLine("│");				
					
					xPozice = 0;
					yPozice += 1;
					Console.SetCursorPosition(xPozice, yPozice);					
				}
				poradi++;
			}
			hLine(2, yPozice);
			
		}
    	
    	public static void printFooter(string druhNapovedy = "")
    	{
    		int xPozice = 0;
    		int yPozice = consoleHeight-3;
    		bool selected = false;
    		
    		switch(druhNapovedy) 
			{
			  case "kontinent":
			    Console.SetCursorPosition(xPozice, yPozice);			    
			    printLine("┌");
			    for(int i = 0; i < kontinenty.Length; i++) 
			    {
			    	for(int j = 0; j < kontinenty[i].Length; j++) printLine("─");
			    	if (i < kontinenty.Length - 1) printLine("┬");
			    }			    
			    printLine("┐");
			    
			    xPozice = 0;
			    yPozice += 1;
			    Console.SetCursorPosition(xPozice, yPozice);
			    printLine("│");
			    for(int i = 0; i < kontinenty.Length; i++) 
			    {
			    	if(i == 0) {			    		
			    		printHelp(" " + stranka.ToString().PadLeft(2, '0') + "/" + stranek.ToString().PadLeft(2, '0') + " ");
			    	} else {
			    		if (kontinenty[i].IndexOf(filtr) > 0) selected = true;
			    			else selected = false;
			    		
			    		printHelp(kontinenty[i], selected);
			    	}
			    	printLine("│");
			    }			    
			    	
			    xPozice = 0;
			    yPozice += 1;
			    Console.SetCursorPosition(xPozice, yPozice);
			    printLine("└");
			    for(int i = 0; i < kontinenty.Length; i++) 
			    {
			    	for(int j = 0; j < kontinenty[i].Length; j++) printLine("─");
			    	if (i < kontinenty.Length - 1) printLine("┴");
			    }			    
			    printLine("┘");	
			    break;
			  case "nazev":
			    Console.SetCursorPosition(xPozice, yPozice);			    
			    printLine("┌");			    
			    for(int i = 0; i < nazvy.Length; i++) 
			    {
			    	for(int j = 0; j < nazvy[i].Length; j++) printLine("─");
			    	if (i < nazvy.Length - 1) printLine("┬");
			    }			    
			    printLine("┐");
			    
			    xPozice = 0;
			    yPozice += 1;
			    Console.SetCursorPosition(xPozice, yPozice);
			    printLine("│");
			    
			    for(int i = 0; i < nazvy.Length; i++) 
			    {
			    	if(i == 0) {			    		
			    		printHelp(" " + stranka.ToString().PadLeft(2, '0') + "/" + stranek.ToString().PadLeft(2, '0') + " ");
			    	} else {
			    		if (i == 5) selected = true;
			    			else selected = false;
			    		
			    		printHelp(nazvy[i], selected);
			    	}
			    	printLine("│");
			    }			    
			    	
			    xPozice = 0;
			    yPozice += 1;
			    Console.SetCursorPosition(xPozice, yPozice);
			    printLine("└");
			    for(int i = 0; i < nazvy.Length; i++) 
			    {
			    	for(int j = 0; j < nazvy[i].Length; j++) printLine("─");
			    	if (i < nazvy.Length - 1) printLine("┴");
			    }			    
			    printLine("┘");	
			    break;
			  default:
			    // zaklad			    
			    Console.SetCursorPosition(xPozice, yPozice);			    
			    printLine("┌");
			    for(int i = 0; i < napoveda.Length; i++) 
			    {
			    	for(int j = 0; j < napoveda[i].Length; j++) printLine("─");
			    	if (i < napoveda.Length - 1) printLine("┬");
			    }			    
			    printLine("┐");
			    
			    xPozice = 0;
			    yPozice += 1;
			    Console.SetCursorPosition(xPozice, yPozice);
			    printLine("│");
			    for(int i = 0; i < napoveda.Length; i++) 
			    {
			    	if(i == 0) {			    		
			    		printHelp(" " + stranka.ToString().PadLeft(2, '0') + "/" + stranek.ToString().PadLeft(2, '0') + " ");
			    	} else {
			    		printHelp(napoveda[i]);
			    	}
			    	printLine("│");
			    }			    
			    	
			    xPozice = 0;
			    yPozice += 1;
			    Console.SetCursorPosition(xPozice, yPozice);
			    printLine("└");
			    for(int i = 0; i < napoveda.Length; i++) 
			    {
			    	for(int j = 0; j < napoveda[i].Length; j++) printLine("─");
			    	if (i < napoveda.Length - 1) printLine("┴");
			    }			    
			    printLine("┘");			    
			    break;
			}    		
    	}    	
    	
    	public static void printLine(string vZnak)
    	{
    		Console.ForegroundColor = fColorLine;
			Console.Write(vZnak);
			Console.ForegroundColor = fColorText;
    	}
    	
    	public static void printHelp(string helpText, bool selected = false)
    	{
    		if (!selected) Console.ForegroundColor = fColorFooter;
    			else Console.ForegroundColor = fColorSelected;
			Console.Write(helpText);
			Console.ForegroundColor = fColorText;
    	}
    	
    	public static void printHelpPage()
    	{
			int xPozice = 0;
			int yPozice = 0;
			
			Console.SetCursorPosition(xPozice, yPozice);
			printLine(odbocky[0]);			
			for(int i = 0; i < consoleWidth-3; i++) printLine("─");
			printLine(odbocky[2]);

			emptyLine(yPozice);
			
			foreach(var veta in helpPage) {
				xPozice = 0;
				++yPozice;
				Console.SetCursorPosition(xPozice, yPozice);
				printLine("│");
				Console.Write(" " + veta);
				xPozice = consoleWidth-2;
				Console.SetCursorPosition(xPozice, yPozice);
				printLine("│");				
			}
			
			emptyLine(yPozice);
			
			xPozice = 0;
			++yPozice;
			Console.SetCursorPosition(xPozice, yPozice);
			printLine(odbocky[6]);			
			for(int i = 0; i < consoleWidth-3; i++) printLine("─");
			printLine(odbocky[8]);
    	}
    	
    	public static void emptyLine(int yPozice) {
    		int xPozice = 0;
    		
    		++yPozice;
			Console.SetCursorPosition(xPozice, yPozice);
			printLine("│");
			xPozice = consoleWidth-2;
			Console.SetCursorPosition(xPozice, yPozice);
			printLine("│");
    	}
	}
}